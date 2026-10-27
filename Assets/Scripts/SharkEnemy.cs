using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class SharkEnemy : MonoBehaviour
{
    [Header("Detection")]
    public float detectionRadius = 3f;  // yellow circle in the Scene view

    [Header("Movement")]
    public float wanderSpeed = 1.5f;
    public float chaseSpeed = 3f;

    [Header("Bite Attack")]
    public float biteRange = 0.8f;
    public int biteDamage = 1;
    public float biteCooldown = 1f;
    public AudioClip biteSound;

    Enemy enemy;
    SpriteRenderer sprite;
    Animator animator;
    Vector2 wanderDirection = Vector2.down;
    float wanderTimer;
    float nextBiteTime;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        PlayerBehaviour player = PlayerBehaviour.Instance;
        bool playerDetected = false;
        Vector2 toPlayer = Vector2.zero;

        // detection radius - is the player close enough to notice?
        if (player != null)
        {
            toPlayer = player.transform.position - transform.position;
            playerDetected = toPlayer.magnitude < detectionRadius;
        }

        if (playerDetected)
            Chase(player, toPlayer);
        else
            Wander();

        // swim animation goes faster when chasing, and flip the sprite to face where it's going
        if (animator != null)
            animator.speed = playerDetected ? 2f : 1f;
        if (Mathf.Abs(enemy.velocity.x) > 0.1f)
            sprite.flipX = enemy.velocity.x < 0f;  // art faces right (switch to > if yours faces left)
    }

    void Chase(PlayerBehaviour player, Vector2 toPlayer)
    {
        // seek - go straight at the player
        enemy.Steer(toPlayer.normalized * chaseSpeed);

        // bite when it's close, then bounce back so it has to come around again
        if (toPlayer.magnitude < biteRange && Time.time >= nextBiteTime)
        {
            player.TakeDamage(biteDamage);
            SoundFX.Play(biteSound);
            nextBiteTime = Time.time + biteCooldown;
            enemy.velocity = -toPlayer.normalized * chaseSpeed;
        }
    }

    void Wander()
    {
        // wander - keep going down, pick a new random angle every so often
        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0f)
        {
            wanderDirection = new Vector2(Random.Range(-1f, 1f), -1f).normalized;
            wanderTimer = Random.Range(0.5f, 1.5f);
        }

        // don't swim off the sides
        if (transform.position.x < ScreenBounds.Left + 0.5f)
            wanderDirection.x = Mathf.Abs(wanderDirection.x);
        if (transform.position.x > ScreenBounds.Right - 0.5f)
            wanderDirection.x = -Mathf.Abs(wanderDirection.x);

        enemy.Steer(wanderDirection * wanderSpeed);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
