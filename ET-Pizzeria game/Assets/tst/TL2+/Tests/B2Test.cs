using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PizzaCutLengthBoundaryTest
{
    private GameObject pizza;
    private CutScreenManager cutManager;
    private GameObject linePrefab;
    private int pizzaLayer;

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
        cutManager.linePrefab = linePrefab;
        cutManager.pizzaLayer = 1 << pizzaLayer;
    }

    [UnityTest]
    public IEnumerator CutLengthBoundaryTest()
    {
        Vector2 center = pizza.transform.position;
        float radius = 2f;

        // small cut (should NOT work)
        Debug.Log("small");
        Vector2 smallStart = center;
        Vector2 smallEnd = center + new Vector2(0.1f, 0.1f);

        cutManager.PerformCut(smallStart, smallEnd);
        yield return null;

        int afterSmall = cutManager.transform.childCount;
        Assert.IsTrue(afterSmall == 0, "Small cuts should NOT create a line");

        // normal cut (should work)
        Debug.Log("normal");
        Vector2 normalStart = center + Vector2.left * radius;
        Vector2 normalEnd = center + Vector2.right * radius;

        cutManager.PerformCut(normalStart, normalEnd);
        yield return null;

        int afterNormal = cutManager.transform.childCount;
        Assert.IsTrue(afterNormal > afterSmall, "Normal cut should create a line");

        // large cut (should still work)
        Debug.Log("large");
        Vector2 largeStart = center + Vector2.left * 10f;
        Vector2 largeEnd = center + Vector2.right * 10f;

        cutManager.PerformCut(largeStart, largeEnd);
        yield return null;

        int afterLarge = cutManager.transform.childCount;
        Assert.IsTrue(afterLarge > afterNormal, "Large cuts crossing pizza should work");
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