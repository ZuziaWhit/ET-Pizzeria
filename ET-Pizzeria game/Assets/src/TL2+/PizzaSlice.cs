using UnityEngine;

public class PizzaSlice : MonoBehaviour
{
    public void Slice(Vector2 start, Vector2 end)
    {
        // Here you can implement:
        // - Split the pizza sprite into two pieces
        // - Add Rigidbody2D and physics
        // - Play cut sound or particle effects
        Debug.Log($"Pizza {name} sliced from {start} to {end}");
    }
}