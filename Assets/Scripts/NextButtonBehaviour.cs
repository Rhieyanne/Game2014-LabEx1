using UnityEngine;
using UnityEngine.SceneManagement;

public class NextButtonBehaviour : MonoBehaviour
{
    public void OnNextButtonClick()
    {
        Debug.Log("Next Button Clicked!");
        SceneManager.LoadScene("Restart");
    }
}
