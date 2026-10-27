using UnityEngine;


[RequireComponent(typeof(Enemy))]
public class EnemySubAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float slowDownDistance = 1.5f;  // starts slowing down this close to its spot
    public float stayTime = 8f;  // how long before it leaves

    [Header("Line of Sight")]
    public float sightRange = 10f;
    public float sightAngle = 15f;  // half the width of the vision cone, in degrees

    [Header("Weapon")]
    public GameObject bulletPrefab;
    public Transform firePoint;  // empty child at the front
    public float fireCooldown = 1.5f;
    public AudioClip shootSound;

    Enemy enemy;
    float hoverHeight;
    float leaveTime;
    float nextFireTime;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    void Start()
    {
        hoverHeight = Random.Range(ScreenBounds.Top - 3f, ScreenBounds.Top - 1.5f);
        leaveTime = Time.time + stayTime;
        nextFireTime = Time.time + 1f;  // small wait before the first shot
    }

    void Update()
    {
        PlayerBehaviour player = PlayerBehaviour.Instance;

        // time's up (or the player's dead), dive off the bottom
        if (player == null || Time.time > leaveTime)
        {
            enemy.Steer(Vector2.down * moveSpeed * 2f);
            return;
        }

        // arrive - head to a spot lined up with the player and slow down as it gets close,
        // so it stops smoothly instead of overshooting
        Vector2 target = new Vector2(player.transform.position.x, hoverHeight);
        Vector2 toTarget = target - (Vector2)transform.position;
        float speed = moveSpeed * Mathf.Clamp01(toTarget.magnitude / slowDownDistance);
        enemy.Steer(toTarget.normalized * speed);

        // only shoot if it can see you
        if (CanSeePlayer(player) && Time.time >= nextFireTime)
        {
            Fire(player);
            nextFireTime = Time.time + fireCooldown;
        }
    }

    // line of sight - the player has to be in range AND inside the cone in front of the sub
    bool CanSeePlayer(PlayerBehaviour player)
    {
        Vector2 toPlayer = player.transform.position - transform.position;
        bool inRange = toPlayer.magnitude < sightRange;
        bool inFront = Vector2.Angle(Vector2.down, toPlayer) < sightAngle;  // sub faces down
        return inRange && inFront;
    }

    void Fire(PlayerBehaviour player)
    {
        if (bulletPrefab == null) return;  // nothing assigned yet

        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;

        // aim at the player - Atan2 gives the angle, but bullets fly "up" and Atan2's zero
        // points right, so take away 90
        Vector3 toPlayer = player.transform.position - spawnPosition;
        float angle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg - 90f;

        Instantiate(bulletPrefab, spawnPosition, Quaternion.Euler(0f, 0f, angle));
        SoundFX.Play(shootSound);
    }

    void OnDrawGizmosSelected()
    {
        // draw the vision cone
        Gizmos.color = Color.magenta;
        Vector3 leftEdge = Quaternion.Euler(0f, 0f, -sightAngle) * Vector3.down * sightRange;
        Vector3 rightEdge = Quaternion.Euler(0f, 0f, sightAngle) * Vector3.down * sightRange;
        Gizmos.DrawLine(transform.position, transform.position + leftEdge);
        Gizmos.DrawLine(transform.position, transform.position + rightEdge);
    }
}
