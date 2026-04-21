using UnityEngine;

/////SubClass/////
public class Mushroom : Topping
{
    /////****3***//////
    void Awake()
    {
        InitializeData(new ToppingData(name: "Mushroom", cookTime: 1.0f, scoreValue: 5));
    }
}
