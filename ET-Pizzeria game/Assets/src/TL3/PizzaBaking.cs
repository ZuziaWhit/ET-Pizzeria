using UnityEngine;
// This is the main class for baking logic. This class determins the amount of time a pizza has been baking for and sets pizza state. 
// This also begins audio and makes debug logs for easier bug hunting.
public class PizzaBaking : MonoBehaviour
{
    public float currentBakeTime = 0f;
    public float cookedTime = 10f;
    public float burntTime = 20f;

    public bool isInOven = false;
    public bool isCooked = false;
    public bool isBurnt = false;

    public bool canBake = false;

    // Private class data pattern here because I have private fields for pizzabaking
    private Food food;
    private AudioSource audioSource;
    
    void Start()
    {
        food = GetComponent<Food>();
        audioSource = FindObjectOfType<AudioSource>();
    }
    // Update increments the bake timer while pizza is in the oven AND is not burnt. It checks if it is burnt and stops music. Otherwise you get a cooked pizza
    void Update()
    {
        if (!isInOven || isBurnt) return;

        currentBakeTime += Time.deltaTime;

        if (food != null)
        {
            food.bakingTime += Time.deltaTime;
            food.Bake(); // The dynamic binding. If it is a pizza then runs Pizza.Bake or if it is another food like flatbread then we could do Flatbread.bake. Behavior will be defined at runtime
        }

        Debug.Log("Bake Time: " + currentBakeTime);

        if (currentBakeTime >= burntTime)
        {
            isBurnt = true;
            audioSource.Stop();
            isCooked = false;
            Debug.Log("Pizza Burnt!");
        }
        else if (currentBakeTime >= cookedTime)
        {
            isCooked = true;
            Debug.Log("Pizza Cooked!");
        }
    }

    public void SetBakeTime(float time)
    {
        currentBakeTime = time;

        if (currentBakeTime >= burntTime)
        {
            isBurnt = true;
            isCooked = false;
        }
        else if (currentBakeTime >= cookedTime)
        {
            isCooked = true;
            isBurnt = false;
        }
        else
        {
            isCooked = false;
            isBurnt = false;
        }
    }

    public void StartBaking()
    {
        GameManager.Instance.StartBaking();
        Debug.Log("Bake Button Pressed!");

        if (!canBake || isBurnt)
        {
            Debug.Log("Cannot Bake Here");
            return;
        }

        isInOven = true;
        currentBakeTime = 0f;

        if (pizza != null)
            pizza.bakingTime = 0f;

        isCooked = false;
        isBurnt = false;

        audioSource.Play();
    }

    public void StopBakingAudio()
    {
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();
    }
}