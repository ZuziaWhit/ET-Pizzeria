using UnityEngine;

public class cutscreenmanager : MonoBehaviour
{
    public Vector2 startpos;
    public Vector2 endpos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnMouseDown()
    {
        startpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // Update is called once per frame
    void OnMouseUp()
    {
        endpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        performcut(startpos, endpos);
    }

    void performcut(Vector2 start, Vector2 end)
    {
        Debug.Log("Cut from " + start + " to " + end);
    }
}
