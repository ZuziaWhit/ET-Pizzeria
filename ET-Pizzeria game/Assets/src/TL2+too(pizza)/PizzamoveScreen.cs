using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneVisibility : MonoBehaviour
{
    Renderer[] renderers;

    void Awake()
    {
        if (FindObjectsByType<SceneVisibility>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        renderers = GetComponentsInChildren<Renderer>(true);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool visible =
            scene.name == "GUI" ||
            scene.name == "CuttingScreen" ||
            scene.name == "ToppingScreen" ||
            scene.name == "BakingScreen";

        if(scene.name == "CuttingScreen")
        {
            gameObject.transform.position = new Vector2(0, 0); 
        }

        if(scene.name == "BakingScreen")
        {
            gameObject.transform.position = new Vector2(0, 0); 
        }

        if(scene.name == "ToppingScreen")
        {
            gameObject.transform.position = new Vector2(-1.75f, -0.5f); 
        }

        foreach (Renderer r in renderers)
        {
            r.enabled = visible;
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(visible);
        }
    }
}