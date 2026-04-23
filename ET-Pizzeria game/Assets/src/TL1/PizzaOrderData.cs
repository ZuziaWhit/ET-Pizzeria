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




// -------------------- DESIGN PATTERNS --------------------

// Which patterns did you choose?

// 1. Singleton
// 2. Private Class Data


// -------------------- WHY THESE PATTERNS --------------------

// Singleton:
// I used the Singleton pattern for global systems like scoring/order tracking.
// This ensures there is only one shared instance accessible from anywhere in the game.
// It simplifies communication between systems like the manager and scoring logic without
// needing to pass references everywhere.

// Private Class Data:
// I used Private Class Data in PizzaOrderData and PizzaOrderPrefab by keeping variables
// like orderData protected/private and only modifying them through controlled methods
// (e.g., Initialize()). This prevents unintended modification and keeps the data consistent.


// -------------------- CLASS DIAGRAM (TEXT VERSION) --------------------

//        +------------------------+
//        |  PizzaOrderManager     |
//        +------------------------+
//                   |
//                   v
//        +------------------------+
//        |  PizzaOrderPrefab      |  <---- Super Class
//        +------------------------+
//                   ^
//                   |
//        +-------------------------------+
//        | SpecialPizzaOrderPrefab       |  <---- Sub Class
//        +-------------------------------+
//
//        +------------------------+
//        |  PizzaOrderData        |  (Private Class Data)
//        +------------------------+
//
//        +------------------------+
//        |  PizzaGameData         |  (Singleton)
//        +------------------------+


// -------------------- ALTERNATIVES & LIMITATIONS --------------------

// Could something else have worked?

// Yes:
// - Instead of Singleton, I could pass references manually or use dependency injection.
// - Instead of Private Class Data, I could make variables public, but that would reduce safety.

// When is this a bad choice?

// Singleton:
// - Bad when overused, as it creates tight coupling and makes testing harder
// - Not ideal if multiple instances are needed in the future

// Private Class Data:
// - Can be overly restrictive if frequent direct access is needed
// - Adds extra code if simple data structures are sufficient


// -------------------- SUMMARY --------------------

// These patterns were chosen to improve structure, maintainability, and safety:
// - Singleton ensures controlled global access
// - Private Class Data protects internal state and enforces proper usage