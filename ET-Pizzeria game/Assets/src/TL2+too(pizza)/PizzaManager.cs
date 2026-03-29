using UnityEngine;

public class PizzaManager : MonoBehaviour
{
    public GameObject pizzaPrefab;
    private GameObject currentPizza;
    private bool isSpawning = false;

    public void SpawnPizza()
    {
        Debug.Log("Spawn button pressed");

        if (isSpawning == true)
        {
            return;
        }
        isSpawning = true;

        if (currentPizza != null)
        {
            Destroy(currentPizza);
        }

        currentPizza = Instantiate(pizzaPrefab);
        currentPizza.transform.position = new Vector2(0, 0);
        currentPizza.transform.rotation = Quaternion.identity;

        isSpawning = false;
    }


    public void DeletePizza()
    {
        Debug.Log("Delete button pressed");

        if (currentPizza != null)
        {
            Destroy(currentPizza);
            currentPizza = null;
        }
    }
}