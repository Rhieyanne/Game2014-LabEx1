using UnityEngine;

public class BackgroundBehaviour : MonoBehaviour
{
    public float scrollSpeed = 1.5f;
    public float tileHeight = 5.12f;

    void Update()
    {
        transform.position += Vector3.down * scrollSpeed * Time.deltaTime;

        if (transform.position.y <= -tileHeight)
        {
            transform.position += Vector3.up * tileHeight;
        }
    }
}
