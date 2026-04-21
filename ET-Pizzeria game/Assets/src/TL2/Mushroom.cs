using UnityEngine;

/////SubClass/////
public class Mushroom : Topping
{
    /////****3***//////
    void Awake()
    {
        InitializeData(new ToppingData(name: "Mushroom", cookTime: 1.0f, scoreValue: 5));
    }

    /////***Override
    ////override = “I am the child class, and I am replacing that method.”
   
   /* public override void OnPlaced(Vector2 snappedPosition)
   {
        base.OnPlaced(snappedPosition);
        Debug.Log("Mushroom placed!");
   }*/
}
