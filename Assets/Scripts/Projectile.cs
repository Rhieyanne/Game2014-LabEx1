using UnityEngine;

// Torpedoes and enemy bullets. Flies the way it's pointing and hits whatever it touches.
public class Projectile : MonoBehaviour
{
    public float speed = 12f;
    public int damage = 1;
    public float radius = 0.2f;  // hit circle size
    public bool firedByPlayer = true;  // on = hits enemies, off = hits the player

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;

        if (firedByPlayer)
            CheckHitEnemies();
        else
            CheckHitPlayer();

        if (ScreenBounds.IsOffscreen(transform.position))
            Destroy(gameObject);
    }

    void CheckHitEnemies()
    {
        foreach (Enemy enemy in Enemy.All)
        {
            if (!enemy.IsDead && IsTouching(enemy.transform.position, enemy.radius))
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
                return;  // one shot, one enemy
            }
        }
    }

    void CheckHitPlayer()
    {
        PlayerBehaviour player = PlayerBehaviour.Instance;
        if (player != null && IsTouching(player.transform.position, player.radius))
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    // circle collision, no physics - two things touch if the distance between them
    // is less than their radiuses added together
    bool IsTouching(Vector3 otherPosition, float otherRadius)
    {
        return Vector2.Distance(transform.position, otherPosition) < radius + otherRadius;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
