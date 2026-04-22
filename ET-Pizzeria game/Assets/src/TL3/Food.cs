using UnityEngine;

public class Food : MonoBehaviour
{
    public float bakingTime = 0f;

    public virtual void Bake()
    {
        Debug.Log("Food is baking...");
    }
}
