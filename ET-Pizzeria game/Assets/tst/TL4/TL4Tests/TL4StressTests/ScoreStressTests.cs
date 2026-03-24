// ─────────────────────────────────────────────────────────────────────────────
// ScoreStressTests.cs
// Place this file inside your PlayMode test assembly folder.
// (Window > General > Test Runner > Create PlayMode Test Assembly Folder)
//
// These tests push the scoring system hard to make sure it does not break,
// produce incorrect results, or leak state when subjected to heavy or repeated use.
// ─────────────────────────────────────────────────────────────────────────────

using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ScoreStressTests
{
    // ─── Stress Test 1: Rapid repeated score setting ─────────────────────────
    // Simulates many score updates in a row (e.g. if scores are updated every frame
    // or after every pizza). Verifies the final stored value is always the last one set.

    [UnityTest]
    public IEnumerator SetScore_RepeatedRapidUpdates_RetainsLastValue()
    {
        // Create a temporary ScoreManager in the scene for this test
        GameObject go = new GameObject("TestScoreManager");
        ScoreManager manager = go.AddComponent<ScoreManager>();

        const int iterations = 10000;

        for (int i = 0; i < iterations; i++)
        {
            // Alternate between valid and boundary values to stress the setter
            int value = i % 101; // cycles 0-100 repeatedly
            manager.SetToppingsScore(value);
            manager.SetBakingScore(value);
            manager.SetCuttingScore(value);
        }

        yield return null; // wait one frame to let Unity settle

        // After the loop, i % 101 at i=9999 is 9999 % 101 = 99
        // Each score should be clamped: ClampScore(99) = 99
        int expectedFinal = ScoreManager.ClampScore((iterations - 1) % 101);
        int expectedTotal = expectedFinal * 3;

        Assert.That(manager.ToppingsScore, Is.EqualTo(expectedFinal),
            $"ToppingsScore should be {expectedFinal} after rapid updates.");
        Assert.That(manager.BakingScore, Is.EqualTo(expectedFinal),
            $"BakingScore should be {expectedFinal} after rapid updates.");
        Assert.That(manager.CuttingScore, Is.EqualTo(expectedFinal),
            $"CuttingScore should be {expectedFinal} after rapid updates.");
        Assert.That(manager.TotalScore, Is.EqualTo(expectedTotal),
            $"TotalScore should be {expectedTotal} after rapid updates.");

        Object.DestroyImmediate(go);
    }


    // ─── Stress Test 2: Extreme out-of-range values ───────────────────────────
    // Floods the score setter with very large positive and very large negative numbers.
    // The clamp must hold at 0 and 100 regardless of input magnitude.

    [UnityTest]
    public IEnumerator SetScore_ExtremeValues_AlwaysClamps()
    {
        GameObject go = new GameObject("TestScoreManager");
        ScoreManager manager = go.AddComponent<ScoreManager>();

        int[] extremeValues = { int.MinValue, -100000, -1, 0, 1, 99, 100, 101, 100000, int.MaxValue };

        foreach (int val in extremeValues)
        {
            manager.SetToppingsScore(val);
            manager.SetBakingScore(val);
            manager.SetCuttingScore(val);

            Assert.That(manager.ToppingsScore, Is.InRange(0, 100),
                $"ToppingsScore out of range [0,100] after setting {val}.");
            Assert.That(manager.BakingScore, Is.InRange(0, 100),
                $"BakingScore out of range [0,100] after setting {val}.");
            Assert.That(manager.CuttingScore, Is.InRange(0, 100),
                $"CuttingScore out of range [0,100] after setting {val}.");
            Assert.That(manager.TotalScore, Is.InRange(0, 300),
                $"TotalScore out of range [0,300] after setting {val} to all stations.");
        }

        yield return null;
        Object.DestroyImmediate(go);
    }


    // ─── Stress Test 3: Reset correctness across many days ───────────────────
    // Simulates playing through many "days" in the pizzeria.
    // After each reset, all scores must return to zero so no state bleeds
    // into the next day's results screen.

    [UnityTest]
    public IEnumerator ResetScores_AfterManyDays_AlwaysReturnsToZero()
    {
        GameObject go = new GameObject("TestScoreManager");
        ScoreManager manager = go.AddComponent<ScoreManager>();

        const int simulatedDays = 500;

        for (int day = 0; day < simulatedDays; day++)
        {
            // Simulate a day — set some scores
            manager.SetToppingsScore(Random.Range(0, 101));
            manager.SetBakingScore(Random.Range(0, 101));
            manager.SetCuttingScore(Random.Range(0, 101));

            // End of day — reset for the next day
            manager.ResetScores();

            // All scores must be exactly zero after reset
            Assert.That(manager.ToppingsScore, Is.EqualTo(0),
                $"ToppingsScore was not 0 after reset on day {day + 1}.");
            Assert.That(manager.BakingScore, Is.EqualTo(0),
                $"BakingScore was not 0 after reset on day {day + 1}.");
            Assert.That(manager.CuttingScore, Is.EqualTo(0),
                $"CuttingScore was not 0 after reset on day {day + 1}.");
            Assert.That(manager.TotalScore, Is.EqualTo(0),
                $"TotalScore was not 0 after reset on day {day + 1}.");
        }

        yield return null;
        Object.DestroyImmediate(go);
    }


    // ─── Stress Test 4: CalculateTotal with many random valid inputs ──────────
    // Calls CalculateTotal 5000 times with random valid inputs and checks that
    // the result always equals the manual sum — no rounding, overflow, or drift.

    [UnityTest]
    public IEnumerator CalculateTotal_ManyRandomValidInputs_AlwaysMatchesManualSum()
    {
        const int iterations = 5000;
        bool anyFailed = false;
        string failureDetail = "";

        for (int i = 0; i < iterations; i++)
        {
            int t = Random.Range(0, 101);
            int b = Random.Range(0, 101);
            int c = Random.Range(0, 101);

            int expected = t + b + c; // all inputs are already in range, no clamping needed
            int result   = ScoreManager.CalculateTotal(t, b, c);

            if (result != expected)
            {
                anyFailed = true;
                failureDetail = $"Iteration {i}: CalculateTotal({t},{b},{c}) returned {result}, expected {expected}.";
                break;
            }
        }

        yield return null;

        Assert.That(anyFailed, Is.False,
            $"CalculateTotal produced a wrong result during stress: {failureDetail}");
    }
}
