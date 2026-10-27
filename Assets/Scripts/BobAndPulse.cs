using UnityEngine;

public class BobAndPulse : MonoBehaviour
{
    [Header("Move back and forth")]
    public Vector3 moveAmount = new Vector3(0f, 10f, 0f);  // pixels for UI, units for sprites
    public float moveSpeed = 2f;

    [Header("Grow and shrink")]
    public float pulseAmount = 0.05f;  // 0.05 = 5%
    public float pulseSpeed = 3f;

    [Header("Spin")]
    public float spinSpeed = 0f;  // degrees per second

    Vector3 startPosition;
    Vector3 startScale;

    void Start()
    {
        startPosition = transform.localPosition;
        startScale = transform.localScale;
    }

    void Update()
    {
        transform.localPosition = startPosition + moveAmount * Mathf.Sin(Time.time * moveSpeed);
        transform.localScale = startScale * (1f + pulseAmount * Mathf.Sin(Time.time * pulseSpeed));
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
    }
}
