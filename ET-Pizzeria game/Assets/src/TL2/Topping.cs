using UnityEngine;

public abstract class Topping : MonoBehaviour
{
    public abstract float CookTime { get; }
    public abstract int ScoreValue { get; }

    public virtual void OnPlaced(Vector2 snappedPosition)
    {
        transform.position = snappedPosition;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
