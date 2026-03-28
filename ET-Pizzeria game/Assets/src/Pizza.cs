using System.Collections.Generic;
using UnityEngine;

public class Pizza : MonoBehaviour
{
    [Header("Pizza Data")]
    public List<string> ingredients = new List<string>();
    public float bakingTime = 0f;
    public int numberCuts = 0;

    // Start of Sam Code
    public void AddCut()
    {
        numberCuts++;
        Debug.Log("Pizza cut into " + numberCuts + " slices");
    }
    // End of Sam code
}