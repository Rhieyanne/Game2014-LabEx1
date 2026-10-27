using UnityEngine;
using TMPro;

// shows the final score and best score on the game over screen
public class GameOverScreen : MonoBehaviour
{
    public TMP_Text finalScoreText;
    public TMP_Text bestScoreText;

    void Start()
    {
        finalScoreText.text = "Final Score: " + ScoreManager.score;
        bestScoreText.text = "Best: " + PlayerPrefs.GetInt("HighScore", 0);
    }
}
