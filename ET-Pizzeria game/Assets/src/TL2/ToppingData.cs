using UnityEngine;

/////Private Data Class Container/////
//***1***///
public class ToppingData
{
  public string Name { get; }
  public float CookTime { get; }
  public int ScoreValue { get; }

  //Constructor
  public ToppingData(string name, float cookTime, int scoreValue)
  {
    Name = name;
    CookTime = cookTime;
    ScoreValue = scoreValue;
  }  
}
