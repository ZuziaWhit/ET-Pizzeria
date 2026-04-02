using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PizzaCutBoundaryTest
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
        linePrefab = new GameObject("LinePrefab");
        var lr = linePrefab.AddComponent<LineRenderer>();
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));

        // Manager
        GameObject managerObj = new GameObject("CutManager");
        cutManager = managerObj.AddComponent<CutScreenManager>();
       //cutManager.linePrefab = linePrefab;
        //cutManager.pizzaLayer = 1 << pizzaLayer;
    }

    [UnityTest]
    public IEnumerator BoundaryCutTest()
    {
        Vector2 center = pizza.transform.position;
        float radius = 2f;

        // center cut (should work)
        cutManager.PerformCut(center + Vector2.left * radius, center + Vector2.right * radius);
        yield return null;

        int afterCenter = pizza.transform.childCount;
        Assert.IsTrue(afterCenter > 0, "Center cut should create a line");

        // edge cut (should work)
        cutManager.PerformCut(center + Vector2.up * radius, center + Vector2.right * radius);
        yield return null;

        int afterEdge = pizza.transform.childCount;
        Assert.IsTrue(afterEdge > afterCenter, "Edge cut should create a line");

        // otside cut (should not work)
        cutManager.PerformCut(center + Vector2.up * (radius + 2f), center + Vector2.right * (radius + 2f));
        yield return null;

        int afterOutside = pizza.transform.childCount;
        Assert.AreEqual(afterEdge, afterOutside, "Outside cut should NOT create a line");
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(pizza);
        Object.DestroyImmediate(linePrefab);
        Object.DestroyImmediate(cutManager.gameObject);
        Object.DestroyImmediate(GameObject.Find("TestCamera"));
    }
}