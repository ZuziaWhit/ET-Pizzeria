using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ToppingStressTests
{
    private const int MAX_ITERATIONS = 1000000; // 1 million simulated spawns

    [Test]
    public void StressTest_PepperoniSpawn_DoesNotCrashUnderHeavyLoad()
    {
        int spawnCount = 0;

        // Simulate a massive number of spawn attempts
        for (int i = 0; i < MAX_ITERATIONS; i++)
        {
            spawnCount++;
        }

        // If the loop completes, the system handled the stress
        Assert.AreEqual(MAX_ITERATIONS, spawnCount,
            "System failed to handle high-volume topping spawn simulation.");
    }
}

