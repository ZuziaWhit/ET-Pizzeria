using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance;

    public GameObject pauseMenu;

    private bool isPaused = false;

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

    void Start()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        if (pauseMenu != null)
            pauseMenu.SetActive(true);

        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        isPaused = false;
    }
}