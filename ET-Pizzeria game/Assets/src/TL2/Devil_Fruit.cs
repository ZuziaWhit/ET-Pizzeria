using UnityEngine;

public class Devil_Fruit : Topping
{
    void Awake()
    {
        InitializeData(new ToppingData(name: "Devil Fruit", cookTime: 1.5f, scoreValue: 15));
    }
}
