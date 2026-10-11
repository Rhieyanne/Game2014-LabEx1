using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int poolSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AddEnemy();
        }
    }

    private void AddEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform);
        enemy.GetComponent<EnemyBehaviour>().pool = this;
        enemy.SetActive(false);
        pool.Enqueue(enemy);
    }

    public GameObject GetEnemy(Vector3 position)
    {
        if (pool.Count < 1)
        {
            AddEnemy();
            poolSize++;
        }

        GameObject enemy = pool.Dequeue();
        enemy.transform.position = position;
        enemy.SetActive(true);
        return enemy;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        pool.Enqueue(enemy);
    }
}
