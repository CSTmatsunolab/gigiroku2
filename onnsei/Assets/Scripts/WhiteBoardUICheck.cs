using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MonobitEngine;

public class WhiteBoardUICheck : MonobitEngine.MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    GameObject Plane;

    PixAccess PixAccess;
    // Start is called before the first frame update
    void Start()
    {
        Plane = GameObject.Find("Plane");
        PixAccess = Plane.GetComponent<PixAccess>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        PixAccess.targetUI = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        PixAccess.targetUI = false;
    }
}
