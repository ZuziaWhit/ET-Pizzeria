using UnityEngine;
using UnityEngine.UI;

public class BakeProgressUI : MonoBehaviour
{
    public PizzaBaking baking;
    public Slider slider;
    public Image fill;

    void Start()
    {
        slider.maxValue = baking.burntTime;
    }

    void Update()
    {
        slider.value = baking.currentBakeTime;

        // RAW
        if (!baking.isCooked && !baking.isBurnt)
        {
            fill.color = Color.red;
        }

        // PERFECT
        else if (baking.isCooked && !baking.isBurnt)
        {
            fill.color = Color.green;
        }

        // BURNT
        else if (baking.isBurnt)
        {
            fill.color = Color.black;
        }
    }
}