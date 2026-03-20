using UnityEngine;

public class TestLine : MonoBehaviour
{
    void Start()
    {
        GameObject lineObj = new GameObject("TestLine");

        LineRenderer lr = lineObj.AddComponent<LineRenderer>();

        lr.sortingLayerName = "pizzaLayer";

        lr.positionCount = 2;
        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;

        lr.material = new Material(Shader.Find("Sprites/Default"));

        lr.sortingOrder = 100;

        lr.SetPosition(0, new Vector3(-4, 0, 0));
        lr.SetPosition(1, new Vector3(4, 0, 0));

        Debug.Log("Test line created");
    }
}