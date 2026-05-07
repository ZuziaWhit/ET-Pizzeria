using UnityEngine;
// This script detects and monitors when a pizza enters and exits the oven and ends audio when a pizza exits.
public class OvenTrigger : MonoBehaviour
{
    private PizzaBaking pizza;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something entered oven.");

        if (other.CompareTag("Pizza"))
        {
            Debug.Log("Is Pizza");
            pizza = other.GetComponent<PizzaBaking>();
            pizza.canBake = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Pizza"))
        {
            Debug.Log("Not Pizza");
            pizza.canBake = false;
            pizza.isInOven = false;

            pizza.GetComponent<PizzaBaking>().StopBakingAudio();

            pizza = null;
        }
    }

    public bool HasPizza()
    {
        return pizza != null;
    }
}
