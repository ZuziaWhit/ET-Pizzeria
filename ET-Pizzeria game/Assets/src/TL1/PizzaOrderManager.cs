using System.Collections.Generic;
using UnityEngine;

public class PizzaOrderManager : MonoBehaviour
{
    public GameObject pizzaOrderPrefab;

    public string[] ingredientDatabase = {
        "Pepperoni", "Mushroom", "Onion", "Sausage",
        "Bacon", "Extra Cheese", "Black Olives", "Green Peppers"
    };

    public string[] cutTypes = { "1", "2", "4" };
    public string[] bakeTimes = { "5s", "10s", "15s" };

    void Start()
    {
        PizzaOrderData order = GenerateRandomOrder();

        GameObject obj = Instantiate(pizzaOrderPrefab);
        obj.GetComponent<PizzaOrderPrefab>().Initialize(order);

        // scoring (keep this!)
        PizzaGameData.SetOrder(order.ingredients, order.bakeTime, order.cutType);
    }

    PizzaOrderData GenerateRandomOrder()
    {
        PizzaOrderData order = new PizzaOrderData();

        HashSet<int> selected = new HashSet<int>();

        int count = Random.Range(3, 7); // 3–6 ingredients

        while (selected.Count < count)
        {
            int randomIndex = Random.Range(0, ingredientDatabase.Length);
            selected.Add(randomIndex);
        }

        foreach (int i in selected)
        {
            order.ingredients.Add(ingredientDatabase[i]);
        }

        order.cutType = cutTypes[Random.Range(0, cutTypes.Length)];
        order.bakeTime = bakeTimes[Random.Range(0, bakeTimes.Length)];

        return order;
    }
}