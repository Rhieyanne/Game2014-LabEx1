using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float speed = 3f;
    public EnemyPool pool;

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        if (transform.position.y < -Camera.main.orthographicSize - 1f)
        {
            ScoreManager.score += 10;
            pool.ReturnEnemy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.lives--;
            pool.ReturnEnemy(gameObject);
        }
    }
}
