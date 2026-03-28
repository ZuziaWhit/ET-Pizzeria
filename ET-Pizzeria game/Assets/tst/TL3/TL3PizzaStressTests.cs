using NUnit.Framework;
using UnityEngine;

public class TL3PizzaStressTests
{
    [Test]
    public void Bake100PizzasStressTest()
    {
        const int pizzaCount = 100;

        for (int i = 0; i < pizzaCount; i++)
        {
            GameObject obj = new GameObject();
            PizzaBaking baking = obj.AddComponent<PizzaBaking>();

            baking.SetBakeTime(10f);

            Assert.IsTrue(baking.isCooked);

            Object.DestroyImmediate(obj);
        }
    }
}