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