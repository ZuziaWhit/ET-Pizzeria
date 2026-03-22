using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public void GoToMainMenu()
    {
        Debug.Log("Main Menu button clicked");
        SceneManager.LoadScene("MainMenu");
    }
}