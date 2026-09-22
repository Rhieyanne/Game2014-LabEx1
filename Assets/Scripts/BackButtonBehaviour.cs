using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonBehaviour : MonoBehaviour
{

    public void OnBackButtonClick()
    {
        Debug.Log("Back Button Clicked!");
        SceneManager.LoadScene("Start");

    }
}
