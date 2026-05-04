using UnityEngine;

public class PauseMenu : MenuBase
{
    public override void HandlePrimaryAction() // remove override
    {
        Debug.Log("Resume Game");
    }
}