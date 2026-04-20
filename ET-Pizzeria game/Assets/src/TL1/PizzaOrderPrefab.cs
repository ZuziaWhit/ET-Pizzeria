// Import support for coroutines (IEnumerator, yield, etc.)
using System.Collections;

// Import Unity core engine functionality (MonoBehaviour, StartCoroutine, etc.)
using UnityEngine;

// Import TextMeshPro namespace (used for UI text elements in Unity)
// Required for TMP_Text type
using TMPro;

// BASE CLASS (SUPER CLASS)
// This class defines the DEFAULT behavior for displaying a pizza order
// Other classes (like SpecialPizzaOrderPrefab) will INHERIT from this
public class PizzaOrderPrefab : MonoBehaviour
{
    // PUBLIC UI REFERENCES
    // These should be assigned in the Unity Inspector
    // They represent different parts of the pizza order UI

    public TMP_Text toppingsText; // Displays list of ingredients
    public TMP_Text timeText;     // Displays bake time
    public TMP_Text cutText;      // Displays cut type

    // PROTECTED DATA STORAGE
    // "protected" means:
    // - Accessible inside this class
    // - ALSO accessible in subclasses (important for inheritance)
    // - NOT accessible from unrelated scripts
    protected PizzaOrderData orderData;

    // VIRTUAL METHOD (IMPORTANT FOR POLYMORPHISM)
    // "virtual" allows subclasses to OVERRIDE this method
    // This enables DYNAMIC BINDING (method chosen at runtime)
    public virtual void Initialize(PizzaOrderData data)
    {
        // Store the incoming order data into the class variable
        orderData = data;

        // Start a COROUTINE to display the order step-by-step
        // Coroutines allow delays (yield return) without freezing the game
        StartCoroutine(DisplayOrderRoutine());
    }

    // VIRTUAL COROUTINE METHOD
    // Subclasses can override this to change how the order is displayed
    protected virtual IEnumerator DisplayOrderRoutine()
    {
        // SAFETY CHECK (VERY IMPORTANT IN UNITY)
        // Prevents NullReferenceException if UI elements are not assigned
        // If any required UI element is missing → STOP execution immediately
        if (toppingsText == null || timeText == null || cutText == null)
            yield break;

        // CLEAR PREVIOUS TEXT
        // Ensures old data doesn't remain on screen
        toppingsText.text = "";
        timeText.text = "";
        cutText.text = "";

        // LOOP THROUGH INGREDIENTS
        // orderData.ingredients is likely a list/array of strings
        foreach (string ingredient in orderData.ingredients)
        {
            // Add each ingredient to the toppings display
            // "\n" creates a new line for each ingredient
            toppingsText.text += ingredient + "\n";

            // Pause for 1.5 seconds before showing next ingredient
            // This creates a gradual reveal effect (like typing animation)
            yield return new WaitForSeconds(1.5f);
        }

        // DISPLAY BAKE TIME AFTER INGREDIENTS
        // Concatenate string with data from orderData
        timeText.text = "Bake Time: " + orderData.bakeTime;

        // Wait again before showing next detail
        yield return new WaitForSeconds(1.5f);

        // DISPLAY CUT TYPE LAST
        cutText.text = "Cut Type: " + orderData.cutType;
    }
}


// -------------------- STATIC vs DYNAMIC BINDING --------------------

// Static Binding:
// Static binding happens at compile time. The method that gets called is determined
// by the variable’s declared (static) type, not the actual object it refers to.
// In C#, this applies to methods that are NOT marked as virtual.
// This means even if the object is a subclass, the base class version of the method
// will be called if the variable type is the base class.

// Dynamic Binding:
// Dynamic binding happens at runtime. The method that gets called depends on the
// object's actual (dynamic) type, not the variable type.
// In C#, this occurs when methods are marked as "virtual" in the base class and
// "override" in the subclass. This allows different classes to provide their own
// implementation of the same method, enabling polymorphism.

// In this project:
// - Initialize() is dynamically bound because it is virtual and overridden
// - A non-virtual method (like ResetText) would be statically bound

// Key Difference:
// Static Binding → decided at compile time → based on variable type
// Dynamic Binding → decided at runtime → based on object type






// -------------------- STATIC vs DYNAMIC TYPE (MOCK CODE) --------------------

// Static type is determined by the variable declaration (left side)
// Dynamic type is determined by the object created (right side)

// Example 1:
// Static type = PizzaOrderPrefab
// Dynamic type = PizzaOrderPrefab

/*
PizzaOrderPrefab prefab = new PizzaOrderPrefab();
*/

// Example 2:
// Static type = PizzaOrderPrefab
// Dynamic type = SpecialPizzaOrderPrefab

/*
PizzaOrderPrefab prefab2 = new SpecialPizzaOrderPrefab();
*/

// Example 3:
// Static type = PizzaOrderPrefab

/*
PizzaOrderPrefab prefab3;
*/

// Change dynamic type at runtime:

/*
prefab3 = new PizzaOrderPrefab();            // dynamic type = PizzaOrderPrefab
prefab3 = new SpecialPizzaOrderPrefab();     // dynamic type = SpecialPizzaOrderPrefab
*/

// Key idea:
// Static type = variable type (compile time)
// Dynamic type = actual object type (runtime)






// -------------------- ANSWERS: STATIC vs DYNAMIC BINDING --------------------

// Choose a dynamically bound method:
// Initialize(PizzaOrderData data)

// CASE 1:
// Static type = PizzaOrderPrefab
// Dynamic type = PizzaOrderPrefab
/*
PizzaOrderPrefab prefab = new PizzaOrderPrefab();
prefab.Initialize(orderData);
*/

// Method called:
// PizzaOrderPrefab.Initialize()


// Change the dynamic type:

// CASE 2:
// Static type = PizzaOrderPrefab
// Dynamic type = SpecialPizzaOrderPrefab
/*
prefab = new SpecialPizzaOrderPrefab();
prefab.Initialize(orderData);
*/

// Method called:
// SpecialPizzaOrderPrefab.Initialize()

// Explanation:
// Because Initialize() is virtual/override, the method call is resolved at runtime
// based on the object's dynamic type (this is dynamic binding).


// -------------------- STATIC BINDING --------------------

// Pick a statically bound method (non-virtual):
// Example: ResetText()

// CASE 1:
/*
prefab = new PizzaOrderPrefab();
prefab.ResetText();
*/

// Method called:
// PizzaOrderPrefab.ResetText()


// CASE 2:
/*
prefab = new SpecialPizzaOrderPrefab();
prefab.ResetText();
*/

// Method called:
// PizzaOrderPrefab.ResetText()

// Explanation:
// Since ResetText() is NOT virtual, it is statically bound.
// The method call is determined by the variable's static type (PizzaOrderPrefab),
// not the object's dynamic type.