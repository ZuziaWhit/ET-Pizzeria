using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Reflection;

public class CutScreenManagerTests
{
    private GameObject pizzaObj;
    private GameObject managerObj;
    private CutScreenManager manager;

    [SetUp]
    public void Setup()
    {
        // ---------------- PIZZA ----------------
        pizzaObj = new GameObject("Pizza");
        pizzaObj.transform.position = Vector2.zero;

        var col = pizzaObj.AddComponent<CircleCollider2D>();
        col.radius = 3f;

        pizzaObj.AddComponent<Pizza>();
        pizzaObj.layer = 10;

        // ---------------- MANAGER ----------------
        managerObj = new GameObject("Manager");
        manager = managerObj.AddComponent<CutScreenManager>();

        // ---------------- LINE PREFAB ----------------
        GameObject linePrefab = new GameObject("LinePrefab");
        var lr = linePrefab.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.material = new Material(Shader.Find("Sprites/Default"));

        typeof(CutScreenManager)
            .GetField("linePrefab", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(manager, linePrefab);

        // ---------------- LAYER MASK ----------------
        LayerMask mask = new LayerMask();
        mask.value = ~0; // hit EVERYTHING

        typeof(CutScreenManager)
            .GetField("pizzaLayer", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(manager, mask);

        // ---------------- COLOR PROVIDER ----------------
        typeof(CutScreenManager)
            .GetField("cutColorProvider", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(manager, new DefaultColor());
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(pizzaObj);
        GameObject.DestroyImmediate(managerObj);
    }

    // ---------------- BASIC ----------------

    [Test] public void Test01_ShortDragIgnored()
    {
        manager.PerformCut(Vector2.zero, new Vector2(0.1f, 0.1f));
        LogAssert.Expect(LogType.Log, "Cut too small, ignored");
    }

    [Test] public void Test02_ClickNoDragIgnored()
    {
        manager.PerformCut(Vector2.zero, Vector2.zero);
        LogAssert.Expect(LogType.Log, "Cut too small, ignored");
    }

    [Test] public void Test03_ValidHorizontalCut()
    {
        manager.PerformCut(new Vector2(-5, 0), new Vector2(5, 0));
        Assert.AreEqual(1, pizzaObj.transform.childCount);
    }

    [Test] public void Test04_ValidVerticalCut()
    {
        manager.PerformCut(new Vector2(0, -5), new Vector2(0, 5));
        Assert.AreEqual(1, pizzaObj.transform.childCount);
    }

    [Test] public void Test05_ValidDiagonalCut()
    {
        manager.PerformCut(new Vector2(-5, -5), new Vector2(5, 5));
        Assert.AreEqual(1, pizzaObj.transform.childCount);
    }

    // ---------------- RAYCAST ----------------

    [Test] public void Test06_MissPizza()
    {
        manager.PerformCut(new Vector2(10, 10), new Vector2(12, 12));
        LogAssert.Expect(LogType.Log, "Missed pizza");
    }

    [Test] public void Test07_StartOutside_EndInside()
    {
        manager.PerformCut(new Vector2(-5, 0), Vector2.zero);
        Assert.AreEqual(1, pizzaObj.transform.childCount);
    }

    [Test] public void Test08_StartInside_EndOutside()
    {
        manager.PerformCut(Vector2.zero, new Vector2(5, 0));
        Assert.AreEqual(1, pizzaObj.transform.childCount);
    }

    [Test] public void Test09_TouchEdge()
    {
        manager.PerformCut(new Vector2(-3, 3), new Vector2(3, 3));
        Assert.IsTrue(pizzaObj.transform.childCount <= 1);
    }

    [Test] public void Test10_ParallelNoHit()
    {
        manager.PerformCut(new Vector2(0, 5), new Vector2(5, 5));
        LogAssert.Expect(LogType.Log, "Missed pizza");
    }

    // ---------------- INTERSECTION ----------------

    [Test] public void Test11_ThroughCenter()
    {
        manager.PerformCut(new Vector2(-5, 0), new Vector2(5, 0));
        Assert.AreEqual(1, pizzaObj.transform.childCount);
    }

    [Test] public void Test12_SmallAngleCut()
    {
        manager.PerformCut(new Vector2(-5, 0.1f), new Vector2(5, 0.2f));
        Assert.AreEqual(1, pizzaObj.transform.childCount);
    }

    [Test] public void Test13_LongDragStillClips()
    {
        manager.PerformCut(new Vector2(-50, 0), new Vector2(50, 0));
        Assert.AreEqual(1, pizzaObj.transform.childCount);
    }

    [Test] public void Test14_NoIntersection()
    {
        manager.PerformCut(new Vector2(0, 6), new Vector2(5, 6));
        LogAssert.Expect(LogType.Log, "Missed pizza");
    }

    [Test] public void Test15_ExactEdgePass()
    {
        manager.PerformCut(new Vector2(-3, 0), new Vector2(3, 0));
        Assert.AreEqual(1, pizzaObj.transform.childCount);
    }

    // ---------------- MULTIPLE CUTS ----------------

    [Test] public void Test16_TwoCutsPersist()
    {
        manager.PerformCut(new Vector2(-5, 0), new Vector2(5, 0));
        manager.PerformCut(new Vector2(0, -5), new Vector2(0, 5));
        Assert.AreEqual(2, pizzaObj.transform.childCount);
    }

    [Test] public void Test17_ThreeCutsPersist()
    {
        manager.PerformCut(new Vector2(-5, 0), new Vector2(5, 0));
        manager.PerformCut(new Vector2(0, -5), new Vector2(0, 5));
        manager.PerformCut(new Vector2(-5, -5), new Vector2(5, 5));
        Assert.AreEqual(3, pizzaObj.transform.childCount);
    }

    // ---------------- COMPONENT SAFETY ----------------

    [Test] public void Test18_NoPizzaComponent()
    {
        Object.DestroyImmediate(pizzaObj.GetComponent<Pizza>());
        manager.PerformCut(new Vector2(-5, 0), new Vector2(5, 0));
        Assert.Pass();
    }

    [Test] public void Test19_LineRendererCreated()
    {
        manager.PerformCut(new Vector2(-5, 0), new Vector2(5, 0));
        Assert.IsNotNull(pizzaObj.GetComponentInChildren<LineRenderer>());
    }

    [Test] public void Test20_LineParentedCorrectly()
    {
        manager.PerformCut(new Vector2(-5, 0), new Vector2(5, 0));
        Assert.AreEqual(pizzaObj.transform, pizzaObj.transform.GetChild(0).parent);
    }

    // ---------------- LINE PROPERTIES ----------------

    [Test] public void Test21_LineWidthCorrect()
    {
        manager.PerformCut(new Vector2(-5, 0), new Vector2(5, 0));
        var lr = pizzaObj.GetComponentInChildren<LineRenderer>();
        Assert.AreEqual(0.06f, lr.startWidth);
    }

    [Test] public void Test22_LineUsesLocalSpace()
    {
        manager.PerformCut(new Vector2(-5, 0), new Vector2(5, 0));
        var lr = pizzaObj.GetComponentInChildren<LineRenderer>();
        Assert.IsFalse(lr.useWorldSpace);
    }

    [Test] public void Test23_LinePositionCount()
    {
        manager.PerformCut(new Vector2(-5, 0), new Vector2(5, 0));
        var lr = pizzaObj.GetComponentInChildren<LineRenderer>();
        Assert.AreEqual(2, lr.positionCount);
    }

    // ---------------- COLOR SYSTEM ----------------

    [Test] public void Test24_ColorApplied()
    {
        manager.PerformCut(new Vector2(-5, 0), new Vector2(5, 0));
        var lr = pizzaObj.GetComponentInChildren<LineRenderer>();
        Assert.AreEqual(lr.startColor, lr.endColor);
    }

    [Test] public void Test25_ColorNotDefaultRed()
    {
        manager.PerformCut(new Vector2(-5, 0), new Vector2(5, 0));
        var lr = pizzaObj.GetComponentInChildren<LineRenderer>();
        Assert.AreNotEqual(Color.red, lr.startColor);
    }

    // ---------------- EDGE CASES ----------------

    [Test] public void Test26_VerySmallAngle()
    {
        manager.PerformCut(new Vector2(-5, 0.01f), new Vector2(5, 0.02f));
        Assert.AreEqual(1, pizzaObj.transform.childCount);
    }

    [Test]
    public void Test27_NegativeCoordinates()
    {
        manager.PerformCut(new Vector2(-5, -1), new Vector2(5, -1));
        Assert.AreEqual(1, pizzaObj.transform.childCount);
    }

    [Test] public void Test28_LargeCoordinates()
    {
        manager.PerformCut(new Vector2(-100, 0), new Vector2(100, 0));
        Assert.AreEqual(1, pizzaObj.transform.childCount);
    }

    [Test] public void Test29_CutNearTopEdge()
    {
        manager.PerformCut(new Vector2(-5, 2.9f), new Vector2(5, 2.9f));
        Assert.IsTrue(pizzaObj.transform.childCount <= 1);
    }

    [Test] public void Test30_CutNearBottomEdge()
    {
        manager.PerformCut(new Vector2(-5, -2.9f), new Vector2(5, -2.9f));
        Assert.IsTrue(pizzaObj.transform.childCount <= 1);
    }
}