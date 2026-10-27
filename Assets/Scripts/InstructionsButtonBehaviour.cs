using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsButtonBehaviour : MonoBehaviour
{
    public AudioClip clickSound;

    public void OnInstructionsButtonClick()
    {
        SoundFX.Play(clickSound);
        SceneManager.LoadScene("Instructions");
    }
}
