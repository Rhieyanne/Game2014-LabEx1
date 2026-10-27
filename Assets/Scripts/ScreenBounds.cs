using UnityEngine;

// Screen edges in world units, worked out from the main camera.
public static class ScreenBounds
{
    public static float Left   => Camera.main.transform.position.x - HalfWidth;
    public static float Right  => Camera.main.transform.position.x + HalfWidth;
    public static float Top    => Camera.main.transform.position.y + HalfHeight;
    public static float Bottom => Camera.main.transform.position.y - HalfHeight;

    // camera size = half the screen height, times the aspect ratio = half the width
    static float HalfHeight => Camera.main.orthographicSize;
    static float HalfWidth  => Camera.main.orthographicSize * Camera.main.aspect;

    // true if something's gone way off screen (used to delete stuff)
    public static bool IsOffscreen(Vector3 position, float margin = 2f)
    {
        return position.x < Left - margin || position.x > Right + margin
            || position.y < Bottom - margin || position.y > Top + margin;
    }
}
