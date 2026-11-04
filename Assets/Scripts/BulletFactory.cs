using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    [Header("Bullet Prefabs")]
    [SerializeField] private Projectile playerBulletPrefab;  // Torpedo
    [SerializeField] private Projectile enemyBulletPrefab;  // EnemyBullet


    public Projectile CreateBullet(BulletType type, Transform parent)
    {
        Projectile prefab = GetPrefab(type);
        if (prefab == null)
        {
            Debug.LogError($"BulletFactory: no prefab set for {type} bullets", this);
            return null;
        }

        Projectile bullet = Instantiate(prefab, parent);
        bullet.name = prefab.name;
        bullet.type = type;
        bullet.gameObject.SetActive(false);
        return bullet;
    }

    private Projectile GetPrefab(BulletType type)
    {
        switch (type)
        {
            case BulletType.Player: return playerBulletPrefab;
            case BulletType.Enemy: return enemyBulletPrefab;
            default: return null;
        }
    }
}
