using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PizzaOrderGenerator : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text cutText;
    public TMP_Text toppingsText;


    public string[] ingredientDatabase =
    {
        "Pepperoni",
        "Mushroom",
        "Onion",
        "Sausage",
        "Bacon",
        "Extra Cheese",
        "Black Olives",
        "Green Peppers"
    };

    public string[] cutTypes = { "1", "2", "4" };
    public string[] bakeTimes = { "5s", "10s", "15s" };

    [System.Serializable]
    public class PizzaOrder
    {
        public List<string> ingredients = new List<string>();
        public string cutType;
        public string bakeTime;
    }

    public PizzaOrder GenerateRandomOrder()
    {
        PizzaOrder order = new PizzaOrder();

        HashSet<int> selectedIngredients = new HashSet<int>();

        // Random count between 3 and 6
        int ingredientCount = Random.Range(3, 7); // 7 is exclusive

        while (selectedIngredients.Count < ingredientCount)
        {
            int randomIndex = Random.Range(0, ingredientDatabase.Length);
            selectedIngredients.Add(randomIndex);
        }

        foreach (int index in selectedIngredients)
        {
            order.ingredients.Add(ingredientDatabase[index]);
        }

        order.cutType = cutTypes[Random.Range(0, cutTypes.Length)];
        order.bakeTime = bakeTimes[Random.Range(0, bakeTimes.Length)];

        return order;
    }

    private void Start()
    {
        PizzaOrder order = GenerateRandomOrder();
        StartCoroutine(DisplayOrderRoutine(order));
    }

    IEnumerator DisplayOrderRoutine(PizzaOrder order)
    {
        // Clear UI first
        toppingsText.text = "";
        timeText.text = "";
        cutText.text = "";

        // Show toppings one by one
        foreach (string ingredient in order.ingredients)
        {
            toppingsText.text += ingredient + "\n";
            yield return new WaitForSeconds(1.5f);
        }

        // Show bake time
        timeText.text = "Bake Time: " + order.bakeTime;
        yield return new WaitForSeconds(1.5f);

        // Show cut type
        cutText.text = "Cut Type: " + order.cutType;
    }
}