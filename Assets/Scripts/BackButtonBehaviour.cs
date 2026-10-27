using UnityEngine;
using UnityEngine.SceneManagement;

// Back / Main Menu button, goes to the Start scene
public class BackButtonBehaviour : MonoBehaviour
{
    public AudioClip clickSound;

    public void OnBackButtonClick()
    {
        SoundFX.Play(clickSound);
        SceneManager.LoadScene("Start");
    }
}
