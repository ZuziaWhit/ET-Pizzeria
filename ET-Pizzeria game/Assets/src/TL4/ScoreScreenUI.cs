// ScoreScreenUI.cs
// ─────────────────────────────────────────────────────────────────────────────
// Attach to a GameObject in your "EndDayScene".
// Wire up the TMP_Text fields (and optionally star Image arrays) in the Inspector.
//
// Minimal scene setup
// ───────────────────
// You need TMP_Text objects for:
//   • starsText          — e.g. "★★★☆☆"
//   • toppingScoreText   — e.g. "Toppings:  80 / 100"
//   • bakeScoreText      — e.g. "Bake Time: 65 / 100"
//   • cutScoreText       — e.g. "Cuts:     100 / 100"
//   • averageScoreText   — e.g. "Overall:   82 / 100"
//   • detailText         — diagnostic breakdown (missing/extra toppings, times, cuts)
//
// Optionally wire up:
//   • starImages[]       — 5 UnityEngine.UI.Image components; filled stars get a
//                          full-color sprite, empty ones get a dimmed sprite.
// ─────────────────────────────────────────────────────────────────────────────

using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScreenUI : MonoBehaviour
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

    // ── Unity lifecycle ───────────────────────────────────────────────────────

    private void Start()
    {
        PizzaScorer.ScoreResult result = PizzaScorer.ScoreFromGameData();

        if (result == null)
        {
            ShowErrorState();
            return;
        }

        DisplayScore(result);

        // Clean up static data so it doesn't bleed into the next round
        PizzaGameData.Clear();
    }

    // ── Display helpers ───────────────────────────────────────────────────────

    private void DisplayScore(PizzaScorer.ScoreResult result)
    {
        // ── Star display ─────────────────────────────────────────────────────
        SetText(starsText, BuildStarString(result.Stars));
        SetStarImages(result.Stars);

        // ── Category scores ──────────────────────────────────────────────────
        SetText(toppingScoreText, $"Toppings:  {result.ToppingScore:F0} / 100");
        SetText(bakeScoreText,    $"Bake Time: {result.BakeScore:F0} / 100");
        SetText(cutScoreText,     $"Cuts:      {result.CutScore:F0} / 100");
        SetText(averageScoreText, $"Overall:   {result.AverageScore:F0} / 100");

        // ── Detail / diagnostic breakdown ────────────────────────────────────
        if (detailText != null)
            detailText.text = BuildDetailString(result);
    }

    private void ShowErrorState()
    {
        SetText(starsText,        "? / 5");
        SetText(toppingScoreText, "Toppings:  -- / 100");
        SetText(bakeScoreText,    "Bake Time: -- / 100");
        SetText(cutScoreText,     "Cuts:      -- / 100");
        SetText(averageScoreText, "Overall:   -- / 100");
        SetText(detailText,       "Score data unavailable.\nMake sure PizzaGameData.SetOrder() and SetPizza() were called.");
    }

    // ── String builders ───────────────────────────────────────────────────────

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

        // Toppings detail
        sb.AppendLine("── Toppings ──────────────────");
        if (r.MissingToppings.Count == 0 && r.ExtraToppings.Count == 0)
        {
            sb.AppendLine("  Perfect! All toppings matched.");
        }
        else
        {
            if (r.MissingToppings.Count > 0)
                sb.AppendLine("  Missing: " + string.Join(", ", r.MissingToppings));
            if (r.ExtraToppings.Count > 0)
                sb.AppendLine("  Extra (-10 each): " + string.Join(", ", r.ExtraToppings));
        }

        // Bake time detail
        sb.AppendLine();
        sb.AppendLine("── Bake Time ─────────────────");
        sb.AppendLine($"  Ordered: {r.OrderedBakeSeconds}s   Actual: {r.ActualBakeSeconds:F1}s");
        float diff = Mathf.Abs(r.ActualBakeSeconds - r.OrderedBakeSeconds);
        if (diff < 0.05f)
            sb.AppendLine("  Perfect bake!");
        else
            sb.AppendLine($"  Off by {diff:F1}s");

        // Cut detail
        sb.AppendLine();
        sb.AppendLine("── Cuts ──────────────────────");
        sb.AppendLine($"  Ordered: {r.OrderedCuts}   Actual: {r.ActualCuts}");
        sb.AppendLine(r.OrderedCuts == r.ActualCuts ? "  Perfect cut!" : "  Wrong number of cuts.");

        return sb.ToString();
    }

    // ── Utility ───────────────────────────────────────────────────────────────

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