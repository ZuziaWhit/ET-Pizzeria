using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private MenuBase logic = new MainMenu();

    public string sceneName;

    public void OnStartClicked()
    {
        logic.HandlePrimaryAction(); // dynamic binding
        SceneManager.LoadScene(sceneName);
    }

    public void OnSettingsClicked(GameObject settingsMenu)
    {
        gameObject.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}