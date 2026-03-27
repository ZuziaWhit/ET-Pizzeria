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

    void Update()
    {
        if (isInOven && !isBurnt)
        {
            currentBakeTime += Time.deltaTime;
            Debug.Log("Bake Time: " + currentBakeTime);

            if (currentBakeTime >= burntTime)
            {
                isBurnt = true;
                isCooked = false;
                Debug.Log("Pizza Burnt!");
            }
            else if (currentBakeTime >= cookedTime)
            {
                isCooked = true;
                Debug.Log("Pizza Cooked!");
            }
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

    if (!canBake) {

    Debug.Log("Cannot Bake Here");
    return;
        }
    isInOven = true;
    currentBakeTime = 0f;
    isCooked = false;
    isBurnt = false;
    }
}
