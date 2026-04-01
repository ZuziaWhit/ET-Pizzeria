using UnityEngine;

public class PauseMenu : MenuBase
{
    public override void HandlePrimaryAction()
    {
        Time.timeScale = 1f; // resumes game
        gameObject.SetActive(false); // hides pause menu UI
    }
}