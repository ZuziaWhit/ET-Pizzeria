using System.Collections.Generic;
using UnityEngine;

// This attribute allows the class to be visible and editable in the Unity Inspector
// even though it does NOT inherit from MonoBehaviour.
// It makes the data easy to view, debug, and pass between systems.
[System.Serializable]
public class PizzaOrderData
{
    // A list of ingredient names for the pizza order.
    // We use a List instead of an array because:
    // - The number of ingredients is dynamic (not fixed)
    // - It allows easy adding/removing elements at runtime
    public List<string> ingredients = new List<string>();

    // Represents how the pizza should be cut (e.g., 1, 2, or 4 slices).
    // Stored as a string to keep it simple and directly usable in UI display.
    public string cutType;

    // Represents how long the pizza should be baked (e.g., "5s", "10s").
    // Also stored as a string for easy display in the UI without formatting.
    public string bakeTime;
}