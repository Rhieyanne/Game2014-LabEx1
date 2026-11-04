using UnityEngine;

// Torpedoes and enemy bullets. Flies the way it's pointing and hits whatever it touches.
// They come from the BulletPool (built by the BulletFactory) and go back to the pool
// when they hit something or leave the screen, instead of being destroyed.
public class Projectile : MonoBehaviour
{
    public float speed = 12f;
    public int damage = 1;
    public float radius = 0.2f;  // hit circle size
    public BulletType type = BulletType.Player;  // set by the factory: Player hits enemies, Enemy hits the player

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;

        bool hitSomething = type == BulletType.Player ? CheckHitEnemies() : CheckHitPlayer();

        if (hitSomething || ScreenBounds.IsOffscreen(transform.position))
            ReturnToPool();
    }

    // bullet vs enemy collision - the enemy takes damage, and gives the points if this kills it
    bool CheckHitEnemies()
    {
        foreach (Enemy enemy in Enemy.All)
        {
            if (!enemy.IsDead && IsTouching(enemy.transform.position, enemy.radius))
            {
                enemy.TakeDamage(damage);
                return true;  // one shot, one enemy
            }
        }
        return false;
    }

    bool CheckHitPlayer()
    {
        PlayerBehaviour player = PlayerBehaviour.Instance;
        if (player != null && IsTouching(player.transform.position, player.radius))
        {
            player.TakeDamage(damage);
            return true;
        }
        return false;
    }

    // circle collision, no physics - two things touch if the distance between them
    // is less than their radiuses added together
    bool IsTouching(Vector3 otherPosition, float otherRadius)
    {
        return Vector2.Distance(transform.position, otherPosition) < radius + otherRadius;
    }

    void ReturnToPool()
    {
        if (BulletPool.Instance != null)
            BulletPool.Instance.ReturnBullet(this);
        else
            Destroy(gameObject);  // no pool in the scene, just get rid of it
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
