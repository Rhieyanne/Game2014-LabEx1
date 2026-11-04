using UnityEngine;

// Pearls, chests and oxygen tanks. They float down, touch one to collect it.
public class Pickup : MonoBehaviour
{
    public int points = 50;
    public int extraLives = 0;  // 1 for the oxygen tank
    public float fallSpeed = 1.5f;
    public float radius = 0.4f;  // pickup range
    public AudioClip collectSound;

    Vector3 normalScale;

    void Start()
    {
        normalScale = transform.localScale;
    }

    void Update()
    {
        // float down and pulse a bit so it stands out
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        transform.localScale = normalScale * (1f + 0.1f * Mathf.Sin(Time.time * 6f));

        PlayerBehaviour player = PlayerBehaviour.Instance;
        if (player != null && Vector2.Distance(transform.position, player.transform.position) < radius + player.radius)
        {
            Collect();
            return;
        }

        if (ScreenBounds.IsOffscreen(transform.position))
            Destroy(gameObject);
    }

    void Collect()
    {
        ScoreManager.AddPoints(points);
        ScoreManager.AddLife(extraLives);
        SoundFX.Play(collectSound);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
