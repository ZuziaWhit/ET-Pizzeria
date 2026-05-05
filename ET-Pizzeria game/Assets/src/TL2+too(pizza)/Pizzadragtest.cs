using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Pizzadrag : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector3 offset;
    private float zCoord;
    private Camera cam;
    private bool dragging = false;

    void Start()
    {
        cam = Camera.main;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += (scene, mode) => cam = Camera.main;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("nMouseDown");
        if (SceneManager.GetActiveScene().name != "BakingScreen") return;

        dragging = true;

        zCoord = cam.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("drag");
        if (!dragging) return;
        if (SceneManager.GetActiveScene().name != "BakingScreen") return;

        transform.position = GetMouseWorldPosition(eventData) + offset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("up");
        dragging = false;
    }

    Vector3 GetMouseWorldPosition(PointerEventData eventData)
    {
        Vector3 mousePoint = eventData.position;
        mousePoint.z = zCoord;
        return cam.ScreenToWorldPoint(mousePoint);
    }
}