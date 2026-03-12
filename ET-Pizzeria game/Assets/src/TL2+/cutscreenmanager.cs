using UnityEngine;
using UnityEngine.InputSystem;

public class CutScreenManager : MonoBehaviour
{
    private Vector2 startpos;
    private Vector2 endpos;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startpos = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            endpos = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            PerformCut(startpos, endpos);
        }
    }

    void PerformCut(Vector2 start, Vector2 end)
    {
        Debug.Log("Cut from " + start + " to " + end);
    }
}