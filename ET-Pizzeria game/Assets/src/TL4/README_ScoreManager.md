# README_ScoreManager.md
# ScoreManager Prefab Documentation
# ═══════════════════════════════════════════════════════════════════════════

## What is this prefab?

ScoreManager is a GameObject prefab that lives in the EndDayScene.
It holds all the scoring logic for the end-of-day results screen.
Drop it into any scene that needs to display pizza scoring results.

---

## Components on this prefab

| Component         | Purpose                                                  |
|-------------------|----------------------------------------------------------|
| ScoreScreenUI     | Drives the TMP_Text fields and star images in the UI     |
| ScoreDebugMenu    | IMGUI overlay for testing scores without playing the game|

---

## How does the data get here?

ScoreManager does NOT receive data directly. Instead, two other scripts
store data into the static class PizzaGameData before the scene loads:

```
PizzaOrderGenerator.Start()
    └─ PizzaGameData.SetOrder(ingredients, bakeTime, cutType)

PizzaManager.SUbmitPizza()
    └─ PizzaGameData.SetPizza(ingredients, bakeTime, cuts)
    └─ SceneManager.LoadScene("EndDayScene")
```

When EndDayScene loads, ScoreScreenUI.Start() calls RefreshScore(),
which reads PizzaGameData and scores the pizza automatically.

**Question this answers:** "Where does the score screen get its data from?"
**Who is asking:** A teammate who needs to understand why the score shows
nothing, or anyone integrating a new station into the pipeline.
**Other questions they might need:**
- "Why does the score show -- / 100?" → PizzaGameData.IsReady was false.
  Check that SetOrder() and SetPizza() were both called before LoadScene.
- "How do I test without playing the full game?" → Use ScoreDebugMenu
  (backtick key in Play mode).
- "What assembly do I need to reference?" → Add 'scoring' to your .asmdef
  Assembly Definition References. See assembly setup notes below.

---

## Inspector wiring (ScoreScreenUI fields)

| Inspector Field    | Connect to                          | Required? |
|--------------------|-------------------------------------|-----------|
| Stars Text         | StarsText TMP_Text object           | Yes       |
| Topping Score Text | Topping Score > Topping Value       | Yes       |
| Bake Score Text    | Baking Score > BakingValue          | Yes       |
| Cut Score Text     | Cutting Score > CuttingValue        | Yes       |
| Average Score Text | Total Score > TotalValue            | Yes       |
| Detail Text        | Any TMP_Text for breakdown display  | Optional  |
| Star Images [0-4]  | Five Image components for star icons| Optional  |
| Star Filled Sprite | Sprite asset for a lit star         | Optional  |
| Star Empty Sprite  | Sprite asset for an unlit star      | Optional  |

**Question this answers:** "Which UI objects do I drag into which slot?"
**Who is asking:** Anyone setting up the prefab in a new scene for the
first time, or re-wiring after a scene restructure.

---

## Inspector wiring (ScoreDebugMenu fields)

| Inspector Field | Connect to                              |
|-----------------|-----------------------------------------|
| Score Display   | The ScoreManager GameObject itself      |
|                 | (ScoreScreenUI component will be found) |

**Question this answers:** "Why doesn't the debug menu update the UI
when I click Apply?" → Score Display is not wired up.

---

## Assembly setup (for teammates)

This prefab's scripts live in the 'scoring' assembly (.asmdef).
Any script that calls PizzaGameData.SetOrder() or SetPizza() must add
'scoring' as an Assembly Definition Reference in their own .asmdef:

1. Find your folder's .asmdef file in the Project window
2. Inspector → Assembly Definition References → + → search 'scoring'
3. Apply

Affected teammates: TL1 (PizzaOrderGenerator) and TL2+too(pizza) (PizzaManager).

**Question this answers:** "Why do I get CS0246 'PizzaGameData does not exist'?"
**Who is asking:** Any teammate trying to call SetOrder() or SetPizza().

---

## Scoring formula reference

| Category  | Formula                                          | Range   |
|-----------|--------------------------------------------------|---------|
| Toppings  | (correct / ordered) * 100 - (extras * 10)        | 0-100   |
| Bake Time | (1 - abs(actual-ordered) / ordered) * 100        | 0-100   |
| Cuts      | exact match = 100, any deviation = 0             | 0 or 100|
| Average   | (Toppings + Bake + Cuts) / 3                     | 0-100   |
| Stars     | >=90=5* | >=75=4* | >=60=3* | >=40=2* | else=1* | 1-5     |

**Question this answers:** "How is the score calculated?"
**Who is asking:** Teacher, QA tester, or teammate checking if their
station is being scored fairly.

---

## Design decisions / why we built it this way

### Why a static class (not DontDestroyOnLoad)?
Unity's DontDestroyOnLoad keeps a GameObject alive but requires a
scene reference to access it. A static class is globally accessible
from any assembly with no reference needed, and has no GameObject
lifecycle to manage.

### Why primitives in PizzaGameData (not PizzaOrder / Pizza objects)?
Early versions referenced PizzaOrderGenerator and Pizza directly.
This created a circular assembly dependency:
  scoring → TL1 → scoring (cycle detected, compile error)
Using List<string>, float, and int broke the cycle entirely.
The 'scoring' assembly now has zero references to teammates' assemblies.

### Why does ScoreScreenUI inherit from PizzaScoreDisplay?
To demonstrate dynamic binding (teacher requirement). The base class
declares virtual methods DisplayScore() and ShowErrorState(). The
runtime type of the object determines which override runs — currently
ScoreScreenUI, but any future display subclass would also work.

---

## Files in this system

| File                  | Role                                        |
|-----------------------|---------------------------------------------|
| PizzaGameData.cs      | Static data bridge (Singleton pattern)      |
| PizzaScorer.cs        | Scoring math + ScoreResult (Private Class Data)|
| PizzaScoreDisplay.cs  | Abstract base class (dynamic binding)       |
| ScoreScreenUI.cs      | Concrete UI driver (dynamic binding subclass)|
| ScoreDebugMenu.cs     | Debug/testing overlay                       |
| README_ScoreManager.md| This file                                   |
