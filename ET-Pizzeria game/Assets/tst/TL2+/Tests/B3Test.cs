/*
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PizzaCutSmallBoundaryTest
{
    private GameObject pizza;
    private static CutScreenManager cutManager;
    //private static GameObject linePrefab = cutManager.getlinePrefab();
    //private int pizzaLayer = cutManager.getpizzaLayer();

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


        *//*
        // Line prefab
        linePrefab = new GameObject("LinePrefab");
        var lr = linePrefab.AddComponent<LineRenderer>();
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        *//*

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
        //float radius = 2f;

        // small cut (should NOT work)
        Debug.Log("T1: smallest");
        Vector2 smallStartt1 = center;
        Vector2 smallEndt1 = center + new Vector2(0.01f, 0.01f);

        cutManager.PerformCut(smallStartt1, smallEndt1);
        yield return null;

        int afterSmallt1 = pizza.transform.childCount;
        Assert.IsTrue(afterSmallt1 == 0, "T1: Small cuts should NOT create a line");

        // small cut (should NOT work)
        Debug.Log("T2: smaller");
        Vector2 smallStartt2 = center;
        Vector2 smallEndt2 = center + new Vector2(0.1f, 0.1f);

        cutManager.PerformCut(smallStartt2, smallEndt2);
        yield return null;

        int afterSmallt2 = pizza.transform.childCount;
        Assert.IsTrue(afterSmallt2 == 0, "T2: Small cuts should NOT create a line");

        // small cut (should NOT work)
        Debug.Log("T3: small");
        Vector2 smallStartt3 = center;
        Vector2 smallEndt3 = center + new Vector2(0.35f, 0.35f);

        cutManager.PerformCut(smallStartt3, smallEndt3);
        yield return null;

        int afterSmallt3 = pizza.transform.childCount;
        Assert.IsTrue(afterSmallt3 == 0, "T3: Small cuts should NOT create a line");

        // smallest cut that should work (should work)
        Debug.Log("T4: small");
        Vector2 smallStartt4 = center;
        Vector2 smallEndt4 = center + new Vector2(0.36f, 0.36f);

        cutManager.PerformCut(smallStartt4, smallEndt4);
        yield return null;

        int afterSmallt4 = pizza.transform.childCount;
        Assert.IsTrue(afterSmallt4 > 0, "T4 Small cuts should create a line");

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
*/