using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Score and lives on screen, and game over when you run out of lives.
public class ScoreManager : MonoBehaviour
{
    public TMP_Text livesText;
    public TMP_Text scoreText;
    public float padding = 20f;

    public int startingLives = 3;
    public float gameOverDelay = 1.5f;  // wait a bit so you can see the explosion

    // static so other scripts can change them, and the Restart scene can still read the score
    public static int lives;
    public static int score;

    private float livesTextHalfWidth;
    private float livesTextHalfHeight;
    private float scoreTextHalfWidth;
    private float scoreTextHalfHeight;
    private bool gameOver;

    void Start()
    {
        lives = startingLives;
        score = 0;

        livesTextHalfWidth = livesText.rectTransform.rect.width * 0.5f;
        livesTextHalfHeight = livesText.rectTransform.rect.height * 0.5f;

        scoreTextHalfWidth = scoreText.rectTransform.rect.width * 0.5f;
        scoreTextHalfHeight = scoreText.rectTransform.rect.height * 0.5f;
    }

    void Update()
    {
        // keep the text inside the safe area so the notch doesn't cover it
        float scale = livesText.canvas.scaleFactor;

        livesText.rectTransform.position = new Vector2(Screen.safeArea.xMin + livesTextHalfWidth * scale + padding,
            Screen.safeArea.yMax - livesTextHalfHeight * scale);

        scoreText.rectTransform.position = new Vector2(Screen.safeArea.xMax - scoreTextHalfWidth * scale - padding,
            Screen.safeArea.yMax - scoreTextHalfHeight * scale);

        livesText.text = "Lives: " + lives;
        scoreText.text = "Score: " + score;

        // out of lives -> game over after a short delay (only once)
        if (lives <= 0 && !gameOver)
        {
            gameOver = true;
            Invoke(nameof(GameOver), gameOverDelay);
        }
    }

    // ---- point management ----

    // enemies call this when a torpedo kills them, pickups when you grab them
    public static void AddPoints(int amount)
    {
        if (amount > 0)
            score += amount;
    }

    // ---- life management ----

    // the player calls this when it gets hit (bullet, bite, mine, or crashing into an enemy)
    public static void LoseLife(int amount = 1)
    {
        lives = Mathf.Max(lives - amount, 0);
    }

    // oxygen tanks give one back
    public static void AddLife(int amount = 1)
    {
        if (amount > 0)
            lives += amount;
    }

    void GameOver()
    {
        // save the high score
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }

        SceneManager.LoadScene("Restart");
    }
}
