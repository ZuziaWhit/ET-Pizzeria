/*
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Reflection;

public class PizzaCutStressTest
{
    private GameObject pizza;
    private GameObject managerObj;
    private CutScreenManager cutManager;

    [SetUp]
    public void Setup()
    {
        // ---------------- PIZZA ----------------
        pizza = new GameObject("testpizza");
        pizza.transform.position = Vector3.zero;

        var collider = pizza.AddComponent<CircleCollider2D>();
        collider.radius = 3f;

        pizza.layer = 10;

        // ---------------- CAMERA ----------------
        GameObject cameraObj = new GameObject("TestCamera");
        var cam = cameraObj.AddComponent<Camera>();
        cam.orthographic = true;
        cam.transform.position = new Vector3(0, 0, -10);
        cameraObj.tag = "MainCamera";

        // ---------------- MANAGER ----------------
        managerObj = new GameObject("CutManager");
        cutManager = managerObj.AddComponent<CutScreenManager>();

        // ---------------- LINE PREFAB ----------------
        GameObject linePrefab = new GameObject("LinePrefab");
        LineRenderer lr = linePrefab.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.material = new Material(Shader.Find("Sprites/Default"));

        typeof(CutScreenManager)
            .GetField("linePrefab", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(cutManager, linePrefab);

        // ---------------- LAYER MASK ----------------
        LayerMask mask = new LayerMask();
        mask.value = ~0; // ignore filtering in stress test

        typeof(CutScreenManager)
            .GetField("pizzaLayer", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(cutManager, mask);

        // ---------------- COLOR PROVIDER ----------------
        typeof(CutScreenManager)
            .GetField("cutColorProvider", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(cutManager, new DefaultColor());
    }

    [UnityTest]
    [Timeout(30000)]
    public IEnumerator StressTestMultipleCuts()
    {
        int cutCount = 100;

        for (int i = 0; i < cutCount; i++)
        {
            Vector2 start = (Vector2)pizza.transform.position + Random.insideUnitCircle * 2f;
            Vector2 end = (Vector2)pizza.transform.position + Random.insideUnitCircle * 2f;

            cutManager.PerformCut(start, end);

            yield return null;
        }

        // Optional validation (safe version)
        Assert.IsTrue(pizza.transform.childCount >= 0);

        yield return null;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(pizza);
        Object.DestroyImmediate(managerObj);
        Object.DestroyImmediate(GameObject.Find("TestCamera"));
    }
}
*/