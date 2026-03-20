using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PizzaCutStressTest
{
    private GameObject pizza;
    private CutScreenManager cutManager;
    private GameObject linePrefab;

    [SetUp]
    public void Setup()
    {
        // create a fake pizza
        pizza = new GameObject("testpizza");
        pizza.transform.position = Vector3.zero;
        pizza.AddComponent<SpriteRenderer>();
        var collider = pizza.AddComponent<CircleCollider2D>();
        collider.radius = 2f;

        int pizzaLayer = LayerMask.NameToLayer("pizzaLayer");
        if (pizzaLayer == -1)
        {
            Debug.LogWarning("No 'Pizza' layer found. Defaulting to layer 0.");
            pizzaLayer = 0;
        }
        pizza.layer = pizzaLayer;

        // create a dummy camera
        GameObject cameraObj = new GameObject("TestCamera");
        var cam = cameraObj.AddComponent<Camera>();
        cam.orthographic = true;
        cam.transform.position = new Vector3(0, 0, -10); 
        cameraObj.tag = "MainCamera"; 

        // create a dummy LineRenderer prefab
        linePrefab = new GameObject("LinePrefab");
        var lr = linePrefab.AddComponent<LineRenderer>();
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));

        // create CutScreenManager
        GameObject managerObj = new GameObject("CutManager");
        cutManager = managerObj.AddComponent<CutScreenManager>();
        cutManager.linePrefab = linePrefab;
        cutManager.pizzaLayer = 1 << pizzaLayer; 
    }

    [UnityTest]
    public IEnumerator StressTestMultipleCuts()
    {
        int cutCount = 500; 

        for (int i = 0; i < cutCount; i++)
        {
            Vector2 start = (Vector2)pizza.transform.position + Random.insideUnitCircle * 2f;
            Vector2 end = (Vector2)pizza.transform.position + Random.insideUnitCircle * 2f;

            cutManager.PerformCut(start, end);

            yield return null;
        }

        Assert.IsTrue(cutManager.transform.childCount >= cutCount, "Cuts spawned correctly and hit pizza.");

        yield return null;
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