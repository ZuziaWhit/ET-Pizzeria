using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject mainMenuUI;

    private bool openedFromPause = false;

    private VolumeSettings volumeSettings = new VolumeSettings(1f); // default volume

    public void Show()
    {
        gameObject.SetActive(true);

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
        // store it using Private Class Data
        volumeSettings = new VolumeSettings(value);

        // apply it to Unity
        AudioListener.volume = volumeSettings.GetVolume();
    }
}