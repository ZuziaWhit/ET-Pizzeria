using UnityEngine;

public class deletepizza : MonoBehaviour
{
    public GameObject currentPizza;
    public void DeletePizzaObject()
    {
        if (currentPizza != null)
        {
            Destroy(currentPizza);
            currentPizza = null;
        }
    }
}