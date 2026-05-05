using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ToppingController : MonoBehaviour
{
    /* ============================
       1. Placing Topping Clone
       ============================ */
    [SerializeField] private bool isDragging = false;
    GameObject pepperoni_clone;
    public GameObject toppingPrefab;

    //need to figure it out how I referenced the coin stuff. Need to access the getmepizza
    GameObject currentPizza = PizzaManager.getmepizza();




    private Collider2D myCollider; //reference to the topping collider

    public Transform pizzaCenter;
    public float outerRadius = 1.8f;
    public float innerRadius = 0.9f;

    private List<Vector2> toppingSlots = new List<Vector2>();

    public ToppingManager manager;
    

    void Start()
    {
        

     GenerateToppingSlots();
     myCollider = GetComponent<Collider2D>();



    }

    void Awake()//Thread Safe bc Atomic Operation...Unity locks for you
    {
        manager = ToppingManager.GetInstance();  //*******use static singleton
        Debug.Log(manager.CurrentCount);
    }


////Function for creating the Topping Slots
    void GenerateToppingSlots()
    {
        toppingSlots.Clear();

        // 8 outer slots
        int outerCount = 8;
        for (int i = 0; i < outerCount; i++)
        {
            float angle = (Mathf.PI * 2f / outerCount) * i;
            Vector2 pos = (Vector2)pizzaCenter.position +
                        new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * outerRadius;
            toppingSlots.Add(pos);
        }

        // 3 inner slots (e.g., spaced 120 degrees apart)
        int innerCount = 3;
        for (int i = 0; i < innerCount; i++)
        {
            float angle = (Mathf.PI * 2f / innerCount) * i;
            Vector2 pos = (Vector2)pizzaCenter.position +
                        new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * innerRadius;
            toppingSlots.Add(pos);
        }
    }

    Vector2 SnapToNearestSlot(Vector2 pos)
    {
        Vector2 best = pos;
        float bestDist = float.MaxValue;

        foreach (Vector2 slot in toppingSlots)
        {
            float dist = Vector2.Distance(pos, slot);
            if (dist < bestDist)
            {
                bestDist = dist;
                best = slot;

            
            }
        }

        return best;
    }

    void Update()
    {
        if (isDragging)
        {    
            pepperoni_clone.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

/////Fuction to Make the Topping Snap to a Fixed Grid
    private Vector2 SnapToGrid(Vector2 pos, float gridSize = 0.5f)
    {
        float x = Mathf.Round(pos.x / gridSize) * gridSize;
        float y = Mathf.Round(pos.y / gridSize) * gridSize;
        return new Vector2(x, y);
    }

    void OnMouseDown()
    {
        ///Create Clone
        pepperoni_clone = Instantiate(toppingPrefab);
        pepperoni_clone.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pepperoni_clone.transform.rotation = Quaternion.identity;
        ////

        myCollider.enabled = false;
        isDragging = true;


    }


    void OnMouseUp()
    {
        isDragging = false;
        myCollider.enabled = true;

        

        if (pepperoni_clone != null)
        {
            //Get the read data... is this okay?????????
            Topping topping = pepperoni_clone.GetComponent<Topping>(); 

            ToppingData new_topping = new CustomTopping(topping.ToppingName, topping.CookTime, topping.ScoreValue); ////**subclass 

            Debug.Log(new_topping.strGetName()); //call overridden version

            if (manager.CanPlaceTopping())
            {
                Debug.Log("Enter manager!!");
                Vector2 snapped = SnapToNearestSlot(pepperoni_clone.transform.position);
                pepperoni_clone.transform.position = snapped;

                topping.OnPlaced(snapped);

                // Tell the manager we placed one
                manager.RegisterToppingPlaced();

                //currentPizza = PizzaManager.getmepizza();

                topping.transform.SetParent(pizza.transform, true);



               // Debug.Log($"Placed {new_topping.strGetName()} | CookTime: {new_topping.CookTime} | Score: {new_topping.ScoreValue}");
            }
            else
            {
                // Too many toppings — destroy the clone
                Destroy(pepperoni_clone);
            }
        }
    }















   //  ============================
   //    2. SCORING SYSTEM (0–100 Clamp)
   //   ============================ 

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



   // ============================
    //   3. TOPPING LIMIT SYSTEM (Max 20)
    //   ============================ 

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



   //  ============================
   //    4. PEPPERONI SPAWNER (Stress Test)
   //    ============================ 

    [Header("Pepperoni Spawner")]
    public Transform spawnPoint;

    /// <summary>
    /// Spawns a pepperoni at the spawn point.
    /// </summary>
    public GameObject SpawnPepperoni()
    {
        if (toppingPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("Spawner missing prefab or spawn point.");
            return null;
        }

        return Instantiate(toppingPrefab, spawnPoint.position, Quaternion.identity);
    }
}

