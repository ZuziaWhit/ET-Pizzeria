using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string sceneName = "ToppingScreen";

    public void StartGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}