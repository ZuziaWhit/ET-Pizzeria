using UnityEngine;

/////Singleton//////
public class ToppingManager
{
  private static ToppingManager _instance; ////******static

   //global data
  public int MaxToppings { get; private set; }
  public int CurrentCount { get; private set; }

  // Private constructor prevents "new ToppingManager()"
  private ToppingManager() 
  {
    Debug.Log("Create manager");

    MaxToppings = 20;
    CurrentCount = 0;
    
  }


//Thread Safe****

  // Create new instance ////********dynamic
  public static ToppingManager GetInstance()
  {
    if (_instance == null)
    {
    _instance = new ToppingManager();
    }

    return _instance;
  }

 

  //global behavior
  public bool CanPlaceTopping()
  {
    return CurrentCount < MaxToppings;
  }

  public void RegisterToppingPlaced()
  {
    CurrentCount++;
  }
}
