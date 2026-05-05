using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    private MenuBase logic = new PauseMenu();

    public GameObject settingsMenu;

    public void OnResumeClicked()
    {
        logic.HandlePrimaryAction();
        PauseController.Instance.ResumeGame();
    }

    public void OnSettingsClicked()
    {
        gameObject.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}