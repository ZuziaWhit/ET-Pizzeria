using UnityEngine;

public class PizzaBaking : MonoBehaviour
{
    public float currentBakeTime = 0f;
    public float cookedTime = 10f;
    public float burntTime = 20f;

    public bool isInOven = false;
    public bool isCooked = false;
    public bool isBurnt = false;

    public bool canBake = false;

    private Pizza pizza;
    private AudioSource audioSource;

    void Start()
    {
        pizza = GetComponent<Pizza>();
        audioSource = FindObjectOfType<AudioSource>();
    }

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