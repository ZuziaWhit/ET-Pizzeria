using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance;

    public GameObject pauseMenu;
    public GameObject settingsMenu;

    private bool isPaused = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupPersistentMenus();
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

        if (settingsMenu != null)
            settingsMenu.SetActive(false);
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

    private void SetupPersistentMenus()
    {
        if (pauseMenu == null)
        {
            PauseMenuUI foundPauseMenu = FindFirstObjectByType<PauseMenuUI>(FindObjectsInactive.Include);
            if (foundPauseMenu != null)
                pauseMenu = foundPauseMenu.gameObject;
        }

        if (settingsMenu == null)
        {
            SettingsMenu foundSettingsMenu = FindFirstObjectByType<SettingsMenu>(FindObjectsInactive.Include);
            if (foundSettingsMenu != null)
                settingsMenu = foundSettingsMenu.gameObject;
        }

        if (pauseMenu == null && settingsMenu == null)
            return;

        GameObject menuCanvas = new GameObject("Persistent Menu Canvas");
        DontDestroyOnLoad(menuCanvas);

        Canvas canvas = menuCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;

        menuCanvas.AddComponent<CanvasScaler>();
        menuCanvas.AddComponent<GraphicRaycaster>();

        MoveMenuToCanvas(pauseMenu, menuCanvas.transform);
        MoveMenuToCanvas(settingsMenu, menuCanvas.transform);
        LinkMenuReferences();
    }

    private void MoveMenuToCanvas(GameObject menu, Transform canvasTransform)
    {
        if (menu == null)
            return;

        menu.transform.SetParent(canvasTransform, false);
    }

    private void LinkMenuReferences()
    {
        if (pauseMenu != null)
        {
            PauseMenuUI pauseMenuUI = pauseMenu.GetComponent<PauseMenuUI>();
            if (pauseMenuUI != null)
                pauseMenuUI.settingsMenu = settingsMenu;
        }

        if (settingsMenu != null)
        {
            SettingsMenu settings = settingsMenu.GetComponent<SettingsMenu>();
            if (settings != null)
                settings.pauseMenu = pauseMenu;
        }
    }
}
