using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private MenuBase currentMenu;

    void Awake()
    {
        currentMenu = new PauseMenu(); // dynamic binding setup
    }

    public void SetPauseMenu()
    {
        currentMenu = new PauseMenu();
    }

    public void SetMainMenu()
    {
        currentMenu = new MainMenu();
    }

    public void ExecutePrimaryAction()
    {
        currentMenu.HandlePrimaryAction(); // dynamic binding happens here
    }
}