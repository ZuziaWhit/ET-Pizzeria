using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CutScreenManager : MonoBehaviour
{
    private Vector2 startpos;
    private Vector2 endpos;

    [Header("References")]
    public LayerMask pizzaLayer;
    public LineRenderer previewLine;  
    public GameObject linePrefab;    

    private List<LineRenderer> cutLines = new List<LineRenderer>();

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startpos = GetMouseWorld();

            if (previewLine != null)
            {
                previewLine.positionCount = 2;
                previewLine.SetPosition(0, ToV3(startpos));
                previewLine.SetPosition(1, ToV3(startpos));
            }
        }

        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 current = GetMouseWorld();
            if (previewLine != null)
                previewLine.SetPosition(1, ToV3(current));
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            endpos = GetMouseWorld();

            if (previewLine != null)
                previewLine.positionCount = 0;

            PerformCut(startpos, endpos);
        }
    }

    Vector2 GetMouseWorld()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = 10f; 
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    Vector3 ToV3(Vector2 v)
    {
        return new Vector3(v.x, v.y, 0f);
    }

    void PerformCut(Vector2 start, Vector2 end)
    {
        RaycastHit2D hit = Physics2D.Linecast(start, end, pizzaLayer);

        if (!hit)
        {
            Debug.Log("Missed pizza");
            return;
        }

        Transform pizza = hit.collider.transform;

        Vector2 center = pizza.position;
        float pizzaRadius = 2f;
        Vector2 dir = (end - start).normalized;
        Vector2 p1 = center + (Vector2.Dot(start - center, dir)) * dir;
        Vector2 p2 = center + (Vector2.Dot(end - center, dir)) * dir;

        if ((p1 - center).magnitude > pizzaRadius) p1 = center + (p1 - center).normalized * pizzaRadius;
        if ((p2 - center).magnitude > pizzaRadius) p2 = center + (p2 - center).normalized * pizzaRadius;

        if (linePrefab == null)
        {
            Debug.LogError("Line Prefab NOT assigned!");
            return;
        }

        GameObject lineObj = Instantiate(linePrefab);
        LineRenderer lr = lineObj.GetComponent<LineRenderer>();

        if (lr == null)
        {
            Debug.LogError("LinePrefab missing LineRenderer!");
            return;
        }

        lr.positionCount = 2;
        lr.SetPosition(0, ToV3(p1));
        lr.SetPosition(1, ToV3(p2));

        lr.startWidth = 0.12f;
        lr.endWidth = 0.12f;
        lr.numCapVertices = 5;

        lr.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        lr.sortingLayerName = "pizzaLayer";
        lr.sortingOrder = 100;

        cutLines.Add(lr);

        PizzaSlice slice = pizza.GetComponent<PizzaSlice>();
        if (slice != null)
            slice.Slice(p1, p2);

        Debug.Log($"Freeform cut performed");
    }
}