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
    public PizzaOrderData GenerateRandomOrder()
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



// -------------------- CODE REUSE & COPYRIGHT DISCUSSION --------------------

// Example of reuse in this project:
// I used AI-generated images (e.g., pizza/topping UI assets) in my Unity prefab.

// How this could violate copyright:
// AI-generated images may be trained on copyrighted datasets, and their licensing
// is not always clear. If the generated image closely resembles or reproduces
// protected content (e.g., branded food images, stock photos, or copyrighted art),
// using it without permission could violate copyright law. Additionally, if the
// image is taken from a platform with usage restrictions, using it commercially
// without a license could also be a violation.


// What I had to do to integrate it with my code:
// - Imported the image files into Unity (Assets folder)
// - Assigned them to UI elements (e.g., Image components in the prefab)
// - Linked them in the Inspector or via script to display in the pizza order UI
// - Ensured proper scaling, formatting, and compatibility with TextMeshPro/UI layout


// Legal implications if I market this code:
// - I could face copyright infringement claims if the images are not licensed
//   for commercial use
// - I may be required to remove or replace the assets
// - There could be legal or financial penalties depending on the violation
// - Distribution platforms (e.g., app stores) could reject or remove the project


// Fair Use Argument (limited and situational):
// One could argue fair use if:
// - The images are used for educational purposes (e.g., this assignment)
// - The use is non-commercial
// - The images are transformed (e.g., resized, stylized, or used as part of a system)
// - The use does not harm the market value of the original work

// However:
// Fair use is NOT guaranteed and depends on legal interpretation.
// For commercial projects, it is safer to use:
// - Original assets
// - Properly licensed assets
// - Public domain or royalty-free resources


// Summary:
// While AI-generated assets can accelerate development, developers must ensure
// proper licensing and legal compliance, especially for commercial use.