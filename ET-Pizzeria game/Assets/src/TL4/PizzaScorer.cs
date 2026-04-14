// PizzaScorer.cs
// ─────────────────────────────────────────────────────────────────────────────
// Reads from PizzaGameData (primitives only) and returns per-category scores.
// This assembly references NOTHING outside of UnityEngine — no circular deps.
//
// Scoring rules
// ─────────────
// TOPPINGS  (0–100)
//   • Each ordered topping present on the pizza  → equal share of 100 pts
//   • Each extra topping NOT in the order        → -10 pts penalty
//   • Clamped to [0, 100]
//
// BAKE TIME (0–100)
//   • Accuracy = max(0, 1 - |actual - ordered| / ordered) × 100
//
// CUTS      (0 or 100)
//   • Exact match → 100,  any deviation → 0
//
// STARS     (1–5)
//   • Average of three scores:  ≥90=5★  ≥75=4★  ≥60=3★  ≥40=2★  else 1★
// ─────────────────────────────────────────────────────────────────────────────

using System.Collections.Generic;
using UnityEngine;

public static class PizzaScorer
{
    // ── Public result container ───────────────────────────────────────────────

    public class ScoreResult
    {
        public float ToppingScore;
        public float BakeScore;
        public float CutScore;

        public float AverageScore => (ToppingScore + BakeScore + CutScore) / 3f;

        public int Stars
        {
            get
            {
                float avg = AverageScore;
                if (avg >= 90f) return 5;
                if (avg >= 75f) return 4;
                if (avg >= 60f) return 3;
                if (avg >= 40f) return 2;
                return 1;
            }
        }

        // Diagnostic fields
        public List<string> MissingToppings      = new List<string>();
        public List<string> ExtraToppings        = new List<string>();
        public float        OrderedBakeSeconds;
        public float        ActualBakeSeconds;
        public int          OrderedCuts;
        public int          ActualCuts;
    }

    // ── Main entry point ──────────────────────────────────────────────────────

    /// <summary>Reads PizzaGameData and returns a ScoreResult. Returns null if data isn't ready.</summary>
    public static ScoreResult ScoreFromGameData()
    {
        if (!PizzaGameData.IsReady)
        {
            Debug.LogError("[PizzaScorer] PizzaGameData is not ready. " +
                           "Ensure SetOrder() and SetPizza() were called before the scene loaded.");
            return null;
        }

        ScoreResult result = new ScoreResult();

        ScoreToppings(result);
        ScoreBakeTime(result);
        ScoreCuts(result);

        Debug.Log($"[PizzaScorer] Toppings: {result.ToppingScore:F1} | " +
                  $"Bake: {result.BakeScore:F1} | " +
                  $"Cuts: {result.CutScore:F1} | " +
                  $"Avg: {result.AverageScore:F1} | " +
                  $"Stars: {result.Stars}");

        return result;
    }

    // ── Private scoring helpers ───────────────────────────────────────────────

    private static void ScoreToppings(ScoreResult result)
    {
        List<string> orderedList = PizzaGameData.OrderedIngredients;
        List<string> actualList  = PizzaGameData.ActualIngredients;

        HashSet<string> orderedSet = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase);
        HashSet<string> actualSet  = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase);

        foreach (string t in orderedList) orderedSet.Add(t);
        foreach (string t in actualList)  actualSet.Add(t);

        foreach (string t in orderedSet)
            if (!actualSet.Contains(t))
                result.MissingToppings.Add(t);

        foreach (string t in actualSet)
            if (!orderedSet.Contains(t))
                result.ExtraToppings.Add(t);

        int   totalOrdered = orderedList.Count;
        int   correctCount = totalOrdered - result.MissingToppings.Count;
        float baseScore    = totalOrdered > 0 ? (correctCount / (float)totalOrdered) * 100f : 100f;
        float penalty      = result.ExtraToppings.Count * 10f;

        result.ToppingScore = Mathf.Clamp(baseScore - penalty, 0f, 100f);
    }

    private static void ScoreBakeTime(ScoreResult result)
    {
        string raw = PizzaGameData.OrderedBakeTime.Replace("s", "").Trim();

        if (!float.TryParse(raw, out float orderedSeconds))
        {
            Debug.LogWarning($"[PizzaScorer] Could not parse bake time '{PizzaGameData.OrderedBakeTime}'. Bake score = 0.");
            result.BakeScore          = 0f;
            result.OrderedBakeSeconds = 0f;
            result.ActualBakeSeconds  = PizzaGameData.ActualBakeTime;
            return;
        }

        float actualSeconds           = PizzaGameData.ActualBakeTime;
        result.OrderedBakeSeconds     = orderedSeconds;
        result.ActualBakeSeconds      = actualSeconds;

        if (orderedSeconds <= 0f)
        {
            result.BakeScore = actualSeconds == 0f ? 100f : 0f;
            return;
        }

        float diff       = Mathf.Abs(actualSeconds - orderedSeconds);
        float accuracy   = Mathf.Clamp01(1f - (diff / orderedSeconds));
        result.BakeScore = accuracy * 100f;
    }

    private static void ScoreCuts(ScoreResult result)
    {
        if (!int.TryParse(PizzaGameData.OrderedCutType, out int orderedCuts))
        {
            Debug.LogWarning($"[PizzaScorer] Could not parse cut type '{PizzaGameData.OrderedCutType}'. Cut score = 0.");
            result.CutScore    = 0f;
            result.OrderedCuts = 0;
            result.ActualCuts  = PizzaGameData.ActualCuts;
            return;
        }

        result.OrderedCuts = orderedCuts;
        result.ActualCuts  = PizzaGameData.ActualCuts;
        result.CutScore    = (PizzaGameData.ActualCuts == orderedCuts) ? 100f : 0f;
    }
}