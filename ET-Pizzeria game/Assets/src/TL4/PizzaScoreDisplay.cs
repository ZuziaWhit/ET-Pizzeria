// ═══════════════════════════════════════════════════════════════════════════
// PizzaScoreDisplay.cs
// ───────────────────────────────────────────────────────────────────────────
// CONTRIBUTION — what this file does:
//   Defines the BASE CLASS for all score display behaviours in the game.
//   ScoreScreenUI (the actual end-of-day screen) inherits from this class.
//   This file exists specifically to demonstrate DYNAMIC BINDING — the
//   teacher's requirement — in a way that is genuine to the project.
//
// DYNAMIC BINDING — how it works here:
//   This abstract base class declares two virtual methods:
//     - DisplayScore(ScoreResult)  — how to show a score result on screen
//     - ShowErrorState()           — what to show if data is missing
//
//   ScoreScreenUI overrides both methods with its own TMP_Text implementation.
//   If you later add a "DebugScoreDisplay" that prints to Console instead of
//   UI, it would also override these methods with different behaviour.
//
//   STATIC TYPE  = PizzaScoreDisplay  (what the variable is declared as)
//   DYNAMIC TYPE = ScoreScreenUI      (what the object actually is at runtime)
//
//   Mock code demonstrating this (write on paper for exam):
//   ─────────────────────────────────────────────────────
//   // Static type is PizzaScoreDisplay, dynamic type is ScoreScreenUI
//   PizzaScoreDisplay display = new ScoreScreenUI();
//   display.ShowScore();   // calls ScoreScreenUI.ShowScore() at runtime
//                          // because dynamic type is ScoreScreenUI
//
//   // Change the dynamic type:
//   display = new DebugScoreDisplay();
//   display.ShowScore();   // NOW calls DebugScoreDisplay.ShowScore() instead
//   ─────────────────────────────────────────────────────
//
//   SUPER CLASS (static type):  PizzaScoreDisplay
//   SUB CLASS   (dynamic type): ScoreScreenUI
//   VIRTUAL FUNCTION:           DisplayScore() / ShowErrorState()
//
// STATICALLY BOUND METHOD — RefreshScore():
//   RefreshScore() is NOT virtual. It is sealed behaviour — it always calls
//   PizzaScorer.ScoreFromGameData() and routes to either DisplayScore() or
//   ShowErrorState(). No subclass can change this routing logic, only the
//   display methods. This is static binding — the compiler locks in which
//   RefreshScore() is called at compile time regardless of dynamic type.
//
// PATTERN — PRIVATE CLASS DATA (small pattern):
//   ScoreResult (defined in PizzaScorer.cs) is the Private Class Data pattern.
//   Its fields (ToppingScore, BakeScore, CutScore) have private setters —
//   they are written once during scoring and cannot be modified afterward.
//   The UI reads them but cannot corrupt them.
//
//   JUSTIFY: Protects score integrity. Once a pizza is scored, nothing should
//   be able to change the numbers — only display them.
//
//   BAD TIME TO USE: When the data genuinely needs to change after creation.
//   Forcing immutability on frequently-mutating data just adds complexity.
//
// REUSE / COPYRIGHT:
//   Inherits from UnityEngine.MonoBehaviour, which is part of Unity's engine.
//   Use requires a valid Unity license. Fair use argument: this is a student
//   educational project with no commercial intent, transformative in nature
//   (building an original game mechanic), and causes no market harm to Unity.
// ═══════════════════════════════════════════════════════════════════════════

using UnityEngine;

/// <summary>
/// Abstract base class for all score display behaviours.
/// Subclasses override DisplayScore and ShowErrorState to control
/// how results are presented (UI, console, debug overlay, etc.).
///
/// EXAM — SUPER CLASS for dynamic binding demonstration.
/// </summary>
public abstract class PizzaScoreDisplay : MonoBehaviour
{
    // ── Entry point (STATICALLY BOUND) ────────────────────────────────────
    // RefreshScore is NOT virtual. The routing logic (read data, decide
    // which method to call) is always the same regardless of subclass.
    // Only the display methods below are virtual and dynamically bound.
    //
    // EXAM — STATICALLY BOUND METHOD:
    //   In both cases (static type = PizzaScoreDisplay, dynamic type =
    //   ScoreScreenUI OR DebugScoreDisplay), RefreshScore() always calls
    //   THIS exact method — the compiler locks it in at compile time.
    /// <summary>
    /// Reads PizzaGameData, scores the pizza, and calls the appropriate
    /// display method. Safe to call multiple times (e.g. from debug menu).
    /// This method is statically bound — it cannot be overridden.
    /// </summary>
    public void RefreshScore()
    {
        PizzaScorer.ScoreResult result = PizzaScorer.ScoreFromGameData();

        // Route to the correct display method.
        // DisplayScore and ShowErrorState are virtual — dynamic binding
        // decides at runtime which subclass version gets called here.
        if (result == null)
            ShowErrorState();   // <-- dynamically bound call
        else
            DisplayScore(result); // <-- dynamically bound call
    }

    // ── Virtual display methods (DYNAMICALLY BOUND) ───────────────────────
    // Subclasses override these to control HOW results are shown.
    // The runtime type of 'this' determines which version executes.

    /// <summary>
    /// EXAM — VIRTUAL / DYNAMICALLY BOUND METHOD.
    /// Called when scoring succeeds. Override to display score data
    /// however the subclass needs to (TMP_Text, Console, overlay, etc.).
    /// Dynamic type = ScoreScreenUI  -> ScoreScreenUI.DisplayScore() runs.
    /// Dynamic type = DebugScoreDisplay -> DebugScoreDisplay.DisplayScore() runs.
    /// </summary>
    protected virtual void DisplayScore(PizzaScorer.ScoreResult result)
    {
        // Base implementation: log to console only.
        // Subclasses override this to drive actual UI.
        Debug.Log($"[PizzaScoreDisplay] Score — " +
                  $"Toppings:{result.ToppingScore:F0} " +
                  $"Bake:{result.BakeScore:F0} " +
                  $"Cuts:{result.CutScore:F0} " +
                  $"Stars:{result.Stars}");
    }

    /// <summary>
    /// EXAM — VIRTUAL / DYNAMICALLY BOUND METHOD.
    /// Called when PizzaGameData is not ready. Override to show an
    /// appropriate error state in the subclass's display system.
    /// </summary>
    protected virtual void ShowErrorState()
    {
        Debug.LogError("[PizzaScoreDisplay] Score data unavailable. " +
                       "Ensure SetOrder() and SetPizza() were called.");
    }

    // ── Unity lifecycle ───────────────────────────────────────────────────
    protected virtual void Start()
    {
        // Trigger initial score display when the scene loads.
        // Virtual so subclasses can extend Start() if needed.
        RefreshScore();
    }
}