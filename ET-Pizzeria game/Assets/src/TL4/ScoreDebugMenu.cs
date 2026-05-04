// ═══════════════════════════════════════════════════════════════════════════
// ScoreDebugMenu.cs
// ───────────────────────────────────────────────────────────────────────────
// CONTRIBUTION — what this file does:
//   On-screen IMGUI debug panel for testing the score system without playing
//   through the full game. Lets you manually set all order and pizza values,
//   push them into PizzaGameData, and trigger ScoreScreenUI to refresh.
//
// DYNAMIC BINDING NOTE:
//   This script holds a reference typed as PizzaScoreDisplay (the base class).
//   When it calls scoreDisplay.RefreshScore(), C# routes to the actual
//   subclass at runtime — currently ScoreScreenUI, but it would work with
//   any future subclass of PizzaScoreDisplay without changing this file.
//   That is dynamic binding in action from the caller's perspective.
//
//   EXAM MOCK CODE (write on paper):
//   ─────────────────────────────────────────────────────
//   PizzaScoreDisplay display = scoreDisplay;  // static type = base class
//   display.RefreshScore();                    // RefreshScore is NOT virtual
//                                              // (statically bound — always
//                                              //  calls base class version)
//                                              // BUT inside RefreshScore(),
//                                              // DisplayScore() IS virtual
//                                              // so it routes to ScoreScreenUI
//   ─────────────────────────────────────────────────────
//
// USAGE:
//   1. Attach to ScoreManager GameObject in EndDayScene.
//   2. Drag ScoreManager (which has ScoreScreenUI) into Score Display field.
//   3. Press Play — debug panel appears top-left of Game view.
//   4. Toggle with backtick (`) key or Hide button.
//   5. Use Quick Presets to one-click test common scenarios.
//   6. Hit Apply & Score to push values and see results.
//
// DISABLE IN BUILDS:
//   Uncheck this component before building, or wrap with #if UNITY_EDITOR.
// ═══════════════════════════════════════════════════════════════════════════

using System.Collections.Generic;
using UnityEngine;

public class ScoreDebugMenu : MonoBehaviour
{
    // ── Inspector ─────────────────────────────────────────────────────────
    [Header("Reference to score display in the scene")]
    // Typed as PizzaScoreDisplay (base class) — demonstrates dynamic binding.
    // Drag the ScoreManager GameObject here; ScoreScreenUI will be found.
    [SerializeField] private PizzaScoreDisplay scoreDisplay;

    // ── Panel state ───────────────────────────────────────────────────────
    private bool _visible = true;

    // ── ORDER fields (what was requested) ─────────────────────────────────
    private bool _pepperoni    = true;
    private bool _mushroom     = false;
    private bool _onion        = false;
    private bool _sausage      = false;
    private bool _bacon        = false;
    private bool _extraCheese  = false;
    private bool _blackOlives  = false;
    private bool _greenPeppers = false;
    private int  _bakeTimeIndex = 1;  // 0=5s 1=10s 2=15s
    private int  _cutTypeIndex  = 2;  // 0=1  1=2   2=4

    // ── PIZZA fields (what was actually made) ──────────────────────────────
    private bool   _aPepperoni    = true;
    private bool   _aMushroom     = false;
    private bool   _aOnion        = false;
    private bool   _aSausage      = false;
    private bool   _aBacon        = false;
    private bool   _aExtraCheese  = false;
    private bool   _aBlackOlives  = false;
    private bool   _aGreenPeppers = false;
    private string _actualBakeStr = "10";
    private string _actualCutsStr = "4";

    // ── Result display ────────────────────────────────────────────────────
    private string _resultText = "Press 'Apply & Score' to test.";

    // ── Lookup tables (must match PizzaOrderGenerator exactly) ────────────
    private readonly string[] _bakeTimes = { "5s", "10s", "15s" };
    private readonly string[] _cutTypes  = { "1", "2", "4" };

    // ── Style cache ───────────────────────────────────────────────────────
    private GUIStyle _headerStyle;
    private GUIStyle _resultStyle;
    private bool     _stylesBuilt;

    // ── Unity lifecycle ───────────────────────────────────────────────────

    private void Update()
    {
        // Toggle panel with backtick key
        if (Input.GetKeyDown(KeyCode.BackQuote))
            _visible = !_visible;
    }

    private void OnGUI()
    {
        BuildStyles();
        if (!_visible) { DrawShowButton(); return; }
        DrawPanel();
    }

    // ── GUI drawing ───────────────────────────────────────────────────────

    private void DrawShowButton()
    {
        if (GUI.Button(new Rect(10, 10, 130, 28), "[ Debug Menu  ` ]"))
            _visible = true;
    }

    private void DrawPanel()
    {
        float w = 430f, h = 560f;
        GUI.Box(new Rect(8, 8, w, h), "");
        GUILayout.BeginArea(new Rect(12, 12, w - 8, h - 8));

        // Header row
        GUILayout.BeginHorizontal();
        GUILayout.Label("  Score Debug Menu", _headerStyle);
        if (GUILayout.Button("Hide", GUILayout.Width(48))) _visible = false;
        GUILayout.EndHorizontal();
        GUILayout.Label("  ` key toggles  |  Does not affect build", SmallGray());
        GUILayout.Space(8);

        // ORDER section
        GUILayout.Label("── ORDER (what the customer wants) ──", _headerStyle);
        GUILayout.Label("Toppings:");
        GUILayout.BeginHorizontal();
        _pepperoni   = GUILayout.Toggle(_pepperoni,   " Pepperoni");
        _mushroom    = GUILayout.Toggle(_mushroom,    " Mushroom");
        _onion       = GUILayout.Toggle(_onion,       " Onion");
        _sausage     = GUILayout.Toggle(_sausage,     " Sausage");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        _bacon       = GUILayout.Toggle(_bacon,       " Bacon");
        _extraCheese = GUILayout.Toggle(_extraCheese, " Extra Cheese");
        _blackOlives = GUILayout.Toggle(_blackOlives, " Black Olives");
        _greenPeppers= GUILayout.Toggle(_greenPeppers," Green Peppers");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Bake Time:", GUILayout.Width(80));
        _bakeTimeIndex = GUILayout.SelectionGrid(_bakeTimeIndex, _bakeTimes, 3);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Cut Type:", GUILayout.Width(80));
        _cutTypeIndex = GUILayout.SelectionGrid(_cutTypeIndex, _cutTypes, 3);
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        // PIZZA section
        GUILayout.Label("── PIZZA (what the player made) ──", _headerStyle);
        GUILayout.Label("Toppings applied:");
        GUILayout.BeginHorizontal();
        _aPepperoni   = GUILayout.Toggle(_aPepperoni,   " Pepperoni");
        _aMushroom    = GUILayout.Toggle(_aMushroom,    " Mushroom");
        _aOnion       = GUILayout.Toggle(_aOnion,       " Onion");
        _aSausage     = GUILayout.Toggle(_aSausage,     " Sausage");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        _aBacon       = GUILayout.Toggle(_aBacon,       " Bacon");
        _aExtraCheese = GUILayout.Toggle(_aExtraCheese, " Extra Cheese");
        _aBlackOlives = GUILayout.Toggle(_aBlackOlives, " Black Olives");
        _aGreenPeppers= GUILayout.Toggle(_aGreenPeppers," Green Peppers");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Bake Time (sec):", GUILayout.Width(120));
        _actualBakeStr = GUILayout.TextField(_actualBakeStr, GUILayout.Width(60));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Number of Cuts:", GUILayout.Width(120));
        _actualCutsStr = GUILayout.TextField(_actualCutsStr, GUILayout.Width(60));
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        // Quick presets
        GUILayout.Label("── Quick Presets ──", _headerStyle);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Perfect Pizza"))  ApplyPreset_Perfect();
        if (GUILayout.Button("Wrong Toppings")) ApplyPreset_WrongToppings();
        if (GUILayout.Button("Bad Bake+Cuts"))  ApplyPreset_BadBakeAndCuts();
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        // Apply button
        GUI.backgroundColor = new Color(0.3f, 0.8f, 0.4f);
        if (GUILayout.Button("  Apply & Score  ", GUILayout.Height(32)))
            ApplyAndScore();
        GUI.backgroundColor = Color.white;

        GUILayout.Space(6);
        GUILayout.Label(_resultText, _resultStyle);

        GUILayout.EndArea();
    }

    // ── Core apply logic ──────────────────────────────────────────────────

    private void ApplyAndScore()
    {
        // Parse text field inputs
        if (!float.TryParse(_actualBakeStr, out float bakeTime)) bakeTime = 0f;
        if (!int.TryParse(_actualCutsStr,   out int   cuts))     cuts     = 0;

        // Push order into PizzaGameData
        PizzaGameData.SetOrder(
            BuildToppingList(_pepperoni, _mushroom, _onion, _sausage,
                             _bacon, _extraCheese, _blackOlives, _greenPeppers),
            _bakeTimes[_bakeTimeIndex],
            _cutTypes[_cutTypeIndex]);

        // Push pizza into PizzaGameData
        PizzaGameData.SetPizza(
            BuildToppingList(_aPepperoni, _aMushroom, _aOnion, _aSausage,
                             _aBacon, _aExtraCheese, _aBlackOlives, _aGreenPeppers),
            bakeTime,
            cuts);

        // Trigger score refresh via base class reference (dynamic binding demo)
        // scoreDisplay is typed as PizzaScoreDisplay but holds a ScoreScreenUI.
        // RefreshScore() is statically bound (not virtual) — same method always.
        // Inside it, DisplayScore() is virtual — routes to ScoreScreenUI at runtime.
        if (scoreDisplay != null)
            scoreDisplay.RefreshScore();
        else
            Debug.LogWarning("[ScoreDebugMenu] scoreDisplay not assigned in Inspector.");

        // Show result summary in the panel
        PizzaScorer.ScoreResult r = PizzaScorer.ScoreFromGameData();
        if (r == null) { _resultText = "ERROR — check Console."; return; }

        _resultText =
            $"Toppings: {r.ToppingScore:F0}/100\n" +
            $"Bake:     {r.BakeScore:F0}/100\n" +
            $"Cuts:     {r.CutScore:F0}/100\n" +
            $"Average:  {r.AverageScore:F0}/100\n" +
            $"Stars:    {r.Stars} / 5" +
            (r.MissingToppings.Count > 0 ? $"\nMissing: {string.Join(", ", r.MissingToppings)}" : "") +
            (r.ExtraToppings.Count  > 0 ? $"\nExtra:   {string.Join(", ", r.ExtraToppings)}"   : "");
    }

    // ── Presets ───────────────────────────────────────────────────────────

    private void ApplyPreset_Perfect()
    {
        SetOrder(true, true, false, false, false, false, false, false, 1, 2);
        SetPizza(true, true, false, false, false, false, false, false, "10", "4");
    }

    private void ApplyPreset_WrongToppings()
    {
        // Order: Pepperoni+Mushroom+Onion. Pizza: missing Onion, extra Bacon.
        SetOrder(true, true, true, false, false, false, false, false, 1, 2);
        SetPizza(true, true, false, false, true, false, false, false, "10", "4");
    }

    private void ApplyPreset_BadBakeAndCuts()
    {
        // Order: Pepperoni, 5s, 2 cuts. Pizza: correct topping, 10s bake, 4 cuts.
        SetOrder(true, false, false, false, false, false, false, false, 0, 1);
        SetPizza(true, false, false, false, false, false, false, false, "10", "4");
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    private void SetOrder(bool pep, bool mush, bool onion, bool saus,
                          bool bacon, bool cheese, bool olive, bool pepper,
                          int bakeIdx, int cutIdx)
    {
        _pepperoni = pep; _mushroom = mush; _onion = onion; _sausage = saus;
        _bacon = bacon; _extraCheese = cheese; _blackOlives = olive; _greenPeppers = pepper;
        _bakeTimeIndex = bakeIdx; _cutTypeIndex = cutIdx;
    }

    private void SetPizza(bool pep, bool mush, bool onion, bool saus,
                          bool bacon, bool cheese, bool olive, bool pepper,
                          string bakeStr, string cutsStr)
    {
        _aPepperoni = pep; _aMushroom = mush; _aOnion = onion; _aSausage = saus;
        _aBacon = bacon; _aExtraCheese = cheese; _aBlackOlives = olive; _aGreenPeppers = pepper;
        _actualBakeStr = bakeStr; _actualCutsStr = cutsStr;
    }

    private List<string> BuildToppingList(
        bool pep, bool mush, bool onion, bool saus,
        bool bacon, bool cheese, bool olive, bool pepper)
    {
        List<string> list = new List<string>();
        if (pep)    list.Add("Pepperoni");
        if (mush)   list.Add("Mushroom");
        if (onion)  list.Add("Onion");
        if (saus)   list.Add("Sausage");
        if (bacon)  list.Add("Bacon");
        if (cheese) list.Add("Extra Cheese");
        if (olive)  list.Add("Black Olives");
        if (pepper) list.Add("Green Peppers");
        return list;
    }

    private void BuildStyles()
    {
        if (_stylesBuilt) return;
        _headerStyle = new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Bold,
            normal    = { textColor = Color.white }
        };
        _resultStyle = new GUIStyle(GUI.skin.label)
        {
            normal    = { textColor = new Color(0.4f, 1f, 0.5f) },
            fontStyle = FontStyle.Bold,
            fontSize  = 13
        };
        _stylesBuilt = true;
    }

    private static GUIStyle SmallGray()
    {
        GUIStyle s = new GUIStyle(GUI.skin.label);
        s.normal.textColor = new Color(0.6f, 0.6f, 0.6f);
        s.fontSize = 11;
        return s;
    }
}