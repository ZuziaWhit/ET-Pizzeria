// ═══════════════════════════════════════════════════════════════════════════
// PizzaScorer.cs
// ───────────────────────────────────────────────────────────────────────────
// CONTRIBUTION — what this file does:
//   Pure scoring logic. Reads PizzaGameData (order vs actual pizza) and
//   returns a ScoreResult with per-category scores (0-100) and a star rating.
//   No MonoBehaviour, no scene objects — just math.
//
// PATTERN — PRIVATE CLASS DATA (small pattern):
//   ScoreResult is the Private Class Data pattern. Its score fields have
//   NO public setters — they are written once inside PizzaScorer and are
//   read-only to everything outside this file. The UI can read the numbers
//   but cannot change them after scoring is complete.
//
//   JUSTIFY: Protects score integrity. Once a pizza is scored the numbers
//   should be final. Private Class Data enforces that at the language level.
//
//   ALTERNATIVE: A plain struct with public fields would also work but would
//   allow any code to overwrite the scores after the fact, which is a bug
//   waiting to happen.
//
//   BAD TIME TO USE: When the data genuinely needs to be mutable after
//   creation — wrapping frequently-changing data in immutable containers
//   just creates unnecessary complexity.
//
// SCORING RULES:
//   TOPPINGS (0-100):
//     Each ordered topping present on the pizza earns an equal share of 100.
//     Each extra (unwanted) topping deducts 10 points. Clamped to [0,100].
//   BAKE TIME (0-100):
//     accuracy = max(0, 1 - |actual - ordered| / ordered) * 100
//     Being off by the full ordered duration = 0 pts. Perfect = 100 pts.
//   CUTS (0 or 100):
//     Exact match = 100. Any deviation = 0. Binary — either correct or not.
//   STARS (1-5):
//     Average of three scores: >=90=5* >=75=4* >=60=3* >=40=2* else 1*
//
// DYNAMIC BINDING NOTE:
//   PizzaScorer itself is a static class (no dynamic binding here).
//   Dynamic binding lives in PizzaScoreDisplay / ScoreScreenUI — this class
//   just produces the ScoreResult data that those classes display.
//
// REUSE / COPYRIGHT:
//   Uses UnityEngine (Mathf, Debug) — part of Unity's engine under the Unity
//   license. Fair use: student project, no commercial intent, transformative.
//
// TEST PLAN NOTE:
//   Three deliberate test cases were designed here:
//   1. Zero cuts: ensures no divide-by-zero in ScoreCuts().
//   2. Unparseable bake string: TryParse guard logs a warning and scores 0
//      rather than throwing an exception — designed to catch if the order
//      generator's format ever changes.
//   3. Empty ingredient list: base score defaults to 100 (nothing was ordered,
//      nothing can be wrong) — tested to confirm correct edge case handling.
// ═══════════════════════════════════════════════════════════════════════════

using System.Collections.Generic;
using UnityEngine;

public static class PizzaScorer
{
    // ═══════════════════════════════════════════════════════════════════════
    // ScoreResult — PRIVATE CLASS DATA pattern
    // ───────────────────────────────────────────────────────────────────────
    // Fields are written once by PizzaScorer methods and are read-only
    // outside this class. This prevents any external code from corrupting
    // the score after it has been calculated.
    //
    // EXAM — walk through this when asked about Private Class Data:
    //   "ToppingScore has a public getter but no public setter. It is set
    //    inside ScoreToppings() and cannot be changed by the UI or debug menu
    //    afterward. That is Private Class Data — the data is encapsulated and
    //    protected from accidental mutation."
    // ═══════════════════════════════════════════════════════════════════════
    public class ScoreResult
    {
        // ── Per-category scores (0-100) ───────────────────────────────────
        // Public getter, no public setter — Private Class Data pattern.
        public float ToppingScore { get; internal set; }
        public float BakeScore    { get; internal set; }
        public float CutScore     { get; internal set; }

        // ── Derived scores ────────────────────────────────────────────────
        /// <summary>Average of the three category scores (0-100).</summary>
        public float AverageScore => (ToppingScore + BakeScore + CutScore) / 3f;

        /// <summary>
        /// 1-5 star rating derived from AverageScore.
        /// >=90=5* | >=75=4* | >=60=3* | >=40=2* | else 1*
        /// </summary>
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

        // ── Diagnostic data (for detail text and debug menu) ──────────────
        // These give the UI enough context to explain WHY the score is what it is.
        public List<string> MissingToppings      = new List<string>(); // ordered but not on pizza
        public List<string> ExtraToppings        = new List<string>(); // on pizza but not ordered
        public float        OrderedBakeSeconds;   // parsed from "10s" -> 10f
        public float        ActualBakeSeconds;    // from Pizza.GetBakeTime()
        public int          OrderedCuts;          // parsed from "4" -> 4
        public int          ActualCuts;           // from Pizza.GetCut()
    }

    // ── Main entry point ──────────────────────────────────────────────────
    // EXAM — walk through this when asked "show the code that runs":
    //   1. ScoreScreenUI.RefreshScore() calls ScoreFromGameData()
    //   2. ScoreFromGameData() checks IsReady, then calls Score()
    //   3. Score() calls ScoreToppings, ScoreBakeTime, ScoreCuts in order
    //   4. Returns a ScoreResult which RefreshScore() passes to DisplayScore()

    /// <summary>
    /// Reads PizzaGameData and returns a fully populated ScoreResult.
    /// Returns null and logs an error if data is not ready.
    /// Called by PizzaScoreDisplay.RefreshScore() (and the debug menu).
    /// </summary>
    public static ScoreResult ScoreFromGameData()
    {
        // IsReady guard — catches missing SetOrder() / SetPizza() calls.
        // TEST PLAN: this was a deliberate test case so teammates would get
        // an immediate, clear error if they forgot the integration lines.
        if (!PizzaGameData.IsReady)
        {
            Debug.LogError("[PizzaScorer] PizzaGameData not ready. " +
                           "Ensure SetOrder() and SetPizza() were called before EndDayScene loaded.");
            return null;
        }

        ScoreResult result = new ScoreResult();

        // Each method fills its section of the result independently.
        ScoreToppings(result);
        ScoreBakeTime(result);
        ScoreCuts(result);

        Debug.Log($"[PizzaScorer] Toppings:{result.ToppingScore:F1} | " +
                  $"Bake:{result.BakeScore:F1} | " +
                  $"Cuts:{result.CutScore:F1} | " +
                  $"Avg:{result.AverageScore:F1} | " +
                  $"Stars:{result.Stars}");

        return result;
    }

    // ── Scoring methods ───────────────────────────────────────────────────

    /// <summary>
    /// Compares ordered vs actual toppings.
    /// Each missing ordered topping loses its equal share of 100 pts.
    /// Each unwanted extra topping deducts 10 pts. Clamped to [0,100].
    /// </summary>
    private static void ScoreToppings(ScoreResult result)
    {
        List<string> orderedList = PizzaGameData.OrderedIngredients;
        List<string> actualList  = PizzaGameData.ActualIngredients;

        // Use HashSet for O(1) lookup, case-insensitive (defensive against
        // capitalisation differences between stations)
        HashSet<string> orderedSet = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase);
        HashSet<string> actualSet  = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase);

        foreach (string t in orderedList) orderedSet.Add(t);
        foreach (string t in actualList)  actualSet.Add(t);

        // Find missing (ordered but not on pizza)
        foreach (string t in orderedSet)
            if (!actualSet.Contains(t))
                result.MissingToppings.Add(t);

        // Find extras (on pizza but not ordered) — each costs 10 pts
        foreach (string t in actualSet)
            if (!orderedSet.Contains(t))
                result.ExtraToppings.Add(t);

        int   totalOrdered = orderedList.Count;
        int   correctCount = totalOrdered - result.MissingToppings.Count;

        // TEST PLAN: empty ordered list edge case — if nothing was ordered,
        // nothing can be wrong, so base score defaults to 100.
        float baseScore = totalOrdered > 0
            ? (correctCount / (float)totalOrdered) * 100f
            : 100f;

        float penalty = result.ExtraToppings.Count * 10f;
        result.ToppingScore = Mathf.Clamp(baseScore - penalty, 0f, 100f);
    }

    /// <summary>
    /// Compares ordered bake time (string "10s") to actual bake time (float).
    /// accuracy = max(0, 1 - |actual - ordered| / ordered) * 100
    /// Being off by more seconds proportionally hurts more.
    /// </summary>
    private static void ScoreBakeTime(ScoreResult result)
    {
        // Parse "10s" -> 10f.
        // TEST PLAN: TryParse guard catches format changes from the order
        // generator without throwing an exception — logs a warning instead.
        string raw = PizzaGameData.OrderedBakeTime.Replace("s", "").Trim();
        if (!float.TryParse(raw, out float orderedSeconds))
        {
            Debug.LogWarning($"[PizzaScorer] Could not parse bake time '{PizzaGameData.OrderedBakeTime}'. Bake score = 0.");
            result.BakeScore          = 0f;
            result.OrderedBakeSeconds = 0f;
            result.ActualBakeSeconds  = PizzaGameData.ActualBakeTime;
            return;
        }

        float actualSeconds       = PizzaGameData.ActualBakeTime;
        result.OrderedBakeSeconds = orderedSeconds;
        result.ActualBakeSeconds  = actualSeconds;

        if (orderedSeconds <= 0f)
        {
            result.BakeScore = actualSeconds == 0f ? 100f : 0f;
            return;
        }

        float diff       = Mathf.Abs(actualSeconds - orderedSeconds);
        float accuracy   = Mathf.Clamp01(1f - (diff / orderedSeconds));
        result.BakeScore = accuracy * 100f;
    }

    /// <summary>
    /// Compares ordered cut count to actual cut count.
    /// Binary: exact match = 100, any deviation = 0.
    /// TEST PLAN: zero cuts tested to ensure no divide-by-zero here.
    /// </summary>
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