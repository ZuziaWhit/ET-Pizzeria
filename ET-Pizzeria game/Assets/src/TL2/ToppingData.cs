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


  //Base Class
  public virtual string strGetName()
  {
    string something = "wrong name";
    return something;
  }
}

public class CustomTopping : ToppingData
{
  public CustomTopping(string name, float cookTime, int scoreValue)
        : base(name, cookTime, scoreValue)
  {

  }
  //Override SubClass
  public override string strGetName()
  {
    return Name;
  }
}
