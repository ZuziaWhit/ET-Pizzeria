using UnityEngine;

/////Singleton//////
public class ToppingManager
{
  private static ToppingManager _instance; ////******static

  // Private constructor prevents "new ToppingManager()"
  private ToppingManager() 
  {
    MaxToppings = 20;
    CurrentCount = 0;
  }


  // Create new instance ////********dynamic
  public static ToppingManager GetInstance()
  {
    if (_instance == null)
      _instance = new ToppingManager();

    return _instance;
  }

  //global data
  public int MaxToppings { get; private set; }
  public int CurrentCount { get; private set; }

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
