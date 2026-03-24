using UnityEngine;

public class PizzaBaking : MonoBehaviour
{
    public float currentBakeTime = 0f;
    public float cookedTime = 10f;
    public float burntTime = 20f;

    public bool isInOven = false;
    public bool isCooked = false;
    public bool isBurnt = false;

    void Update()
    {
        if (isInOven && !isBurnt)
        {
            currentBakeTime += Time.deltaTime;

            if (currentBakeTime >= burntTime)
            {
                isBurnt = true;
                isCooked = false;
            }
            else if (currentBakeTime >= cookedTime)
            {
                isCooked = true;
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
}
