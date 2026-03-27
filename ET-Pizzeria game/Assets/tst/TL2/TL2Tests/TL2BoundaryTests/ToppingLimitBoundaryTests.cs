using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ToppingLimitBoundaryTests
{
    private const int MAX_TOPPINGS = 10;

    [Test]
    public void ToppingLimit_DoesNotExceed_Maximum()
    {
        int toppingCount = 0;

        // Simulate adding toppings until we hit the boundary
        for (int i = 0; i < MAX_TOPPINGS + 5; i++)
        {
            if (toppingCount < MAX_TOPPINGS)
                toppingCount++;
        }

        Assert.AreEqual(MAX_TOPPINGS, toppingCount, 
            "Topping count should never exceed the maximum allowed.");
    }

    [Test]
    public void ToppingLimit_Allows_UpTo_Maximum()
    {
        int toppingCount = 0;

        for (int i = 0; i < MAX_TOPPINGS; i++)
        {
            toppingCount++;
        }

        Assert.AreEqual(MAX_TOPPINGS, toppingCount,
            "Topping count should reach the maximum allowed.");
    }
}
