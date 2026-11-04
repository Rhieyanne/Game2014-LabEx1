using UnityEngine;

// Factory - the only place that knows how to build each kind of bullet.
// The BulletPool asks it for a new one whenever it needs more, so nothing else
// has to know which prefab goes with which BulletType.
public class BulletFactory : MonoBehaviour
{
    [Header("Bullet Prefabs")]
    public GameObject playerBulletPrefab;  // Torpedo
    public GameObject enemyBulletPrefab;  // EnemyBullet

    // builds a brand new bullet of that type, switched off and ready to go in the pool
    public Projectile CreateBullet(BulletType type, Transform parent)
    {
        GameObject prefab = GetPrefab(type);
        if (prefab == null)
        {
            Debug.LogError("BulletFactory: no prefab set for " + type + " bullets");
            return null;
        }

        GameObject bullet = Instantiate(prefab, parent);
        bullet.name = prefab.name;  // drop the "(Clone)" so the hierarchy is easier to read
        bullet.SetActive(false);

        Projectile projectile = bullet.GetComponent<Projectile>();
        if (projectile == null)
        {
            Debug.LogError("BulletFactory: " + prefab.name + " needs a Projectile component");
            Destroy(bullet);
            return null;
        }

        // set it up for its type: player bullets hit enemies, enemy bullets hit the player
        projectile.type = type;
        return projectile;
    }

    GameObject GetPrefab(BulletType type)
    {
        switch (type)
        {
            case BulletType.Player: return playerBulletPrefab;
            case BulletType.Enemy: return enemyBulletPrefab;
            default: return null;
        }
    }
}
