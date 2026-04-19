using System.Collections;
using UnityEngine;
using TMPro;

// This class represents the "presentation layer" of the system.
// It is attached to the prefab and is responsible ONLY for displaying data.
// It does NOT generate data or control game logic.
public class PizzaOrderPrefab : MonoBehaviour
{
    // References to UI text elements (TextMeshPro)
    // These are assigned in the Unity Inspector.
    // They display different parts of the pizza order.
    public TMP_Text toppingsText;
    public TMP_Text timeText;
    public TMP_Text cutText;

    // Stores the pizza order data passed from the manager.
    // This is kept private because it should only be set through Initialize().
    private PizzaOrderData orderData;

    // This method is called by the PizzaOrderManager after the prefab is instantiated.
    // It "injects" the data into the prefab.
    // This keeps the prefab reusable, since it doesn't generate its own data.
    public void Initialize(PizzaOrderData data)
    {
        orderData = data;

        // Start displaying the order step-by-step using a coroutine
        StartCoroutine(DisplayOrderRoutine());
    }

    // Coroutine used to display the order over time.
    // Coroutines allow us to pause execution (WaitForSeconds)
    // without freezing the entire game.
    IEnumerator DisplayOrderRoutine()
    {
        // Clear all UI fields before displaying new data
        toppingsText.text = "";
        timeText.text = "";
        cutText.text = "";

        // Display ingredients one at a time
        // This creates a sequential, readable UI experience
        foreach (string ingredient in orderData.ingredients)
        {
            toppingsText.text += ingredient + "\n";

            // Wait 1.5 seconds before showing the next ingredient
            yield return new WaitForSeconds(1.5f);
        }

        // Display bake time after ingredients
        timeText.text = "Bake Time: " + orderData.bakeTime;

        // Pause again before showing the final detail
        yield return new WaitForSeconds(1.5f);

        // Display cut type last
        cutText.text = "Cut Type: " + orderData.cutType;
    }
}