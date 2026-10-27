using UnityEngine;
using UnityEngine.SceneManagement;

// Play / Play Again button
public class StartButtonBehaviour : MonoBehaviour
{
    public AudioClip clickSound;

    public void OnStartButtonClick()
    {
        SoundFX.Play(clickSound);
        SceneManager.LoadScene("Play");
    }
}
