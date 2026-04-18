using System.Collections;
using UnityEngine;
using TMPro;

public class PizzaOrderPrefab : MonoBehaviour
{
    public TMP_Text toppingsText;
    public TMP_Text timeText;
    public TMP_Text cutText;

    private PizzaOrderData orderData;

    public void Initialize(PizzaOrderData data)
    {
        orderData = data;
        StartCoroutine(DisplayOrderRoutine());
    }

    IEnumerator DisplayOrderRoutine()
    {
        toppingsText.text = "";
        timeText.text = "";
        cutText.text = "";

        foreach (string ingredient in orderData.ingredients)
        {
            toppingsText.text += ingredient + "\n";
            yield return new WaitForSeconds(1.5f);
        }

        timeText.text = "Bake Time: " + orderData.bakeTime;
        yield return new WaitForSeconds(1.5f);

        cutText.text = "Cut Type: " + orderData.cutType;
    }
}