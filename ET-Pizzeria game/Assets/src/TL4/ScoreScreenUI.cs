// ═══════════════════════════════════════════════════════════════════════════
// ScoreScreenUI.cs
// ───────────────────────────────────────────────────────────────────────────
// CONTRIBUTION — what this file does:
//   The actual end-of-day score screen. Inherits from PizzaScoreDisplay and
//   overrides DisplayScore() and ShowErrorState() to drive TMP_Text UI fields
//   and optional star Image components in the Unity scene.
//
// DYNAMIC BINDING — this is where it happens:
//   ScoreScreenUI IS the subclass in the dynamic binding demonstration.
//
//   SUPER CLASS:      PizzaScoreDisplay
//   SUB CLASS:        ScoreScreenUI
//   VIRTUAL METHODS:  DisplayScore(ScoreResult), ShowErrorState()
//
//   When PizzaScoreDisplay.RefreshScore() calls DisplayScore() or
//   ShowErrorState(), C# looks at the DYNAMIC TYPE of 'this' at runtime.
//   Because 'this' is a ScoreScreenUI object, it calls ScoreScreenUI's
//   overridden versions — not the base class versions.
//
//   If you changed the dynamic type to DebugScoreDisplay (a hypothetical
//   subclass that prints to Console), those same virtual calls would route
//   to DebugScoreDisplay.DisplayScore() instead. Same call site, different
//   method executes — that is dynamic binding.
//
//   STATICALLY BOUND: RefreshScore() in the base class. It is not virtual,
//   so regardless of whether 'this' is ScoreScreenUI or DebugScoreDisplay,
//   the same RefreshScore() always runs. The compiler locks it in.
//
// PREFAB NOTE:
//   This script is the key component on the ScoreManager prefab.
//   See README_ScoreManager.md for full prefab documentation.
//
// SCENE SETUP:
//   Attach to ScoreManager GameObject in EndDayScene.
//   Wire these TMP_Text fields in the Inspector:
//     starsText        — displays "★★★☆☆"
//     toppingScoreText — displays "Toppings:  80 / 100"
//     bakeScoreText    — displays "Bake Time: 90 / 100"
//     cutScoreText     — displays "Cuts:     100 / 100"
//     averageScoreText — displays "Overall:   90 / 100"
//     detailText       — optional breakdown of missing/extra toppings
//   Optionally wire:
//     starImages[]     — 5 Image components for graphical star display
//     starFilledSprite — sprite for a filled star
//     starEmptySprite  — sprite for an empty star
// ═══════════════════════════════════════════════════════════════════════════

using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// EXAM — SUB CLASS in dynamic binding demonstration.
/// Inherits PizzaScoreDisplay and overrides DisplayScore / ShowErrorState
/// to drive TMP_Text fields and star images in the Unity UI.
/// </summary>
public class ScoreScreenUI : PizzaScoreDisplay
{
    [Header("Score Text Fields")]
    [SerializeField] private TMP_Text starsText;
    [SerializeField] private TMP_Text toppingScoreText;
    [SerializeField] private TMP_Text bakeScoreText;
    [SerializeField] private TMP_Text cutScoreText;
    [SerializeField] private TMP_Text averageScoreText;

    [Header("Detail / Diagnostic Text (optional)")]
    [SerializeField] private TMP_Text detailText;

    [Header("Star Images (optional — assign 5 Image components in order)")]
    [SerializeField] private Image[] starImages;
    [SerializeField] private Sprite  starFilledSprite;
    [SerializeField] private Sprite  starEmptySprite;

    // ── Unity lifecycle ───────────────────────────────────────────────────
    // Calls base.Start() which triggers RefreshScore() -> DisplayScore().
    // 'override' here is dynamic binding in action for the Start() call too.
    protected override void Start()
    {
        base.Start(); // runs PizzaScoreDisplay.Start() which calls RefreshScore()
    }

    // ── DYNAMIC BINDING — overridden methods ──────────────────────────────

    /// <summary>
    /// EXAM — DYNAMICALLY BOUND override of PizzaScoreDisplay.DisplayScore().
    /// At runtime, when RefreshScore() calls DisplayScore(), C# routes here
    /// because the dynamic type of 'this' is ScoreScreenUI.
    /// Populates all TMP_Text fields and star images in the scene.
    /// </summary>
    protected override void DisplayScore(PizzaScorer.ScoreResult result)
    {
        // Stars
        SetText(starsText, BuildStarString(result.Stars));
        SetStarImages(result.Stars);

        // Category scores
        SetText(toppingScoreText, $"Toppings:  {result.ToppingScore:F0} / 100");
        SetText(bakeScoreText,    $"Bake Time: {result.BakeScore:F0} / 100");
        SetText(cutScoreText,     $"Cuts:      {result.CutScore:F0} / 100");
        SetText(averageScoreText, $"Overall:   {result.AverageScore:F0} / 100");

        // Detail breakdown (shows missing toppings, bake diff, cut comparison)
        if (detailText != null)
            detailText.text = BuildDetailString(result);
    }

    /// <summary>
    /// EXAM — DYNAMICALLY BOUND override of PizzaScoreDisplay.ShowErrorState().
    /// Called when PizzaGameData is not ready (missing SetOrder/SetPizza call).
    /// Shows "--/100" placeholders so the UI doesn't appear broken.
    /// </summary>
    protected override void ShowErrorState()
    {
        SetText(starsText,        "? / 5");
        SetText(toppingScoreText, "Toppings:  -- / 100");
        SetText(bakeScoreText,    "Bake Time: -- / 100");
        SetText(cutScoreText,     "Cuts:      -- / 100");
        SetText(averageScoreText, "Overall:   -- / 100");
        SetText(detailText,       "Score data unavailable.\n" +
                                  "Make sure PizzaGameData.SetOrder() and SetPizza() were called.");
    }

    // ── String builders ───────────────────────────────────────────────────

    private static string BuildStarString(int stars)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 1; i <= 5; i++)
            sb.Append(i <= stars ? "★" : "☆");
        return sb.ToString();
    }

    private static string BuildDetailString(PizzaScorer.ScoreResult r)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("── Toppings ──────────────────");
        if (r.MissingToppings.Count == 0 && r.ExtraToppings.Count == 0)
            sb.AppendLine("  Perfect! All toppings matched.");
        else
        {
            if (r.MissingToppings.Count > 0)
                sb.AppendLine("  Missing: " + string.Join(", ", r.MissingToppings));
            if (r.ExtraToppings.Count > 0)
                sb.AppendLine("  Extra (-10 each): " + string.Join(", ", r.ExtraToppings));
        }

        sb.AppendLine();
        sb.AppendLine("── Bake Time ─────────────────");
        sb.AppendLine($"  Ordered: {r.OrderedBakeSeconds}s   Actual: {r.ActualBakeSeconds:F1}s");
        float diff = Mathf.Abs(r.ActualBakeSeconds - r.OrderedBakeSeconds);
        sb.AppendLine(diff < 0.05f ? "  Perfect bake!" : $"  Off by {diff:F1}s");

        sb.AppendLine();
        sb.AppendLine("── Cuts ──────────────────────");
        sb.AppendLine($"  Ordered: {r.OrderedCuts}   Actual: {r.ActualCuts}");
        sb.AppendLine(r.OrderedCuts == r.ActualCuts ? "  Perfect cut!" : "  Wrong number of cuts.");

        return sb.ToString();
    }

    // ── Utility ───────────────────────────────────────────────────────────

    private static void SetText(TMP_Text field, string value)
    {
        if (field != null) field.text = value;
    }

    private void SetStarImages(int stars)
    {
        if (starImages == null || starImages.Length == 0) return;
        if (starFilledSprite == null || starEmptySprite == null) return;

        for (int i = 0; i < starImages.Length; i++)
        {
            if (starImages[i] == null) continue;
            starImages[i].sprite = (i < stars) ? starFilledSprite : starEmptySprite;
        }
    }
}