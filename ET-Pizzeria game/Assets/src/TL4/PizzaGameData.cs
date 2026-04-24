// ═══════════════════════════════════════════════════════════════════════════
// PizzaGameData.cs
// ───────────────────────────────────────────────────────────────────────────
// CONTRIBUTION — what this file does:
//   Static data bridge. Stores the customer order and the completed pizza as
//   plain C# primitives so EndDayScene can read both after the scene change.
//   Unity destroys all GameObjects on scene load, so this static class keeps
//   the data alive in memory between scenes without DontDestroyOnLoad.
//
// PATTERN — SINGLETON (small pattern):
//   A Singleton ensures exactly one shared data store exists globally.
//   We use a static class instead of a MonoBehaviour singleton — same
//   guarantee, zero scene setup, and no DontDestroyOnLoad needed.
//
//   JUSTIFY: Both the game scene (SetOrder, SetPizza) and the score scene
//   (ScoreFromGameData) need access to the same data. A singleton prevents
//   two conflicting copies existing at once.
//
//   ALTERNATIVE: A Unity ScriptableObject asset would also survive scene
//   loads and would show up in the Inspector for easier debugging. We chose
//   a static class to avoid the circular assembly dependency problem —
//   no assembly needs to hold a reference to a scene object, they just call
//   static methods.
//
//   BAD TIME TO USE: Two-player mode. One static store cannot hold two
//   separate orders at once. A list or dictionary keyed by player ID would
//   be needed instead.
//
// ASSEMBLY NOTE (why parameters are primitives, not PizzaOrder / Pizza):
//   Early versions referenced PizzaOrderGenerator and Pizza directly, which
//   caused a circular dependency: scoring -> TL1 -> scoring. Switching to
//   List<string>, float, and int broke the cycle so the 'scoring' assembly
//   needs zero references to teammates' assemblies.
//
// REUSE / COPYRIGHT:
//   Uses only System.Collections.Generic — a .NET standard library bundled
//   with Unity under the Unity license. No third-party code in this file.
//
// TEST PLAN NOTE:
//   IsReady is a deliberate test guard. If a teammate forgets to call
//   SetOrder() or SetPizza(), ScoreFromGameData() catches it immediately
//   and logs a clear error instead of silently producing a wrong score.
//   This was a specific test case designed so teammates would know right
//   away if their integration line was missing or broken.
//
// HOW TO WIRE (only two lines needed in teammates' scripts):
//   PizzaOrderGenerator.Start():
//       PizzaGameData.SetOrder(order.ingredients, order.bakeTime, order.cutType);
//   PizzaManager.SUbmitPizza():
//       Pizza p = currentPizza.GetComponent<Pizza>();
//       PizzaGameData.SetPizza(p.GetIngredients(), p.GetBakeTime(), p.GetCut());
// ═══════════════════════════════════════════════════════════════════════════

using System.Collections.Generic;

public static class PizzaGameData
{
    // ── Order data ────────────────────────────────────────────────────────
    // Set by PizzaOrderGenerator. Represents what the customer WANTS.
    public static List<string> OrderedIngredients { get; private set; } = new List<string>();
    public static string       OrderedBakeTime    { get; private set; } = "";  // e.g. "10s"
    public static string       OrderedCutType     { get; private set; } = "";  // e.g. "4"

    // ── Pizza data ────────────────────────────────────────────────────────
    // Set by PizzaManager on submit. Represents what the player ACTUALLY made.
    public static List<string> ActualIngredients  { get; private set; } = new List<string>();
    public static float        ActualBakeTime     { get; private set; } = 0f;
    public static int          ActualCuts         { get; private set; } = 0;

    // ── IsReady guard ─────────────────────────────────────────────────────
    // Both flags must be true before scoring is attempted.
    // If either is false, ScoreFromGameData() logs a clear error immediately.
    private static bool _orderSet;
    private static bool _pizzaSet;
    public  static bool IsReady => _orderSet && _pizzaSet;

    /// <summary>
    /// Called by PizzaOrderGenerator. Stores what the customer wants.
    /// Uses primitives (not PizzaOrder) to avoid circular assembly deps.
    /// </summary>
    public static void SetOrder(List<string> ingredients, string bakeTime, string cutType)
    {
        OrderedIngredients = new List<string>(ingredients); // defensive copy
        OrderedBakeTime    = bakeTime;
        OrderedCutType     = cutType;
        _orderSet          = true;
    }

    /// <summary>
    /// Called by PizzaManager before LoadScene. Stores what the player made.
    /// Uses primitives (not Pizza component) to avoid circular assembly deps.
    /// </summary>
    public static void SetPizza(List<string> ingredients, float bakeTime, int cuts)
    {
        ActualIngredients = new List<string>(ingredients); // defensive copy
        ActualBakeTime    = bakeTime;
        ActualCuts        = cuts;
        _pizzaSet         = true;
    }

    /// <summary>
    /// Resets all data. Call at the start of a new round so stale data
    /// from the previous round does not pollute the next score screen.
    /// </summary>
    public static void Clear()
    {
        OrderedIngredients = new List<string>();
        OrderedBakeTime    = "";
        OrderedCutType     = "";
        ActualIngredients  = new List<string>();
        ActualBakeTime     = 0f;
        ActualCuts         = 0;
        _orderSet          = false;
        _pizzaSet          = false;
    }
}