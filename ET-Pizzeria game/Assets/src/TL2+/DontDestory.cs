using UnityEngine;

/*
fairly sure this code will not have use :( for what we are doing.
*/


public class DontDestory : MonoBehaviour
{
    private static GameObject[] persistentObjects = new GameObject[3];
    public int objectIndex;

    void Awake()
    {
        if(persistentObjects[objectIndex] == null)
        {
            persistentObjects[objectIndex] = gameObject;
            //DontDestoryOnLoad(gameObject);
        }
        else if (persistentObjects[objectIndex] != gameObject)
        {
            //Destory(gameObject);
        }
    }

}
