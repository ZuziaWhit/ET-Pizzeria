// This shows singleton pattern in GameManager because it creates a static instance and ensures that only one object exists in the scene.

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isGameRunning = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartBaking()
    {
        Debug.Log("GameManager: Baking started");
    }
}