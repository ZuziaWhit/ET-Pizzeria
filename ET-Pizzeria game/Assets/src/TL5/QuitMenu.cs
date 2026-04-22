using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    public void Show()
    {
        gameObject.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void ConfirmQuit()
    {
        Time.timeScale = 1f;

        Application.Quit();
    }
}