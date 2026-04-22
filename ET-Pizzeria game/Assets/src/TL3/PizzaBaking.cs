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
    private Pizza pizza;
    private AudioSource audioSource;
    
    void Start()
    {
        pizza = GetComponent<Pizza>();
        audioSource = FindObjectOfType<AudioSource>();
    }
    // Update increments the bake timer while pizza is in the oven AND is not burnt. It checks if it is burnt and stops music. Otherwise you get a cooked pizza
    void Update()
    {
        if (!isInOven || isBurnt) return;

        currentBakeTime += Time.deltaTime;

        if (pizza != null)
        {
            pizza.bakingTime += Time.deltaTime;
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