using UnityEngine;

public class Blood_Berry : Topping
{
    void Awake()
    {
        InitializeData(new ToppingData(name: "Blood Berry", cookTime: 1.8f, scoreValue: 20));
    }
}
