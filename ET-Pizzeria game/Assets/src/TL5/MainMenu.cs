using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MenuBase
{
    public string sceneName = "ToppingScreen";

    public override void HandlePrimaryAction()
    {
        SceneManager.LoadScene(sceneName);
    }
}