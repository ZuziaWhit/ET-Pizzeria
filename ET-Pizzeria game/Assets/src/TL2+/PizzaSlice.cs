using UnityEngine;

public class PizzaSlice : MonoBehaviour
{
    public void Slice(Vector2 start, Vector2 end)
    {
        Debug.Log($"Pizza sliced from {start} to {end}");
    }
}