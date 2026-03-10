using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ToppingController : MonoBehaviour
{

    [SerializeField] private bool isDragging = false;

    void Update()
    {
    
       if(isDragging)
       {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
       }
    }

    void OnMouseDown()
    {
        isDragging = !isDragging;
    }
}
