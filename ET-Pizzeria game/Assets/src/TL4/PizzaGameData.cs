// PizzaGameData.cs
// ─────────────────────────────────────────────────────────────────────────────
// Static data bridge — stores primitive types only so the 'scoring' assembly
// does NOT need to reference TL1 or Pizza assemblies, avoiding circular deps.
// ─────────────────────────────────────────────────────────────────────────────

using System.Collections.Generic;

public static class PizzaGameData
{
    // ── Order data (from PizzaOrderGenerator) ─────────────────────────────────
    public static List<string> OrderedIngredients { get; private set; } = new List<string>();
    public static string       OrderedBakeTime    { get; private set; } = "";   // e.g. "10s"
    public static string       OrderedCutType     { get; private set; } = "";   // e.g. "4"

    // ── Pizza data (from Pizza component) ─────────────────────────────────────
    public static List<string> ActualIngredients  { get; private set; } = new List<string>();
    public static float        ActualBakeTime     { get; private set; } = 0f;
    public static int          ActualCuts         { get; private set; } = 0;

    // ── Status ────────────────────────────────────────────────────────────────
    private static bool _orderSet;
    private static bool _pizzaSet;
    public  static bool IsReady => _orderSet && _pizzaSet;

    // ── Setters ───────────────────────────────────────────────────────────────

    /// <summary>
    /// Call this in PizzaOrderGenerator after generating the order:
    ///   PizzaGameData.SetOrder(order.ingredients, order.bakeTime, order.cutType);
    /// </summary>
    public static void SetOrder(List<string> ingredients, string bakeTime, string cutType)
    {
        OrderedIngredients = new List<string>(ingredients);
        OrderedBakeTime    = bakeTime;
        OrderedCutType     = cutType;
        _orderSet          = true;
    }

    /// <summary>
    /// Call this in PizzaManager.SUbmitPizza() before LoadScene():
    ///   Pizza p = currentPizza.GetComponent&lt;Pizza&gt;();
    ///   PizzaGameData.SetPizza(p.GetIngredients(), p.GetBakeTime(), p.GetCut());
    /// </summary>
    public static void SetPizza(List<string> ingredients, float bakeTime, int cuts)
    {
        ActualIngredients = new List<string>(ingredients);
        ActualBakeTime    = bakeTime;
        ActualCuts        = cuts;
        _pizzaSet         = true;
    }

    /// <summary>Call between rounds to reset state.</summary>
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