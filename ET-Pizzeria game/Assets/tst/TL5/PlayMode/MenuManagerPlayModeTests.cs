using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class MenuManagerPlayModeTests
{
    // Boundary Test 1: Valid scene loads
    [UnityTest]
    public IEnumerator StartGame_LoadsToppingScreen()
    {
        GameObject obj = new GameObject();
        MenuManager manager = obj.AddComponent<MenuManager>();

        manager.StartGame();

        yield return null;

        Assert.AreEqual("ToppingScreen", SceneManager.GetActiveScene().name);
    }

    // Boundary Test 2: Invalid scene name logs error
    [UnityTest]
    public IEnumerator StartGame_InvalidSceneName_LogsError()
    {
        GameObject obj = new GameObject();
        MenuManager manager = obj.AddComponent<MenuManager>();
        manager.sceneName = "FakeScene";

        LogAssert.Expect(LogType.Error, new System.Text.RegularExpressions.Regex(".*FakeScene.*"));

        manager.StartGame();

        yield return null;
    }

    // Stress Test: Repeated calls
    [UnityTest]
    public IEnumerator StartGame_CalledRepeatedly_LoadsToppingScreen()
    {
        GameObject obj = new GameObject();
        MenuManager manager = obj.AddComponent<MenuManager>();

        for (int i = 0; i < 100; i++)
        {
            manager.StartGame();
        }

        yield return null;

        Assert.AreEqual("ToppingScreen", SceneManager.GetActiveScene().name);
    }
}