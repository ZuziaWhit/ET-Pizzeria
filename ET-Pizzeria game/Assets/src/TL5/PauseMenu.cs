using UnityEngine;

public class PauseMenu : MenuBase
{
    public override void HandlePrimaryAction()
    {
        Debug.Log("Resume Game");
    }
}