using UnityEngine;

/// <summary>
/// Holds and calculates the End of Day accuracy scores for each station.
/// Each subscore represents how closely the player matched the customer's request (0-100).
/// Total score is the sum of all three subscores (0-300).
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    // Each score represents accuracy percentage for that station (0 to 100)
    private int _toppingsScore;
    private int _bakingScore;
    private int _cuttingScore;

    public int ToppingsScore => _toppingsScore;
    public int BakingScore   => _bakingScore;
    public int CuttingScore  => _cuttingScore;

    /// <summary>Total score is the sum of all three station scores (range: 0-300).</summary>
    public int TotalScore => _toppingsScore + _bakingScore + _cuttingScore;

    void Awake()
    {
        // Simple singleton so other scripts can call ScoreManager.Instance
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Sets the accuracy score for a station. Clamps to valid range 0-100.
    /// </summary>
    public void SetToppingsScore(int score) => _toppingsScore = ClampScore(score);
    public void SetBakingScore(int score)   => _bakingScore   = ClampScore(score);
    public void SetCuttingScore(int score)  => _cuttingScore  = ClampScore(score);

    /// <summary>
    /// Resets all scores back to zero (call this at the start of each new day).
    /// </summary>
    public void ResetScores()
    {
        _toppingsScore = 0;
        _bakingScore   = 0;
        _cuttingScore  = 0;
    }

    /// <summary>
    /// Clamps a raw accuracy value to the valid score range [0, 100].
    /// Public and static so it can be tested directly without a MonoBehaviour instance.
    /// </summary>
    public static int ClampScore(int rawScore)
    {
        return Mathf.Clamp(rawScore, 0, 100);
    }

    /// <summary>
    /// Calculates a total from three explicit values without needing an instance.
    /// Useful for unit tests and one-off calculations.
    /// </summary>
    public static int CalculateTotal(int toppings, int baking, int cutting)
    {
        return ClampScore(toppings) + ClampScore(baking) + ClampScore(cutting);
    }
}
