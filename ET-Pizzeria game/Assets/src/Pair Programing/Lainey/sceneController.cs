using UnityEngine;
using UnityEngine.SceneManagement;
public class sceneController : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
