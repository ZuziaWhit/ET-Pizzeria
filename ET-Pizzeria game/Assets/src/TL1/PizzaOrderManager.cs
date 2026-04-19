using System.Collections.Generic;
using UnityEngine;

// This class is responsible for the "logic layer" of the system.
// It generates pizza orders and spawns prefabs to display them.
// It inherits from MonoBehaviour because it needs to exist in the scene
// and use Unity lifecycle methods like Start().
public class PizzaOrderManager : MonoBehaviour
{
    // Reference to the prefab that will visually display the pizza order.
    // This is assigned in the Unity Inspector.
    // Using a prefab allows us to dynamically create UI elements at runtime.
    public GameObject pizzaOrderPrefab;

    // Database of all possible ingredients.
    // This acts as a fixed pool from which random ingredients are selected.
    public string[] ingredientDatabase = {
        "Pepperoni", "Mushroom", "Onion", "Sausage",
        "Bacon", "Extra Cheese", "Black Olives", "Green Peppers"
    };

    // Possible cut types for the pizza.
    public string[] cutTypes = { "1", "2", "4" };

    // Possible bake times.
    public string[] bakeTimes = { "5s", "10s", "15s" };

    // Start() is called once when the scene begins.
    // This is where we trigger the system:
    // 1. Generate data
    // 2. Instantiate prefab
    // 3. Pass data into prefab
    void Start()
    {
        // Generate a random pizza order (data only)
        PizzaOrderData order = GenerateRandomOrder();

        // Instantiate a new prefab instance in the scene
        GameObject obj = Instantiate(pizzaOrderPrefab);

        // Pass the generated data into the prefab so it can display it
        obj.GetComponent<PizzaOrderPrefab>().Initialize(order);

        // Send order data to another system (likely scoring or validation)
        // This shows how the data layer can be reused across systems
        PizzaGameData.SetOrder(order.ingredients, order.bakeTime, order.cutType);
    }

    // This method generates a randomized pizza order.
    // It returns a PizzaOrderData object (pure data, no UI or behavior).
    PizzaOrderData GenerateRandomOrder()
    {
        // Create a new empty data object
        PizzaOrderData order = new PizzaOrderData();

        // HashSet is used to ensure all selected ingredient indices are unique.
        // This prevents duplicate ingredients without needing extra checks.
        HashSet<int> selected = new HashSet<int>();

        // Random.Range with integers is MIN inclusive, MAX exclusive.
        // So this generates a number between 3 and 6.
        int count = Random.Range(3, 7); // 3–6 ingredients

        // Keep selecting random indices until we reach the desired count
        while (selected.Count < count)
        {
            int randomIndex = Random.Range(0, ingredientDatabase.Length);
            selected.Add(randomIndex); // HashSet automatically ignores duplicates
        }

        // Convert selected indices into actual ingredient names
        foreach (int i in selected)
        {
            order.ingredients.Add(ingredientDatabase[i]);
        }

        // Randomly select cut type and bake time
        order.cutType = cutTypes[Random.Range(0, cutTypes.Length)];
        order.bakeTime = bakeTimes[Random.Range(0, bakeTimes.Length)];

        // Return the completed data object
        return order;
    }
}