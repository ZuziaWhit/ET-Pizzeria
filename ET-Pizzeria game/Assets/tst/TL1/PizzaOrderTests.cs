using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

// This test class validates the Pizza Order system.
// It focuses on correctness, data integrity, randomness, and prefab behavior.
// The goal is not only to check functionality, but also to ensure the system
// remains stable if future changes are introduced.
public class PizzaOrderTests
{
    // Reference to the manager object created for testing
    private GameObject managerObject;

    // Reference to the script under test
    private PizzaOrderManager manager;

    // Runs before each test
    [SetUp]
    public void Setup()
    {
        // Create a new GameObject to simulate the scene environment
        managerObject = new GameObject();

        // Attach the PizzaOrderManager script
        manager = managerObject.AddComponent<PizzaOrderManager>();

        // Create a simple prefab substitute for testing
        // This avoids needing full UI setup
        GameObject prefab = new GameObject();
        prefab.AddComponent<PizzaOrderPrefab>();

        // Assign prefab to manager
        manager.pizzaOrderPrefab = prefab;
    }

    // Runs after each test to clean up
    [TearDown]
    public void TearDown()
    {
        // Destroy test object to avoid interference between tests
        Object.DestroyImmediate(managerObject);
    }

    // ---------- CORE LOGIC TESTS ----------

    // Ensures generated orders always have between 3 and 6 ingredients
    // This directly validates the Random.Range(3, 7) logic
    [Test] public void IngredientCount_IsWithinRange()
    {
        var order = manager.GenerateRandomOrder();
        Assert.GreaterOrEqual(order.ingredients.Count, 3);
        Assert.LessOrEqual(order.ingredients.Count, 6);
    }

    // Ensures no duplicate ingredients appear in an order
    // Uses HashSet behavior as the underlying guarantee
    [Test] public void Ingredients_AreUnique()
    {
        var order = manager.GenerateRandomOrder();
        HashSet<string> unique = new HashSet<string>(order.ingredients);
        Assert.AreEqual(unique.Count, order.ingredients.Count);
    }

    // Ensures all ingredients come from the predefined database
    // Protects against invalid or unintended values
    [Test] public void Ingredients_AreFromDatabase()
    {
        var order = manager.GenerateRandomOrder();
        foreach (var ingredient in order.ingredients)
            Assert.Contains(ingredient, manager.ingredientDatabase);
    }

    // Ensures generated cut type is valid
    [Test] public void CutType_IsValid()
    {
        var order = manager.GenerateRandomOrder();
        Assert.Contains(order.cutType, manager.cutTypes);
    }

    // Ensures generated bake time is valid
    [Test] public void BakeTime_IsValid()
    {
        var order = manager.GenerateRandomOrder();
        Assert.Contains(order.bakeTime, manager.bakeTimes);
    }

    // ---------- RANDOMNESS TESTS ----------

    // Ensures two generated orders are not identical
    // This is a basic validation that randomness is functioning
    [Test] public void Orders_AreDifferent()
    {
        var o1 = manager.GenerateRandomOrder();
        var o2 = manager.GenerateRandomOrder();

        bool same = o1.ingredients.Count == o2.ingredients.Count;

        for (int i = 0; i < o1.ingredients.Count && i < o2.ingredients.Count; i++)
            if (o1.ingredients[i] != o2.ingredients[i]) same = false;

        Assert.IsFalse(same);
    }

    // Stress-style test: ensures system can generate many orders without crashing
    [Test] public void MultipleOrders_DoNotCrash()
    {
        for (int i = 0; i < 50; i++)
            Assert.IsNotNull(manager.GenerateRandomOrder());
    }

    // ---------- BOUNDARY TESTS ----------

    // Ensures ingredient count never goes below minimum
    [Test] public void IngredientCount_MinBoundary()
    {
        var order = manager.GenerateRandomOrder();
        Assert.GreaterOrEqual(order.ingredients.Count, 3);
    }

    // Ensures ingredient count never exceeds maximum
    [Test] public void IngredientCount_MaxBoundary()
    {
        var order = manager.GenerateRandomOrder();
        Assert.LessOrEqual(order.ingredients.Count, 6);
    }

    // ---------- DATA INTEGRITY TESTS ----------

    // Ensures generated order contains at least one ingredient
    [Test] public void Order_HasIngredients()
    {
        var order = manager.GenerateRandomOrder();
        Assert.IsNotEmpty(order.ingredients);
    }

    // Ensures cut type is not null
    [Test] public void Order_HasCutType()
    {
        var order = manager.GenerateRandomOrder();
        Assert.IsNotNull(order.cutType);
    }

    // Ensures bake time is not null
    [Test] public void Order_HasBakeTime()
    {
        var order = manager.GenerateRandomOrder();
        Assert.IsNotNull(order.bakeTime);
    }

    // ---------- PREFAB TESTS ----------

    // Ensures prefab can be instantiated without errors
    [Test] public void Prefab_CanBeInstantiated()
    {
        GameObject obj = Object.Instantiate(manager.pizzaOrderPrefab);
        Assert.IsNotNull(obj.GetComponent<PizzaOrderPrefab>());
    }

    // Ensures prefab has correct script attached
    [Test] public void Prefab_HasScript()
    {
        GameObject obj = Object.Instantiate(manager.pizzaOrderPrefab);
        Assert.IsNotNull(obj.GetComponent<PizzaOrderPrefab>());
    }

    // Ensures instantiated prefab object itself is valid
    [Test] public void Prefab_Instance_NotNull()
    {
        GameObject obj = Object.Instantiate(manager.pizzaOrderPrefab);
        Assert.IsNotNull(obj);
    }

    // ---------- DATABASE TESTS ----------

    // Ensures ingredient database is not empty
    [Test] public void IngredientDatabase_NotEmpty()
    {
        Assert.IsNotEmpty(manager.ingredientDatabase);
    }

    // Ensures cut types list is not empty
    [Test] public void CutTypes_NotEmpty()
    {
        Assert.IsNotEmpty(manager.cutTypes);
    }

    // Ensures bake times list is not empty
    [Test] public void BakeTimes_NotEmpty()
    {
        Assert.IsNotEmpty(manager.bakeTimes);
    }

    // ---------- CONSISTENCY TESTS ----------

    // Ensures generated ingredient count never exceeds database size
    [Test] public void IngredientCount_NeverExceedsDatabase()
    {
        var order = manager.GenerateRandomOrder();
        Assert.LessOrEqual(order.ingredients.Count, manager.ingredientDatabase.Length);
    }

    // Ensures generated order object is valid
    [Test] public void GeneratedOrder_NotNull()
    {
        var order = manager.GenerateRandomOrder();
        Assert.IsNotNull(order);
    }
}