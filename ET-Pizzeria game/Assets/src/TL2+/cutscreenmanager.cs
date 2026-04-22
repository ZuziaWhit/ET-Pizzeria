using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CutScreenManager : MonoBehaviour
{
    private Vector2 startpos;
    private Vector2 endpos;

    private float pizzaRadius = 3f;
    private PizzaCutColor cutColorProvider;

    [Header("References")]
    [SerializeField] private LayerMask pizzaLayer;
    [SerializeField] private LineRenderer previewLine;
    [SerializeField] private GameObject linePrefab;

    private List<LineRenderer> cutLines = new List<LineRenderer>();

    void Awake()
    {
        cutColorProvider = new DefaultColor();
    }
    

    void Update()
    {

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startpos = GetMouseWorld();

            if (previewLine != null)
            {
                previewLine.enabled = true;
                previewLine.positionCount = 2;
                previewLine.SetPosition(0, ToV3(startpos));
                previewLine.SetPosition(1, ToV3(startpos));
            }
        }

        if (Mouse.current.leftButton.isPressed)
        {
            if (previewLine != null && previewLine.positionCount == 2)
            {
                Vector2 current = GetMouseWorld();
                previewLine.SetPosition(1, ToV3(current));
            }
            // Vector2 current = GetMouseWorld();
            // if (previewLine != null)
            //     previewLine.SetPosition(1, ToV3(current));
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            endpos = GetMouseWorld();

            if (previewLine != null)
                previewLine.positionCount = 0;

            PerformCut(startpos, endpos); 
        }
    }

    private Vector2 GetMouseWorld()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = 10f;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private Vector3 ToV3(Vector2 v)
    {
        return new Vector3(v.x, v.y, 0f);
    }

    public void PerformCut(Vector2 start, Vector2 end)
    {
        Debug.Log("Base cut");

        DoCut(start, end);
    }

    private void DoCut(Vector2 start, Vector2 end)
    {
        RaycastHit2D hit = Physics2D.Linecast(start, end, pizzaLayer);

        if (Vector2.Distance(start, end) < 0.5f)
        {
            Debug.Log("Cut too small, ignored");
            return;
        }

        if (!hit)
        {
            Debug.Log("Missed pizza");
            return;
        }

        Pizza pizzaData = hit.collider.GetComponent<Pizza>();
        Transform pizza = hit.collider.transform;

        Vector2 center = pizza.position;
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

        CreateCutLine(p1, p2, pizza);

        if (pizzaData != null)
            pizzaData.AddCut();
    }

    private void CreateCutLine(Vector2 p1, Vector2 p2, Transform parent)
    {
        GameObject lineObj = Instantiate(linePrefab, parent);
        LineRenderer lr = lineObj.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));

        lr.useWorldSpace = false; 

        Vector3 localP1 = parent.InverseTransformPoint(ToV3(p1));
        Vector3 localP2 = parent.InverseTransformPoint(ToV3(p2));

        lr.positionCount = 2;
        lr.SetPosition(0, localP1);
        lr.SetPosition(1, localP2);

        lr.startWidth = 0.06f;
        lr.endWidth = 0.06f;
        
        Color cutColor = cutColorProvider.GetCutColor();
        lr.startColor = cutColor;
        lr.endColor = cutColor;
        lr.sortingLayerName = "pizzaLayer";
        lr.sortingOrder = 100;

        cutLines.Add(lr);
    }
}
