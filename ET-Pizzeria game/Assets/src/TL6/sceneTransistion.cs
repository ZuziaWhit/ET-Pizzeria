using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneTransistion : MonoBehaviour
{
    public void NextLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
