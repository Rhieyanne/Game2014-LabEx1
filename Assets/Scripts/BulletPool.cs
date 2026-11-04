using System.Collections.Generic;
using UnityEngine;

// Object pool for bullets - same idea as the EnemyPool from Lab 2.
// Makes a bunch of bullets at the start and switches them on and off instead of
// calling Instantiate / Destroy on every shot. If a queue runs out, it asks the
// factory for one more.
public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; }

    public BulletFactory factory;  // if left empty, uses the one on this object

    [Header("Starting Pool Sizes")]
    public int playerBulletCount = 20;
    public int enemyBulletCount = 10;

    Queue<Projectile> playerBullets = new Queue<Projectile>();
    Queue<Projectile> enemyBullets = new Queue<Projectile>();

    void Awake()
    {
        Instance = this;
        if (factory == null)
            factory = GetComponent<BulletFactory>();

        if (factory == null)
        {
            Debug.LogError("BulletPool: needs a BulletFactory to make bullets");
            enabled = false;
            return;
        }

        // fill the pools up front
        for (int i = 0; i < playerBulletCount; i++)
            AddBullet(BulletType.Player);

        for (int i = 0; i < enemyBulletCount; i++)
            AddBullet(BulletType.Enemy);
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    void AddBullet(BulletType type)
    {
        if (factory == null) return;

        Projectile bullet = factory.CreateBullet(type, transform);
        if (bullet != null)
            GetQueue(type).Enqueue(bullet);
    }

    // take a bullet out of the pool and send it off from this spot
    public Projectile GetBullet(BulletType type, Vector3 position, Quaternion rotation)
    {
        Queue<Projectile> queue = GetQueue(type);

        if (queue.Count == 0)
            AddBullet(type);  // ran out, make one more

        if (queue.Count == 0)
            return null;  // factory couldn't make one (no prefab set)

        Projectile bullet = queue.Dequeue();
        bullet.transform.SetPositionAndRotation(position, rotation);
        bullet.gameObject.SetActive(true);
        return bullet;
    }

    // switch it off and put it back in line so it can be fired again
    public void ReturnBullet(Projectile bullet)
    {
        if (!bullet.gameObject.activeSelf)
            return;  // already back in the pool, don't add it twice

        bullet.gameObject.SetActive(false);
        GetQueue(bullet.type).Enqueue(bullet);
    }

    Queue<Projectile> GetQueue(BulletType type)
    {
        return type == BulletType.Player ? playerBullets : enemyBullets;
    }
}
