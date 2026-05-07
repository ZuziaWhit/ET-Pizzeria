/*
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ScoreBoundaryTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void Score_Clamps_Values_Outside_Valid_Range()
    {
        // Create a GameObject so we can attach the controller
        GameObject go = new GameObject();
        ToppingController controller = go.AddComponent<ToppingController>();

        // ---- Just outside the bounds ----
        Assert.AreEqual(100, controller.SetScore(105));
        Assert.AreEqual(0, controller.SetScore(-1));
        Assert.AreEqual(0, controller.SetScore(-5));
        Assert.AreEqual(100, controller.SetScore(110));

        // ---- Extremely outside the bounds ----
        Assert.AreEqual(0, controller.SetScore(-100));
        Assert.AreEqual(100, controller.SetScore(300));
        Assert.AreEqual(0, controller.SetScore(-11111));
        Assert.AreEqual(100, controller.SetScore(11111));
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator ScoreBoundaryTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}



*/