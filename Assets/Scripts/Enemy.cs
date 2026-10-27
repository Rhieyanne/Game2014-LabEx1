using System.Collections.Generic;
using UnityEngine;

// Stuff every enemy shares (shark, enemy sub, mine): health, hit flash, explosion, points, loot.
// The AI scripts decide where it wants to go, and this script moves it there.
public class Enemy : MonoBehaviour
{
    // every enemy that's alive right now, torpedoes check this list
    public static List<Enemy> All = new List<Enemy>();

    [Header("Health & Score")]
    public int health = 2;
    public int points = 100;
    public float radius = 0.5f;  // hit circle size

    [Header("Steering")]
    public float acceleration = 6f;  // how fast it can change direction

    [Header("Random Loot")]
    public GameObject[] lootDrops;  // what it might drop
    [Range(0f, 1f)] public float dropChance = 0.3f;

    [Header("Effects")]
    public GameObject explosionPrefab;
    public AudioClip hitSound;
    public AudioClip explodeSound;

    [HideInInspector] public Vector2 velocity;

    public bool IsDead { get; private set; }

    SpriteRenderer sprite;
    Color normalColor;
    float flashTimer;

    void OnEnable()
    {
        All.Add(this);
    }

    void OnDisable()
    {
        All.Remove(this);
    }

    void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        normalColor = sprite.color;
    }

    void Update()
    {
        // flash red for a sec after getting hit
        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;
            sprite.color = flashTimer > 0f ? Color.red : normalColor;
        }

        // delete it once it's off screen
        if (ScreenBounds.IsOffscreen(transform.position))
            Destroy(gameObject);
    }

    // steering - nudge the velocity a bit toward where we want to go every frame
    // instead of snapping to it, that's what makes it turn smoothly
    public void Steer(Vector2 desiredVelocity)
    {
        velocity = Vector2.MoveTowards(velocity, desiredVelocity, acceleration * Time.deltaTime);
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;

        health -= amount;
        flashTimer = 0.1f;

        if (health <= 0)
            Die(true);
        else
            SoundFX.Play(hitSound);
    }

    // killedByPlayer is false when a mine blows itself up, so you only get points for your own kills
    public void Die(bool killedByPlayer)
    {
        if (IsDead) return;
        IsDead = true;

        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 1f);  // get rid of it after the animation
        }
        SoundFX.Play(explodeSound);

        if (killedByPlayer)
        {
            ScoreManager.score += points;
            DropLoot();
        }

        Destroy(gameObject);
    }

    // random loot - sometimes drop a random pickup
    void DropLoot()
    {
        if (lootDrops == null || lootDrops.Length == 0) return;
        if (Random.value > dropChance) return;

        GameObject loot = lootDrops[Random.Range(0, lootDrops.Length)];
        if (loot != null)
            Instantiate(loot, transform.position, Quaternion.identity);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
