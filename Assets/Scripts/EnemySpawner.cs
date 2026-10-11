using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyPool enemyPool;
    public float spawnInterval = 0.8f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;

            float height = Camera.main.orthographicSize;
            float width = height * Camera.main.aspect;
            float x = Random.Range(-width + 0.5f, width - 0.5f);

            enemyPool.GetEnemy(new Vector3(x, height + 1f, 0f));
        }
    }
}
