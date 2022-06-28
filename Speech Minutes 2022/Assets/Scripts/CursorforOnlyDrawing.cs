using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MonobitEngine;

public class CursorforOnlyDrawing :  MonobitEngine.MonoBehaviour
{
    
    public int pointerjudge = 0;

    PixAccess PixAccess;

    public GameObject Plane;

    GameObject Pointer;

    CursorChange CursorChange;

    GameObject CursorPointer;

public void Start() {
    Plane= GameObject.Find("Plane");
    PixAccess = Plane.GetComponent<PixAccess>();
    Pointer = GameObject.Find("Pointer");
    CursorChange = Pointer.GetComponent<CursorChange>();
}
    
    public void OnMouseDown()
    {
        if(PixAccess.mode&&CursorChange.pointerjudge==2){
GameObject prefab = MonobitEngine.MonobitNetwork.Instantiate("CursorPointer", Vector3.zero, Quaternion.identity, 0);
            Debug.Log("CursorPointer複製完了");
            CursorChange.pointerjudge=3;
        }
    }
     public void OnMouseUp() {
        if(PixAccess.mode&&CursorChange.pointerjudge!=1){
        CursorChange.pointerjudge=4;
    }
     }

}