 using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;


public class PlayerBehaviour : MonoBehaviour
{

    public static PlayerBehaviour Instance;

    [Header("Movement")]
    public float moveSpeed = 12f;
    public float radius = 0.4f;  // hit circle size
    public float edgePadding = 0.5f;  // how far to stay from the screen edges
    public float maxTilt = 20f;  // max lean angle

    [Header("Weapon")]
    public GameObject torpedoPrefab;
    public Transform firePoint;  // empty child at the nose
    public float fireCooldown = 0.2f;  // time between shots

    [Header("Getting Hit")]
    public float invincibleTime = 1f;  // how long you blink after getting hit
    public GameObject explosionPrefab;

    [Header("Sounds")]
    public AudioClip shootSound;
    public AudioClip hurtSound;
    public AudioClip explodeSound;

    SpriteRenderer sprite;
    float nextFireTime;
    float invincibleUntil;
    float tilt;
    TouchControl activeTouch;  // the finger the sub is following (null = none)

    void Awake()
    {
        Instance = this;
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        float startX = transform.position.x;

        // touch on the phone, mouse in the editor
        if (!ReadTouch())
            ReadMouse();

        KeepOnScreen();
        Lean(transform.position.x - startX);
        Blink();
    }

    // Follows the newest finger. check every finger instead of just primaryTouch because
    // spam tapping in the simulator could leave that touch stuck, and then the sub stopped responding.
    // BIG BUG FIX HERE DOWN HERE!!!!
    // remember the touch itself, not its touchId: the Device Simulator gives every click the
    // same touchId, so matching by id also matched the old stuck touch and the sub slid back to it.
    // A stuck touch never gets "pressed this frame" again, so it can't be picked anymore.
    bool ReadTouch()
    {
        if (Touchscreen.current == null) return false;

        // a finger just came down: shoot, and follow that finger from now on
        foreach (TouchControl touch in Touchscreen.current.touches)
        {
            if (touch.press.wasPressedThisFrame)
            {
                activeTouch = touch;
                Fire();
            }
        }

        // finger lifted (or the simulator swapped touchscreens): stop following it
        if (activeTouch == null || activeTouch.device != Touchscreen.current || !activeTouch.press.isPressed)
        {
            activeTouch = null;
            return false;
        }

        // move toward that finger while it's still down
        MoveTowards(activeTouch.position.ReadValue());
        return true;
    }

    void ReadMouse()
    {
        if (Mouse.current == null || !Mouse.current.leftButton.isPressed) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
            Fire();  // new click = shoot
        MoveTowards(Mouse.current.position.ReadValue());
    }

    void MoveTowards(Vector2 screenPosition)
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(screenPosition);
        target.z = 0f;
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    void Fire()
    {
        if (torpedoPrefab == null) return;  // nothing assigned yet, ok i have made a prefab
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireCooldown;

        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;
        Instantiate(torpedoPrefab, spawnPosition, Quaternion.identity);  // goes straight up
        SoundFX.Play(shootSound);
    }

    void KeepOnScreen()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, ScreenBounds.Left + edgePadding, ScreenBounds.Right - edgePadding);
        position.y = Mathf.Clamp(position.y, ScreenBounds.Bottom + edgePadding, ScreenBounds.Top - edgePadding);
        transform.position = position;
    }

    // tilt the sub when it moves sideways, then level out
    void Lean(float sidewaysMove)
    {
        float sidewaysSpeed = sidewaysMove / Mathf.Max(Time.deltaTime, 0.001f);
        float targetTilt = Mathf.Clamp(-sidewaysSpeed * 2f, -maxTilt, maxTilt);
        tilt = Mathf.Lerp(tilt, targetTilt, 10f * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, tilt);
    }

    // flash on and off while invincible so you can tell you got hit
    void Blink()
    {
        bool invincible = Time.time < invincibleUntil;
        sprite.enabled = !invincible || Mathf.Repeat(Time.time * 10f, 1f) > 0.5f;
    }

    // sharks, enemy bullets and mines call this
    public void TakeDamage(int amount)
    {
        if (Time.time < invincibleUntil) return;  // still blinking, ignore it

        ScoreManager.lives = Mathf.Max(ScoreManager.lives - amount, 0);
        SoundFX.Play(hurtSound);

        if (ScoreManager.lives == 0)
            Explode();
        else
            invincibleUntil = Time.time + invincibleTime;
    }

    void Explode()
    {
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 1f);  // get rid of it after the animation
        }
        SoundFX.Play(explodeSound);

        Instance = null;  // enemies stop chasing, spawners stop
        gameObject.SetActive(false);  // ScoreManager sees 0 lives and does the game over
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
