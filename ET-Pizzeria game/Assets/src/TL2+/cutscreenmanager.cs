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

    public void PerformCut(Vector2 start, Vector2 end)
    {
        RaycastHit2D hit = Physics2D.Linecast(start, end, pizzaLayer);

        if (Vector2.Distance(start, end) < 0.1f)
        {
            Debug.Log("Cut too small, ignored");
            return;
        }

        //if (end > (8,6) || start > (8,6))
        //{
            //Debug.Log("Cut outside the cutting board, ignored");
            //return;
        //}

        if (!hit)
        {
            Debug.Log("Missed pizza");
            return;
        }

        Transform pizza = hit.collider.transform;

        Vector2 center = pizza.position;
        float pizzaRadius = 2f;
        
        Vector2 dir = (end - start).normalized;

        Vector2 f = start - center;

        float a = Vector2.Dot(dir, dir);
        float b = 2 * Vector2.Dot(f, dir);
        float c = Vector2.Dot(f, f) - pizzaRadius * pizzaRadius;

        float discriminant = b * b - 4 * a * c;

        if (discriminant < 0)
        {
            Debug.Log("No intersection with pizza");
            return;
        }

        discriminant = Mathf.Sqrt(discriminant);

        float t1 = (-b - discriminant) / (2 * a);
        float t2 = (-b + discriminant) / (2 * a);

        Vector2 p1 = start + dir * t1;
        Vector2 p2 = start + dir * t2;

        if (linePrefab == null)
        {
            Debug.LogError("Line Prefab NOT assigned!");
            return;
        }

        GameObject lineObj = Instantiate(linePrefab, this.transform);
        LineRenderer lr = lineObj.GetComponent<LineRenderer>();

        lr.positionCount = 2;
        lr.SetPosition(0, ToV3(p1));
        lr.SetPosition(1, ToV3(p2));

        lr.startWidth = 0.06f;
        lr.endWidth = 0.06f;
        lr.numCapVertices = 5;

        lr.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));

        Color cutColor;

        if (ColorUtility.TryParseHtmlString("#FCFAE3", out cutColor))
        {
            lr.material.color = cutColor;
        }

        lr.sortingLayerName = "pizzaLayer";
        lr.sortingOrder = 100;

        cutLines.Add(lr);

        if (lr == null)
        {
            Debug.LogError("LinePrefab missing LineRenderer!");
            return;
        }

        PizzaSlice slice = pizza.GetComponent<PizzaSlice>();
        if (slice != null)
            slice.Slice(p1, p2);

        Debug.Log($"Freeform cut performed");
    }
}