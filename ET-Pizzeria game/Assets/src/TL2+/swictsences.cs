using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;

public class swictsences : MonoBehaviour
{
    public void LoadSpecificScene(string sceneName)
    {
        if(sceneName == "EndDayScene"){
            SceneManager.LoadScene(sceneName);
        }else{
            SceneManager.LoadScene(sceneName);
        }

    }
}
