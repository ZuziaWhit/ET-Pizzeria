using System.Collections.Generic;
using UnityEngine;

public class Pizza : MonoBehaviour
{
    [Header("Pizza Data")]
    public List<string> ingredients = new List<string>(); //Will become Private
    public float bakingTime = 0f; //Will become Private
    private int numberCuts = 0;

    //This is to be use by the Baking Station
    public void AddBakeTime(float baketime)
        {
            bakingTime = baketime;
            Debug.Log("Pizza is baking" + bakingTime);
        }

    //This is to be use by the Topping Station
    public void AddIngredient(string ingredient)
    {
        ingredients.Add(ingredient);
        Debug.Log("Added: " + ingredient);
    }

    //This is to be use by the Cutting Station
    public void AddCut()
    {
        numberCuts++;
        Debug.Log("Pizza cut into " + numberCuts + " slices");
    }

    public int GetCut()
    {
        Debug.Log(numberCuts + " slices"); 
        return numberCuts;
    }

    public float GetBakeTime()
    {
        Debug.Log(bakingTime + " seconds"); 
        return bakingTime;
    }

    public List<string> GetIngredients() //idk if this will work
    {
        Debug.Log(ingredients + " on the pizza"); 
        return ingredients;
    }
}