using UnityEngine;

public class OvenTrigger : MonoBehaviour
{
    private PizzaBaking pizza;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something entered oven.");

        if (other.CompareTag("Pizza"))
        {
            pizza = other.GetComponent<PizzaBaking>();
            pizza.canBake = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Pizza"))
        {
            pizza.canBake = false;
            pizza.isInOven = false;
            pizza = null;
        }
    }

    public bool HasPizza()
    {
        return pizza != null;
    }
}