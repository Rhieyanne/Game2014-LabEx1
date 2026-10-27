using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] prefabs;

    [Header("Timing")]
    public float startDelay = 2.5f;  // time between spawns at the start
    public float endDelay = 0.8f;  // time between spawns when it's hardest
    public float rampTime = 90f;  // how long it takes to get there

    public float edgeMargin = 0.6f;  // don't spawn right at the edges

    float timer;

    void Start()
    {
        timer = startDelay;
    }

    void Update()
    {
        if (PlayerBehaviour.Instance == null) return;  // player's dead, stop spawning

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Spawn();
            timer = NextDelay();
        }
    }

    // random prefab at a random spot along the top
    void Spawn()
    {
        if (prefabs == null || prefabs.Length == 0) return;  // nothing set up yet

        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
        if (prefab == null) return;  // empty slot

        float x = Random.Range(ScreenBounds.Left + edgeMargin, ScreenBounds.Right - edgeMargin);
        Vector3 position = new Vector3(x, ScreenBounds.Top + 1f, 0f);
        Instantiate(prefab, position, Quaternion.identity);
    }

    // spawns get faster over time, with a bit of randomness so it's not too regular
    float NextDelay()
    {
        float progress = Mathf.Clamp01(Time.timeSinceLevelLoad / rampTime);
        float delay = Mathf.Lerp(startDelay, endDelay, progress);
        return delay * Random.Range(0.7f, 1.3f);
    }
}
