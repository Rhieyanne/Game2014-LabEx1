using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    public float moveSpeed = 12f;

    void Update()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            MoveTowards(Touchscreen.current.primaryTouch.position.ReadValue());
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            MoveTowards(Mouse.current.position.ReadValue());
        }
    }

    void MoveTowards(Vector2 screenPosition)
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(screenPosition);
        target.z = 0f;
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }
}
