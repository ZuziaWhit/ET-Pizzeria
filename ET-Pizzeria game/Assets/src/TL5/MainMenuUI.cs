using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private MenuBase logic = new MainMenu();

    public string sceneName;

    public void OnStartClicked()
    {
        logic.HandlePrimaryAction(); // dynamic binding
        SceneManager.LoadScene("GUI");//Sam add
        SceneManager.LoadScene(sceneName);
    }

    public void OnSettingsClicked(GameObject settingsMenu)
    {
        gameObject.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OnQuitClicked()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }
}