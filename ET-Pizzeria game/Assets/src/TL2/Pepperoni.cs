using UnityEngine;

/////SubClass/////
public class Pepperoni : Topping
{
    /////****3***//////
   void Awake()
   {
        InitializeData(new ToppingData(name: "Pepperoni", cookTime: 1.2f, scoreValue:10));
   }
}
