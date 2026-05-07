using UnityEngine;
using UnityEngine.SceneManagement; //samnEmily
// This class allows the user to drag the pizza
public class DragPizza : MonoBehaviour
{

    private Vector3 offset;
    private float zCoord;

    void OnMouseDown()
    {
        //Debug.Log("nMouseDown");
        Scene scene = SceneManager.GetActiveScene();//samnEmily
        //Debug.Log(scene.name);
        if(scene.name == "BakingScreen") //samnEmily
        {
            zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
            offset = transform.position - GetMouseWorldPosition();
        }
    }

    void OnMouseDrag()
    {
        Scene scene = SceneManager.GetActiveScene();//samnEmily        
        //Debug.Log(scene.name);
        if(scene.name == "BakingScreen") //samnEmily
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
