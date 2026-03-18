using UnityEngine;
using UnityEngine.InputSystem;

public class CutScreenManager : MonoBehaviour
{
    private Vector2 startpos;
    private Vector2 endpos;

    public LayerMask pizzaLayer;
    public LineRenderer lineRenderer;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startpos = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            if(lineRenderer)
            {
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, startpos);
                lineRenderer.SetPosition(1, startpos);
            }
        }

        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 currentpos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            if(lineRenderer)
            {
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(1, currentpos);
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            endpos = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            if(lineRenderer)
            {
                lineRenderer.positionCount = 0;
            }

            PerformCut(startpos, endpos);
        }
    }

    void PerformCut(Vector2 start, Vector2 end)
    {
        Debug.Log("Cut from " + start + " to " + end);

        UnityEngine.RaycastHit2D[] hits = Physics2D.LinecastAll(start, end, pizzaLayer);
        foreach (var hit in hits)
        {
            Debug.Log("Hit pizza: " + hit.collider.name);

            PizzaSlice pizza = hit.collider.GetComponent<PizzaSlice>();
            if (pizza != null)
            {
                pizza.Slice(start, end);
            }
        }
    }
}