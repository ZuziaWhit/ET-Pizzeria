using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GUIVisibility : MonoBehaviour
{
    private Renderer[] renderers;
    private GameObject[] children;

    //[Header("Scenes where this object is visible")]
    private List<string> visibleScenes = new List<string>
    {
        "GUI",
        "CuttingScreen",
        "ToppingScreen",
        "OrderScene",
        "BakingScreen"
    };

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        renderers = GetComponentsInChildren<Renderer>(true);

        children = new GameObject[transform.childCount];
        for (int i = 0; i < children.Length; i++)
        {
            children[i] = transform.GetChild(i).gameObject;
        }
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
        UpdateVisibility(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateVisibility(scene.name);
    }

    void UpdateVisibility(string sceneName)
    {
        bool visible = visibleScenes.Contains(sceneName);

        foreach (Renderer r in renderers)
        {
            r.enabled = visible;
        }

        foreach (GameObject child in children)
        {
            child.SetActive(visible);
        }
    }
}