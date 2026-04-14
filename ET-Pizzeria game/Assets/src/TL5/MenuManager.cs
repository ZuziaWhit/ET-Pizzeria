using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public MenuBase currentMenu;

    public void ExecutePrimaryAction()
    {
        currentMenu.HandlePrimaryAction();
    }
}