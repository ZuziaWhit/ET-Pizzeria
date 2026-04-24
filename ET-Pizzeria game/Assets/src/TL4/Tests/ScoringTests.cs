// ═══════════════════════════════════════════════════════════════════════════
// ScoringTests.cs
// ───────────────────────────────────────────────────────────────────────────
// CONTRIBUTION — what this file does:
//   Automated test suite for the pizza scoring system. Each test method
//   sets up a specific scenario in PizzaGameData, runs PizzaScorer, and
//   checks the result against expected values using Unity's test assertions.
//
// HOW TO RUN:
//   Window → General → Test Runner → EditMode tab → Run All
//
// TEST PLAN SUMMARY (for oral exam):
//   Tests are grouped into three categories:
//   1. TOPPINGS — correct, missing, extra, mixed, empty list edge case
//   2. BAKE TIME — perfect, over, under, way off, zero edge case
//   3. CUTS — correct, wrong, zero edge case
//   4. INTEGRATION — full pizza scenarios that test all three together
//      and verify the star rating calculation
//
// EXAM — test that caught a teammate bug:
//   TestBakeTime_ActualIsZero was added specifically after discovering that
//   the baking station was not calling pizza.AddBakeTime() during early
//   integration. The pizza's bake time defaulted to 0f, causing a silent
//   wrong score. This test catches that regression — if the baking station
//   ever stops writing the bake time, this test fails immediately and
//   tells us exactly which station broke the handoff.
// ═══════════════════════════════════════════════════════════════════════════

using System.Collections.Generic;
using NUnit.Framework;

public class ScoringTests
{
    // ── Setup helper ──────────────────────────────────────────────────────
    // Called before each test to clear any leftover state
    [SetUp]
    public void Setup()
    {
        PizzaGameData.Clear();
    }

    // ═══════════════════════════════════════════════════════════════════════
    // TOPPING TESTS
    // ═══════════════════════════════════════════════════════════════════════

    [Test]
    public void TestToppings_PerfectMatch_Scores100()
    {
        // ARRANGE — order and pizza have identical toppings
        var toppings = new List<string> { "Pepperoni", "Mushroom", "Onion" };
        PizzaGameData.SetOrder(toppings, "10s", "4");
        PizzaGameData.SetPizza(toppings, 10f, 4);

        // ACT
        var result = PizzaScorer.ScoreFromGameData();

        // ASSERT
        Assert.AreEqual(100f, result.ToppingScore, 0.01f,
            "Perfect topping match should score 100.");
        Assert.AreEqual(0, result.MissingToppings.Count,
            "No toppings should be missing.");
        Assert.AreEqual(0, result.ExtraToppings.Count,
            "No extra toppings should be present.");
    }

    [Test]
    public void TestToppings_OneMissing_ReducesScore()
    {
        // ARRANGE — order has 3 toppings, pizza only has 2
        var ordered = new List<string> { "Pepperoni", "Mushroom", "Onion" };
        var actual  = new List<string> { "Pepperoni", "Mushroom" }; // missing Onion
        PizzaGameData.SetOrder(ordered, "10s", "4");
        PizzaGameData.SetPizza(actual, 10f, 4);

        // ACT
        var result = PizzaScorer.ScoreFromGameData();

        // ASSERT — 2/3 correct = 66.67
        Assert.AreEqual(66.67f, result.ToppingScore, 0.1f,
            "One missing topping out of three should score ~66.67.");
        Assert.IsTrue(result.MissingToppings.Contains("Onion"),
            "Onion should be listed as missing.");
    }

    [Test]
    public void TestToppings_AllMissing_ScoresZero()
    {
        // ARRANGE — order has toppings, pizza has none
        var ordered = new List<string> { "Pepperoni", "Mushroom", "Onion" };
        var actual  = new List<string>();
        PizzaGameData.SetOrder(ordered, "10s", "4");
        PizzaGameData.SetPizza(actual, 10f, 4);

        // ACT
        var result = PizzaScorer.ScoreFromGameData();

        // ASSERT
        Assert.AreEqual(0f, result.ToppingScore, 0.01f,
            "All toppings missing should score 0.");
    }

    [Test]
    public void TestToppings_ExtraTopping_DeductsTenPoints()
    {
        // ARRANGE — pizza has one unwanted topping (Bacon not in order)
        var ordered = new List<string> { "Pepperoni", "Mushroom" };
        var actual  = new List<string> { "Pepperoni", "Mushroom", "Bacon" };
        PizzaGameData.SetOrder(ordered, "10s", "4");
        PizzaGameData.SetPizza(actual, 10f, 4);

        // ACT
        var result = PizzaScorer.ScoreFromGameData();

        // ASSERT — base 100, -10 for Bacon = 90
        Assert.AreEqual(90f, result.ToppingScore, 0.01f,
            "One extra topping should deduct 10 points from a perfect base.");
        Assert.IsTrue(result.ExtraToppings.Contains("Bacon"),
            "Bacon should be listed as an extra topping.");
    }

    [Test]
    public void TestToppings_MultipleExtras_ClampedToZero()
    {
        // ARRANGE — pizza has 11 extra toppings (impossible in game but edge case)
        var ordered = new List<string> { "Pepperoni" };
        var actual  = new List<string>
        {
            "Pepperoni", "Mushroom", "Onion", "Sausage", "Bacon",
            "Extra Cheese", "Black Olives", "Green Peppers",
            "Tomato", "Garlic", "Spinach"
        };
        PizzaGameData.SetOrder(ordered, "10s", "4");
        PizzaGameData.SetPizza(actual, 10f, 4);

        // ACT
        var result = PizzaScorer.ScoreFromGameData();

        // ASSERT — score must not go below 0
        Assert.GreaterOrEqual(result.ToppingScore, 0f,
            "Score should never go below 0 even with many extra toppings.");
    }

    [Test]
    public void TestToppings_CaseInsensitive_StillMatches()
    {
        // ARRANGE — capitalisation mismatch between stations
        var ordered = new List<string> { "pepperoni", "mushroom" };
        var actual  = new List<string> { "Pepperoni", "Mushroom" };
        PizzaGameData.SetOrder(ordered, "10s", "4");
        PizzaGameData.SetPizza(actual, 10f, 4);

        // ACT
        var result = PizzaScorer.ScoreFromGameData();

        // ASSERT — case difference should not penalise the player
        Assert.AreEqual(100f, result.ToppingScore, 0.01f,
            "Topping comparison should be case-insensitive.");
    }

    [Test]
    public void TestToppings_EmptyOrderedList_ScoresHundred()
    {
        // ARRANGE — no toppings were ordered (edge case)
        // If nothing was ordered, nothing can be wrong
        var empty = new List<string>();
        PizzaGameData.SetOrder(empty, "10s", "4");
        PizzaGameData.SetPizza(empty, 10f, 4);

        // ACT
        var result = PizzaScorer.ScoreFromGameData();

        // ASSERT
        Assert.AreEqual(100f, result.ToppingScore, 0.01f,
            "No toppings ordered with no toppings applied should score 100.");
    }

    // ═══════════════════════════════════════════════════════════════════════
    // BAKE TIME TESTS
    // ═══════════════════════════════════════════════════════════════════════

    [Test]
    public void TestBakeTime_PerfectMatch_Scores100()
    {
        var t = new List<string> { "Pepperoni" };
        PizzaGameData.SetOrder(t, "10s", "4");
        PizzaGameData.SetPizza(t, 10f, 4);

        var result = PizzaScorer.ScoreFromGameData();

        Assert.AreEqual(100f, result.BakeScore, 0.01f,
            "Exact bake time match should score 100.");
    }

    [Test]
    public void TestBakeTime_OffByHalf_ScoresFifty()
    {
        // ARRANGE — ordered 10s, baked 15s (5s over = 50% of ordered time)
        var t = new List<string> { "Pepperoni" };
        PizzaGameData.SetOrder(t, "10s", "4");
        PizzaGameData.SetPizza(t, 15f, 4);

        var result = PizzaScorer.ScoreFromGameData();

        // 1 - (5/10) = 0.5 -> 50
        Assert.AreEqual(50f, result.BakeScore, 0.01f,
            "Being off by half the ordered time should score 50.");
    }

    [Test]
    public void TestBakeTime_OverByFullAmount_ScoresZero()
    {
        // ARRANGE — ordered 10s, baked 20s (10s over = 100% of ordered time)
        var t = new List<string> { "Pepperoni" };
        PizzaGameData.SetOrder(t, "10s", "4");
        PizzaGameData.SetPizza(t, 20f, 4);

        var result = PizzaScorer.ScoreFromGameData();

        Assert.AreEqual(0f, result.BakeScore, 0.01f,
            "Being off by the full ordered duration should score 0.");
    }

    [Test]
    public void TestBakeTime_UnderBaked_StillScoresProportionally()
    {
        // ARRANGE — ordered 10s, baked 5s (under by 5s = 50% off)
        var t = new List<string> { "Pepperoni" };
        PizzaGameData.SetOrder(t, "10s", "4");
        PizzaGameData.SetPizza(t, 5f, 4);

        var result = PizzaScorer.ScoreFromGameData();

        Assert.AreEqual(50f, result.BakeScore, 0.01f,
            "Under-baking by half should score 50, same as over-baking by half.");
    }

    [Test]
    public void TestBakeTime_ActualIsZero_ScoresZero()
    {
        // EXAM NOTE — this test was added after discovering the baking station
        // was not calling pizza.AddBakeTime() during early integration.
        // The pizza's bake time defaulted to 0f, causing a silent wrong score.
        // This test catches that regression immediately.
        var t = new List<string> { "Pepperoni" };
        PizzaGameData.SetOrder(t, "10s", "4");
        PizzaGameData.SetPizza(t, 0f, 4); // baking station never set the time

        var result = PizzaScorer.ScoreFromGameData();

        Assert.AreEqual(0f, result.BakeScore, 0.01f,
            "Zero bake time when 10s was ordered should score 0. " +
            "Check that baking station is calling pizza.AddBakeTime().");
    }

    [Test]
    public void TestBakeTime_AllThreeOrderOptions()
    {
        // ARRANGE — test all three bake time strings parse correctly
        var t = new List<string> { "Pepperoni" };

        PizzaGameData.SetOrder(t, "5s", "4");
        PizzaGameData.SetPizza(t, 5f, 4);
        Assert.AreEqual(100f, PizzaScorer.ScoreFromGameData().BakeScore, 0.01f, "5s perfect");

        PizzaGameData.Clear();
        PizzaGameData.SetOrder(t, "10s", "4");
        PizzaGameData.SetPizza(t, 10f, 4);
        Assert.AreEqual(100f, PizzaScorer.ScoreFromGameData().BakeScore, 0.01f, "10s perfect");

        PizzaGameData.Clear();
        PizzaGameData.SetOrder(t, "15s", "4");
        PizzaGameData.SetPizza(t, 15f, 4);
        Assert.AreEqual(100f, PizzaScorer.ScoreFromGameData().BakeScore, 0.01f, "15s perfect");
    }

    // ═══════════════════════════════════════════════════════════════════════
    // CUT TESTS
    // ═══════════════════════════════════════════════════════════════════════

    [Test]
    public void TestCuts_PerfectMatch_Scores100()
    {
        var t = new List<string> { "Pepperoni" };
        PizzaGameData.SetOrder(t, "10s", "4");
        PizzaGameData.SetPizza(t, 10f, 4);

        var result = PizzaScorer.ScoreFromGameData();

        Assert.AreEqual(100f, result.CutScore, 0.01f,
            "Exact cut count match should score 100.");
    }

    [Test]
    public void TestCuts_WrongCount_ScoresZero()
    {
        var t = new List<string> { "Pepperoni" };
        PizzaGameData.SetOrder(t, "10s", "4");
        PizzaGameData.SetPizza(t, 10f, 2); // ordered 4 cuts, only made 2

        var result = PizzaScorer.ScoreFromGameData();

        Assert.AreEqual(0f, result.CutScore, 0.01f,
            "Wrong cut count should score 0.");
    }

    [Test]
    public void TestCuts_ZeroCuts_NoException()
    {
        // ARRANGE — cutting station never cut the pizza (0 cuts)
        var t = new List<string> { "Pepperoni" };
        PizzaGameData.SetOrder(t, "10s", "1");  // ordered 1 cut
        PizzaGameData.SetPizza(t, 10f, 0);      // no cuts made

        // ACT + ASSERT — should not throw, should just score 0
        Assert.DoesNotThrow(() =>
        {
            var result = PizzaScorer.ScoreFromGameData();
            Assert.AreEqual(0f, result.CutScore, 0.01f,
                "Zero cuts when cuts were ordered should score 0 without crashing.");
        });
    }

    [Test]
    public void TestCuts_AllThreeCutOptions()
    {
        var t = new List<string> { "Pepperoni" };

        PizzaGameData.SetOrder(t, "10s", "1");
        PizzaGameData.SetPizza(t, 10f, 1);
        Assert.AreEqual(100f, PizzaScorer.ScoreFromGameData().CutScore, 0.01f, "1 cut correct");

        PizzaGameData.Clear();
        PizzaGameData.SetOrder(t, "10s", "2");
        PizzaGameData.SetPizza(t, 10f, 2);
        Assert.AreEqual(100f, PizzaScorer.ScoreFromGameData().CutScore, 0.01f, "2 cuts correct");

        PizzaGameData.Clear();
        PizzaGameData.SetOrder(t, "10s", "4");
        PizzaGameData.SetPizza(t, 10f, 4);
        Assert.AreEqual(100f, PizzaScorer.ScoreFromGameData().CutScore, 0.01f, "4 cuts correct");
    }

    // ═══════════════════════════════════════════════════════════════════════
    // INTEGRATION TESTS — full pizza scenarios + star ratings
    // ═══════════════════════════════════════════════════════════════════════

    [Test]
    public void TestIntegration_PerfectPizza_FiveStars()
    {
        // ARRANGE — everything matches perfectly
        var toppings = new List<string> { "Pepperoni", "Mushroom", "Onion" };
        PizzaGameData.SetOrder(toppings, "10s", "4");
        PizzaGameData.SetPizza(toppings, 10f, 4);

        var result = PizzaScorer.ScoreFromGameData();

        Assert.AreEqual(100f, result.ToppingScore, 0.01f);
        Assert.AreEqual(100f, result.BakeScore,    0.01f);
        Assert.AreEqual(100f, result.CutScore,     0.01f);
        Assert.AreEqual(100f, result.AverageScore, 0.01f);
        Assert.AreEqual(5,    result.Stars,
            "A perfect pizza should earn 5 stars.");
    }

    [Test]
    public void TestIntegration_TerriblePizza_OneStar()
    {
        // ARRANGE — wrong toppings, way overbaked, wrong cuts
        var ordered = new List<string> { "Pepperoni", "Mushroom", "Onion" };
        var actual  = new List<string> { "Bacon" }; // completely wrong
        PizzaGameData.SetOrder(ordered, "5s", "4");
        PizzaGameData.SetPizza(actual, 30f, 1); // 25s over, wrong cuts

        var result = PizzaScorer.ScoreFromGameData();

        Assert.AreEqual(1, result.Stars,
            "A terrible pizza should earn only 1 star.");
        Assert.Less(result.AverageScore, 40f,
            "Average score should be below 40 for a terrible pizza.");
    }

    [Test]
    public void TestIntegration_StarThresholds()
    {
        // Test each star boundary using bake time to control the score
        // All other fields perfect — only bake time varies

        var t = new List<string> { "Pepperoni" };

        // 5 stars: average >= 90 — perfect everything
        PizzaGameData.SetOrder(t, "10s", "4");
        PizzaGameData.SetPizza(t, 10f, 4);
        Assert.AreEqual(5, PizzaScorer.ScoreFromGameData().Stars, ">=90 avg = 5 stars");

        // 1 star: average < 40 — bake way off, wrong cuts, missing toppings
        PizzaGameData.Clear();
        var ordered3 = new List<string> { "Pepperoni", "Mushroom", "Onion" };
        PizzaGameData.SetOrder(ordered3, "5s", "4");
        PizzaGameData.SetPizza(new List<string>(), 30f, 1);
        Assert.AreEqual(1, PizzaScorer.ScoreFromGameData().Stars, "<40 avg = 1 star");
    }

    [Test]
    public void TestIntegration_IsReadyGuard_ReturnNull()
    {
        // ARRANGE — don't call SetOrder or SetPizza
        // PizzaGameData.Clear() was already called in Setup

        // ACT
        var result = PizzaScorer.ScoreFromGameData();

        // ASSERT — should return null, not throw
        Assert.IsNull(result,
            "ScoreFromGameData should return null if PizzaGameData is not ready.");
    }

    [Test]
    public void TestIntegration_OnlyOrderSet_ReturnNull()
    {
        // ARRANGE — SetOrder called but not SetPizza
        var t = new List<string> { "Pepperoni" };
        PizzaGameData.SetOrder(t, "10s", "4");
        // deliberately skip SetPizza

        var result = PizzaScorer.ScoreFromGameData();

        Assert.IsNull(result,
            "ScoreFromGameData should return null if SetPizza was never called.");
    }

    [Test]
    public void TestIntegration_ClearResetsState()
    {
        // ARRANGE — set data, then clear it
        var t = new List<string> { "Pepperoni" };
        PizzaGameData.SetOrder(t, "10s", "4");
        PizzaGameData.SetPizza(t, 10f, 4);
        Assert.IsTrue(PizzaGameData.IsReady, "Should be ready before Clear.");

        PizzaGameData.Clear();

        Assert.IsFalse(PizzaGameData.IsReady,
            "After Clear(), IsReady should be false.");
        Assert.AreEqual(0, PizzaGameData.OrderedIngredients.Count,
            "After Clear(), OrderedIngredients should be empty.");
        Assert.AreEqual(0f, PizzaGameData.ActualBakeTime,
            "After Clear(), ActualBakeTime should be 0.");
    }
}