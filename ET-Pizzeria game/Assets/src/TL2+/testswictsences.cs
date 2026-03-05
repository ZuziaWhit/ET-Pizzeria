using UnityEngine;
using UnityEngine.SceneManagement; // Required for SceneManager
using UnityEngine.UI; // Required for UI elements

public class testswictsences : MonoBehaviour
{
    // Public function to load a scene by name
    public void LoadSpecificScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Optional: Public function to load a scene by index
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
