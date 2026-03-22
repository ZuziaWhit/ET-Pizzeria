using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PizzaOrderGeneratorTests
{
    PizzaOrderGenerator generator;

    [SetUp]
    public void Setup()
    {
        GameObject obj = new GameObject();
        generator = obj.AddComponent<PizzaOrderGenerator>();
    }

    // ✅ Boundary Test 1: Ingredient Count + Uniqueness
    [Test]
    public void IngredientCount_ShouldBeExactlyThree_AndUnique()
    {
        var order = generator.GenerateRandomOrder();

        Assert.AreEqual(3, order.ingredients.Count, "Ingredient count is not 3");

        HashSet<string> unique = new HashSet<string>(order.ingredients);
        Assert.AreEqual(3, unique.Count, "Ingredients are not unique");
    }

    // ✅ Boundary Test 2: Valid Cut and Bake Values
    [Test]
    public void GeneratedValues_ShouldBeValid()
    {
        var order = generator.GenerateRandomOrder();

        string[] validCuts = { "1", "2", "4" };
        string[] validTimes = { "5s", "10s", "15s" };

        Assert.Contains(order.cutType, validCuts);
        Assert.Contains(order.bakeTime, validTimes);
    }

    // ✅ Stress Test: 500 iterations
    [Test]
    public void StressTest_500Orders_ShouldAllBeValid()
    {
        for (int i = 0; i < 500; i++)
        {
            var order = generator.GenerateRandomOrder();

            Assert.IsNotNull(order);
            Assert.AreEqual(3, order.ingredients.Count);

            HashSet<string> unique = new HashSet<string>(order.ingredients);
            Assert.AreEqual(3, unique.Count);

            Assert.IsNotNull(order.cutType);
            Assert.IsNotNull(order.bakeTime);
        }
    }
}
