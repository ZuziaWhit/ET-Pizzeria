// ─────────────────────────────────────────────────────────────────────────────
// ScoreBoundaryTests.cs
// Place this file inside your EditMode test assembly folder.
// (Window > General > Test Runner > Create EditMode Test Assembly Folder)
//
// These tests check that each station score and the total score behave correctly
// at every important edge value: below minimum, minimum, just above minimum,
// just below maximum, maximum, and above maximum.
// ─────────────────────────────────────────────────────────────────────────────

using NUnit.Framework;

public class ScoreBoundaryTests
{
    // ─── Individual station score clamping ───────────────────────────────────
    // Each station score must stay within [0, 100].
    // Input values below 0 should clamp to 0; values above 100 should clamp to 100.

    [TestCase(-1,   0,   TestName = "Score_BelowMinimum_ClampsToZero")]
    [TestCase(0,    0,   TestName = "Score_AtMinimum_ReturnsZero")]
    [TestCase(1,    1,   TestName = "Score_JustAboveMinimum_ReturnsOne")]
    [TestCase(50,   50,  TestName = "Score_Midpoint_ReturnsFifty")]
    [TestCase(99,   99,  TestName = "Score_JustBelowMaximum_ReturnsNinetyNine")]
    [TestCase(100,  100, TestName = "Score_AtMaximum_ReturnsOneHundred")]
    [TestCase(101,  100, TestName = "Score_AboveMaximum_ClampsToOneHundred")]
    [TestCase(-999, 0,   TestName = "Score_ExtremelyNegative_ClampsToZero")]
    [TestCase(999,  100, TestName = "Score_ExtremelyPositive_ClampsToOneHundred")]
    public void ClampScore_ReturnsExpectedValue_AtBoundaries(int input, int expected)
    {
        int result = ScoreManager.ClampScore(input);
        Assert.That(result, Is.EqualTo(expected),
            $"ClampScore({input}) should return {expected} but returned {result}.");
    }


    // ─── Total score calculation boundaries ──────────────────────────────────
    // Total score = Toppings + Baking + Cutting, each clamped to [0, 100].
    // Valid total range is therefore [0, 300].

    [TestCase(0,   0,   0,   0,   TestName = "Total_AllZero_ReturnsZero")]
    [TestCase(100, 100, 100, 300, TestName = "Total_AllMaximum_Returns300")]
    [TestCase(1,   1,   1,   3,   TestName = "Total_AllOne_ReturnsThree")]
    [TestCase(99,  99,  99,  297, TestName = "Total_AllNinetyNine_Returns297")]
    [TestCase(50,  50,  50,  150, TestName = "Total_AllMidpoint_Returns150")]
    public void CalculateTotal_ReturnsExpectedSum_AtBoundaries(
        int toppings, int baking, int cutting, int expectedTotal)
    {
        int result = ScoreManager.CalculateTotal(toppings, baking, cutting);
        Assert.That(result, Is.EqualTo(expectedTotal),
            $"CalculateTotal({toppings}, {baking}, {cutting}) should be {expectedTotal} but was {result}.");
    }


    // ─── Over-limit inputs fed into total ────────────────────────────────────
    // Even if bad data arrives (e.g. a station somehow returns 150),
    // each component should still be clamped before summing.

    [TestCase(150, 150, 150, 300, TestName = "Total_AllOverMax_ClampsEachThenSums")]
    [TestCase(-50, -50, -50, 0,   TestName = "Total_AllUnderMin_ClampsEachThenSums")]
    [TestCase(150, 0,   0,   100, TestName = "Total_OneOverMax_OthersZero_Clamps")]
    [TestCase(-1,  100, 100, 200, TestName = "Total_OneUnderMin_OthersMax_Clamps")]
    public void CalculateTotal_ClampsComponents_BeforeSumming(
        int toppings, int baking, int cutting, int expectedTotal)
    {
        int result = ScoreManager.CalculateTotal(toppings, baking, cutting);
        Assert.That(result, Is.EqualTo(expectedTotal),
            $"CalculateTotal({toppings}, {baking}, {cutting}) should clamp to {expectedTotal} but was {result}.");
    }


    // ─── Asymmetric realistic score combinations ──────────────────────────────
    // Simulates real gameplay where each station gets a different accuracy value.

    [TestCase(85, 90, 75, 250, TestName = "Total_RealisticScores_SumsCorrectly")]
    [TestCase(100, 0, 50, 150, TestName = "Total_PerfectToppings_ZeroBaking_HalfCutting")]
    [TestCase(0, 100, 0,  100, TestName = "Total_OnlyBakingScores")]
    [TestCase(33, 33, 34, 100, TestName = "Total_EvenSplit_ReturnsOneHundred")]
    public void CalculateTotal_AsymmetricInputs_SumsCorrectly(
        int toppings, int baking, int cutting, int expectedTotal)
    {
        int result = ScoreManager.CalculateTotal(toppings, baking, cutting);
        Assert.That(result, Is.EqualTo(expectedTotal),
            $"CalculateTotal({toppings}, {baking}, {cutting}) should be {expectedTotal} but was {result}.");
    }
}
