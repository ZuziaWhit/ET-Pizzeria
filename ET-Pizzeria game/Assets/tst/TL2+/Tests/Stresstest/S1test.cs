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
        // Create a fake pizza
        pizza = new GameObject("TestPizza");
        pizza.transform.position = Vector3.zero;
        pizza.AddComponent<SpriteRenderer>(); // optional sprite
        var collider = pizza.AddComponent<CircleCollider2D>();
        collider.radius = 2f;

        // Assign pizza layer (make sure this matches CutScreenManager.pizzaLayer)
        int pizzaLayer = LayerMask.NameToLayer("pizzaLayer");
        if (pizzaLayer == -1)
        {
            // Create a temporary layer for testing
            Debug.LogWarning("No 'Pizza' layer found. Defaulting to layer 0.");
            pizzaLayer = 0;
        }
        pizza.layer = pizzaLayer;

        // Create a dummy LineRenderer prefab
        linePrefab = new GameObject("LinePrefab");
        var lr = linePrefab.AddComponent<LineRenderer>();
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));

        // Create CutScreenManager
        GameObject managerObj = new GameObject("CutManager");
        cutManager = managerObj.AddComponent<CutScreenManager>();
        cutManager.linePrefab = linePrefab;
        cutManager.pizzaLayer = 1 << pizzaLayer; // set layer mask correctly
    }

    [UnityTest]
    public IEnumerator StressTestMultipleCuts()
    {
        int cutCount = 50; // number of cuts to simulate

        for (int i = 0; i < cutCount; i++)
        {
            // Generate random start/end points **inside pizza radius**
            Vector2 start = (Vector2)pizza.transform.position + Random.insideUnitCircle * 2f;
            Vector2 end = (Vector2)pizza.transform.position + Random.insideUnitCircle * 2f;

            // Call PerformCut — now it will hit the pizza
            cutManager.PerformCut(start, end);

            yield return null; // simulate frame
        }

        // Check that cuts were spawned
        Assert.IsTrue(cutManager.transform.childCount >= cutCount, "Cuts spawned correctly and hit pizza.");

        yield return null;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(pizza);
        Object.DestroyImmediate(linePrefab);
        Object.DestroyImmediate(cutManager.gameObject);
    }
}