// Import basic C# collection utilities (not strictly used here, but often included by default)
using System.Collections;

// Import Unity engine core functionality (MonoBehaviour, Debug, coroutines, etc.)
using UnityEngine;

// This class is a SUBCLASS (child class) of PizzaOrderPrefab
// It inherits all variables and methods from PizzaOrderPrefab
// and can OVERRIDE (change) specific behaviors → this demonstrates POLYMORPHISM
public class SpecialPizzaOrderPrefab : PizzaOrderPrefab
{
    // OVERRIDE of the Initialize method from the base class
    // "override" keyword enables DYNAMIC BINDING (runtime decision of which method to call)
    public override void Initialize(PizzaOrderData data)
    {
        // Custom behavior added BEFORE base functionality
        // This line logs a message in Unity Console when a special order is initialized
        Debug.Log("Special Pizza Order Initialized!");

        // Call the ORIGINAL implementation from the parent class
        // This ensures we DO NOT lose the base functionality (like assigning orderData, UI setup, etc.)
        base.Initialize(data);
    }

    // OVERRIDE of a protected coroutine from the base class
    // IEnumerator is used for COROUTINES in Unity (allows delays using yield)
    protected override IEnumerator DisplayOrderRoutine()
    {
        // SAFETY CHECK:
        // If any UI text references are missing, STOP execution immediately
        // Prevents NullReferenceException (very common Unity error)
        if (toppingsText == null || timeText == null || cutText == null)
            yield break; // exits coroutine early

        // Initialize UI display with a special label
        toppingsText.text = "⭐ SPECIAL ORDER ⭐\n";

        // Clear other UI fields before displaying new data
        timeText.text = "";
        cutText.text = "";

        // Loop through each ingredient in the order data
        // orderData is inherited from PizzaOrderPrefab (parent class)
        foreach (string ingredient in orderData.ingredients)
        {
            // Append each ingredient to the toppings UI text
            // ">>" gives a stylized list effect
            toppingsText.text += ">> " + ingredient + "\n";

            // Pause execution for 1 second between each ingredient
            // This creates a dynamic "typing" or "revealing" effect
            yield return new WaitForSeconds(1f); // faster than base version (custom behavior)
        }

        // After listing ingredients, display bake time
        // "🔥" emoji visually emphasizes urgency/special nature
        timeText.text = "🔥 Bake Time: " + orderData.bakeTime;

        // Wait again before showing next detail (improves pacing of UI)
        yield return new WaitForSeconds(1f);

        // Finally display the cut type
        // "✂" emoji adds visual clarity
        cutText.text = "✂ Cut Type: " + orderData.cutType;
    }
}
