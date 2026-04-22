using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject mainMenuUI;
    private bool openedFromPause = false;

    public void Show()
    {
        gameObject.SetActive(true);

        // if pause menu is active, we came from pause
        if (pauseMenu != null && pauseMenu.activeSelf)
        {
            openedFromPause = true;
            pauseMenu.SetActive(false);
        }
        else
        {
            openedFromPause = false;
        }

        if (mainMenuUI != null && mainMenuUI.activeSelf)
        {
            mainMenuUI.SetActive(false);
        }
    }

    public void Back()
    {
        gameObject.SetActive(false);

        if (openedFromPause)
        {
            if (pauseMenu != null)
                pauseMenu.SetActive(true);
        }
        else
        {
            if (mainMenuUI != null)
                mainMenuUI.SetActive(true);
        }
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
    }
}