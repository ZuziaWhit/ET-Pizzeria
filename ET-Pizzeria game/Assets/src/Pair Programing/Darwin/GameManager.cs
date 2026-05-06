using UnityEngine;
using UnityEngine.SceneManagement;

// This class acts as the central hub for the entire game.
// It uses the Singleton pattern and DontDestroyOnLoad to survive scene changes.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Global Game Stats")]
    public int totalMoney = 0;
    public int pizzasCompleted = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            Debug.Log("GameManager initialized.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ─── SCENE NAVIGATION PIPELINE ──────────────────────────────────────────

    public void StartNewDay()
    {
        totalMoney = 0;
        pizzasCompleted = 0;
        StartNewOrder();
    }

    public void StartNewOrder()
    {
        PizzaGameData.Clear(); 
        SceneManager.LoadScene("OrderScene");
    }

    public void GoToTopping() => SceneManager.LoadScene("ToppingScreen");
    public void GoToBaking() => SceneManager.LoadScene("BakingScreen");
    public void GoToCutting() => SceneManager.LoadScene("CuttingScreen");

    // ─── SCORING INTEGRATION ────────────────────────────────────────────────

    public void SubmitPizzaAndEndDay()
    {
        if (PizzaGameData.IsReady)
        {
            PizzaScorer.ScoreResult result = PizzaScorer.ScoreFromGameData();
            if (result != null)
            {
                totalMoney += (result.Stars * 15);
                pizzasCompleted++;
            }
        }
        
        SceneManager.LoadScene("EndDayScene");
    }
}