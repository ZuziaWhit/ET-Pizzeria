using UnityEngine;

public class MenuBase
{
    public virtual void HandlePrimaryAction() // remove virtual
    {
        Debug.Log("Default menu action");
    }
}