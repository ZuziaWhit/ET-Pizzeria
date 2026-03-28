using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ToppingController : MonoBehaviour
{
    /* ============================
       1. DRAGGING (Your Original Code)
       ============================ */
    [SerializeField] private bool isDragging = false;

    void Update()
    {
        if (isDragging)
        {
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    void OnMouseDown()
    {
        isDragging = !isDragging;
    }



    /* ============================
       2. SCORING SYSTEM (0–100 Clamp)
       ============================ */

    private int score = 0;

    /// <summary>
    /// Sets the score and clamps it between 0 and 100.
    /// </summary>
    public int SetScore(int value)
    {
        score = Mathf.Clamp(value, 0, 100);
        return score;
    }

    public int GetScore()
    {
        return score;
    }



    /* ============================
       3. TOPPING LIMIT SYSTEM (Max 20)
       ============================ */

    public int maxToppings = 20;
    private int currentToppings = 0;

    /// <summary>
    /// Attempts to add a topping. Returns true if allowed, false if limit reached.
    /// </summary>
    public bool TryAddTopping()
    {
        if (currentToppings >= maxToppings)
            return false;

        currentToppings++;
        return true;
    }

    public int GetToppingCount()
    {
        return currentToppings;
    }



    /* ============================
       4. PEPPERONI SPAWNER (Stress Test)
       ============================ */

    [Header("Pepperoni Spawner")]
    public GameObject pepperoniPrefab;
    public Transform spawnPoint;

    /// <summary>
    /// Spawns a pepperoni at the spawn point.
    /// </summary>
    public GameObject SpawnPepperoni()
    {
        if (pepperoniPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("Spawner missing prefab or spawn point.");
            return null;
        }

        return Instantiate(pepperoniPrefab, spawnPoint.position, Quaternion.identity);
    }
}

