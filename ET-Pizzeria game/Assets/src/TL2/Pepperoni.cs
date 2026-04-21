using UnityEngine;

/////SubClass/////
public class Pepperoni : Topping
{
    /////****3***//////
   void Awake()
   {
        InitializeData(new ToppingData(name: "Pepperoni", cookTime: 1.2f, scoreValue:10));
   }

  /////***Override
  ////override = “I am the child class, and I am replacing that method.”
   public override void OnPlaced(Vector2 snappedPosition)
   {
        base.OnPlaced(snappedPosition);
        Debug.Log("Pepperoni placed!");
   }
}
