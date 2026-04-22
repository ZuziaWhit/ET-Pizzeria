using UnityEngine;

/////SuperClass that uses ToppingData/////
/////****2***//////
public abstract class Topping : MonoBehaviour
{
    private ToppingData data;

    //Subclasses call this to set their data
    protected void InitializeData(ToppingData data)
    {
        this.data = data;
    }

    //Public getters
    public float CookTime => data.CookTime;
    public int ScoreValue => data.ScoreValue;
    public string ToppingName => data.Name;



    ////*****Virtual
    ///virtual = “This method can be replaced by a child class if it wants to.”
    public virtual void OnPlaced(Vector2 snappedPosition)
    {
        transform.position = snappedPosition;
    }
    
}
