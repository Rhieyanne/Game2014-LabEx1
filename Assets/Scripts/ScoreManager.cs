using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text livesText;
    public TMP_Text scoreText;
    public float padding = 20f;

    public static int lives;
    public static int score;

    private float livesTextHalfWidth;
    private float livesTextHalfHeight;
    private float scoreTextHalfWidth;
    private float scoreTextHalfHeight;

    void Start()
    {
        lives = 3;
        score = 0;

        livesTextHalfWidth = livesText.rectTransform.rect.width * 0.5f;
        livesTextHalfHeight = livesText.rectTransform.rect.height * 0.5f;

        scoreTextHalfWidth = scoreText.rectTransform.rect.width * 0.5f;
        scoreTextHalfHeight = scoreText.rectTransform.rect.height * 0.5f;
    }

    void Update()
    {
        float scale = livesText.canvas.scaleFactor;

        livesText.rectTransform.position = new Vector2(Screen.safeArea.xMin + livesTextHalfWidth * scale + padding,
            Screen.safeArea.yMax - livesTextHalfHeight * scale);

        scoreText.rectTransform.position = new Vector2(Screen.safeArea.xMax - scoreTextHalfWidth * scale - padding,
            Screen.safeArea.yMax - scoreTextHalfHeight * scale);

        livesText.text = "Lives: " + lives;
        scoreText.text = "Score: " + score;

        if (lives <= 0)
        {
            SceneManager.LoadScene("Restart");
        }
    }
}
