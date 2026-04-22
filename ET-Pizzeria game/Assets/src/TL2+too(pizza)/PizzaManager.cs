using UnityEngine;
using UnityEngine.SceneManagement; 

public class PizzaManager : MonoBehaviour
{
    [SerializeField] private GameObject pizzaPrefab;
    private GameObject currentPizza;
    private bool isSpawning = false;

    public void SpawnPizza()
    {
        Debug.Log("Spawn button pressed");
        Scene scene = SceneManager.GetActiveScene();

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
        if(scene.name == "CuttingScreen")
        {
            currentPizza.transform.position = new Vector2(0, 0); 
        }

        if(scene.name == "BakingScreen")
        {
            currentPizza.transform.position = new Vector2(0, 0);
        }

        if(scene.name == "ToppingScreen")
        {
            currentPizza.transform.position = new Vector2(-1.75f, -0.5f);
        } 
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

    public void SUbmitPizza()
    {
        Pizza p = currentPizza.GetComponent<Pizza>();
        PizzaGameData.SetPizza(p.GetIngredients(), p.GetBakeTime(), p.GetCut()); //added by Noah for scoring
        SceneManager.LoadScene("EndDayScene");
    }
}