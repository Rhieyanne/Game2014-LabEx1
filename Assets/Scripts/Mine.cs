using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class Mine : MonoBehaviour
{
    [Header("Movement")]
    public float driftSpeed = 1f;
    public float swayAmount = 0.8f;  // side to side
    public float spinSpeed = 30f;  // degrees per second

    [Header("Detection & Explosion")]
    public float detectionRadius = 1.8f;
    public float fuseTime = 0.8f;  // time from arming to boom
    public float blastRadius = 1.6f;
    public int blastDamage = 1;
    public AudioClip armSound;

    Enemy enemy;
    Vector3 normalScale;
    float swayOffset;
    bool armed;
    float fuseTimer;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        normalScale = transform.localScale;
        swayOffset = Random.Range(0f, 10f);  // so they don't all sway together
    }

    void Update()
    {
        // drift down, sway, spin
        float sway = Mathf.Sin(Time.time * 2f + swayOffset) * swayAmount;
        enemy.Steer(new Vector2(sway, -driftSpeed));
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);

        PlayerBehaviour player = PlayerBehaviour.Instance;

        if (!armed)
        {
            // detection radius - arm when the player gets close
            if (player != null && Vector2.Distance(transform.position, player.transform.position) < detectionRadius)
            {
                armed = true;
                fuseTimer = fuseTime;
                SoundFX.Play(armSound);
            }
        }
        else
        {
            // pulse fast while the fuse is going
            fuseTimer -= Time.deltaTime;
            transform.localScale = normalScale * (1f + 0.25f * Mathf.Abs(Mathf.Sin(Time.time * 20f)));

            if (fuseTimer <= 0f)
                Explode(player);
        }
    }

    void Explode(PlayerBehaviour player)
    {
        if (player != null && Vector2.Distance(transform.position, player.transform.position) < blastRadius)
            player.TakeDamage(blastDamage);

        enemy.Die(false);  // no points, the player didn't shoot it
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = new Color(1f, 0.5f, 0f);  // orange
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
