using UnityEngine;

public class MozzarellaStars : Topping
{
    void Awake()
    {
        InitializeData(new ToppingData(name: "MozzarellaStars", cookTime: 2f, scoreValue: 25));
    }
}
