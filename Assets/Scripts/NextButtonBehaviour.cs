using UnityEngine;
using UnityEngine.SceneManagement;

// skips straight to the game over screen (handy for the demo video)
public class NextButtonBehaviour : MonoBehaviour
{
    public AudioClip clickSound;

    public void OnNextButtonClick()
    {
        SoundFX.Play(clickSound);
        SceneManager.LoadScene("Restart");
    }
}
