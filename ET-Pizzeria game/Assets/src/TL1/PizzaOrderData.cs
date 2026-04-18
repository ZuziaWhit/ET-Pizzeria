using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PizzaOrderData
{
    public List<string> ingredients = new List<string>();
    public string cutType;
    public string bakeTime;
}