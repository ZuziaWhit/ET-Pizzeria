using UnityEngine;
using UnityEngine.UI;
// This class fills the bake timer bar at the top and sets it to different colors. 
public class BakeProgressUI : MonoBehaviour
{
    public PizzaBaking baking;
    public Slider slider;
    public Image fill;

    void Start()
    {
        slider.maxValue = baking.burntTime;
        slider.minValue = 0f;
    }

   void Update()
    {
    // Update slider value
    slider.value = baking.currentBakeTime;

    float percent = baking.currentBakeTime / baking.burntTime;

    // Smooth color transition: green → yellow → red
    if (percent < 0.5f)
        {
        fill.color = Color.Lerp(Color.green, Color.yellow, percent * 2f);
        }
    else
     {
        fill.color = Color.Lerp(Color.yellow, Color.red, (percent - 0.5f) * 2f);
     }

    // Optional: override if burnt (force dark red/black)
    if (baking.isBurnt)
        {
        fill.color = new Color(0.3f, 0f, 0f); // dark burnt red
        }
    }
}