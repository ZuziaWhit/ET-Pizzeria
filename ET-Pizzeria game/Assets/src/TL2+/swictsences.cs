using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;

public class swictsences : MonoBehaviour
{
    public void LoadSpecificScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        SceneManager.LoadScene("GUI", LoadSceneMode.Additive);
    }
}
