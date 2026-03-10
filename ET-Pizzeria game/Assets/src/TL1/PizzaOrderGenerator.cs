using System.Collections.Generic;
using UnityEngine;

public class PizzaOrderGenerator : MonoBehaviour
{

    /*
     -------------------------
     ORDER DATABASE
     -------------------------
     Unity 6 best practice:
     Keep order data separate so it can be edited later.
    */

    /*
    The ingredientDatabase array contains the different ingredients.
    */
    public string[] ingredientDatabase =
    {
        "Pepperoni",
        "Mushroom",
        "Onion",
        "Sausage",
        "Bacon",
        "ExtraCheese",
        "BlackOlives",
        "GreenPeppers"
    };

    /*
    The cutTypes array contains the amount of slices the pizza will be cut into.
    */
    public string[] cutTypes =
    {
        "1",
        "2",
        "4"
    };

    /*
    The bakeTimes array contains the different baking times in units of seconds.
    */
    public string[] bakeTimes =
    {
        "5s",
        "10s",
        "15s"
    };


    /*
     -------------------------
     ORDER OBJECT
     -------------------------
     Serializable so Unity can
     display it in the Inspector.
    */

    [System.Serializable]
    public class PizzaOrder
    {
        public string ingredient1;
        public string ingredient2;
        public string ingredient3;
        public string cutType;
        public string bakeTime;
    }


    /*
     -------------------------
     RANDOM ORDER GENERATOR
     -------------------------
    */

    public PizzaOrder GenerateRandomOrder()
    {
        PizzaOrder order = new PizzaOrder();

        HashSet<int> selectedIngredients = new HashSet<int>();

        // Ensure we get 3 unique ingredients
        while (selectedIngredients.Count < 3)
        {
            int randomIndex = Random.Range(0, ingredientDatabase.Length);
            selectedIngredients.Add(randomIndex);
        }

        int i = 0;

        foreach (int index in selectedIngredients)
        {
            if (i == 0)
                order.ingredient1 = ingredientDatabase[index];
            else if (i == 1)
                order.ingredient2 = ingredientDatabase[index];
            else if (i == 2)
                order.ingredient3 = ingredientDatabase[index];

            i++;
        }

        order.cutType = cutTypes[Random.Range(0, cutTypes.Length)];
        order.bakeTime = bakeTimes[Random.Range(0, bakeTimes.Length)];

        return order;
    }


    /*
     -------------------------
     UNITY INTEGRATION
     -------------------------
    */

    private void Start()
    {
        PizzaOrder order = GenerateRandomOrder();

        Debug.Log("New Pizza Order Generated/n");

        Debug.Log("Ingredient 1: " + order.ingredient1);
        Debug.Log("Ingredient 2: " + order.ingredient2);
        Debug.Log("Ingredient 3: " + order.ingredient3);
        Debug.Log("Cut Type: " + order.cutType);
        Debug.Log("Bake Time: " + order.bakeTime);
    }

}