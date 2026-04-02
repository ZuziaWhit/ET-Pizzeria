using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PizzaCutNormalBoundaryTest
{
    private GameObject pizza;
    private static CutScreenManager cutManager;
    private static GameObject linePrefab = cutManager.getlinePrefab();
    private int pizzaLayer = cutManager.getpizzaLayer();

    [SetUp]
    public void Setup()
    {
        // Camera
        GameObject camObj = new GameObject("TestCamera");
        var cam = camObj.AddComponent<Camera>();
        cam.orthographic = true;
        cam.transform.position = new Vector3(0, 0, -10);
        camObj.tag = "MainCamera";

        // Pizza
        pizza = new GameObject("Pizza");
        pizza.transform.position = Vector3.zero;
        var collider = pizza.AddComponent<CircleCollider2D>();
        collider.radius = 2f;

        pizzaLayer = LayerMask.NameToLayer("Pizza");
        if (pizzaLayer == -1) pizzaLayer = 0;
        pizza.layer = pizzaLayer;

        // Line prefab
        /*
        linePrefab = new GameObject("LinePrefab");
        var lr = linePrefab.AddComponent<LineRenderer>();
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        */

        // Manager
        GameObject managerObj = new GameObject("CutManager");
        cutManager = managerObj.AddComponent<CutScreenManager>();
        //cutManager.linePrefab = linePrefab;
        //cutManager.pizzaLayer = 1 << pizzaLayer;
    }

    [UnityTest]
    public IEnumerator CutLengthBoundaryTest()
    {
        Vector2 center = pizza.transform.position;
        float radius = 2f;

        // smallest normal cut (should work)
        Debug.Log("T1: normal");
        Vector2 smallStart = center;
        Vector2 smallEnd = center + new Vector2(0.36f, 0.36f);

        cutManager.PerformCut(smallStart, smallEnd);
        yield return null;

        int afterSmall = pizza.transform.childCount;
        Assert.IsTrue(afterSmall > 0, "T1: Normal cuts should create a line");

        // normal cut (should work)
        Debug.Log("normal");
        Vector2 normalStart = center + Vector2.left * radius;
        Vector2 normalEnd = center + Vector2.right * radius;

        cutManager.PerformCut(normalStart, normalEnd);
        yield return null;

        int afterNormal = pizza.transform.childCount;
        Assert.IsTrue(afterNormal > 1, "T2: Normal cut should create a line");
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(pizza);
        //Object.DestroyImmediate(linePrefab);
        Object.DestroyImmediate(cutManager.gameObject);
        Object.DestroyImmediate(GameObject.Find("TestCamera"));
    }
}
