using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MonobitEngine;

public class CursorChange :  MonobitEngine.MonoBehaviour
{

      [SerializeField]
    GameObject CursorImage;

    [SerializeField]
    GameObject canvas;

    public Texture2D RedCursor;
    
    public int pointerjudge = 0;

    public GameObject Panel;

    PixAccess PixAccess;

    private GameObject Plane;

public void Start() {
    Plane= GameObject.Find("Plane");
    PixAccess = Plane.GetComponent<PixAccess>();
}
        public void OnclickCursorButton()
    {
        /*
        if(pointerjudge == 0)
        {
            Cursor.SetCursor(RedCursor,new Vector2(RedCursor.width / 2,RedCursor.height / 2), CursorMode.Auto);
            pointerjudge = 1;
        }
        else
        {
            Cursor.SetCursor(null,Vector2.zero, CursorMode.Auto);
            pointerjudge = 0;
        }
        */
        if(pointerjudge == 0)
        {
            GameObject prefab = MonobitEngine.MonobitNetwork.Instantiate("CursorPointer", Vector3.zero, Quaternion.identity, 0);
            //prefab.transform.SetParent(canvas.transform, false);
            Debug.Log("CursorPointer複製完了");
            //Cursor.visible = false;
            pointerjudge = 1;
        }
        else
        {
             //Cursor.visible = true;
            pointerjudge = 0;
        }
    }
    
    public void OnMouseDown_()
    {
        if(PixAccess.mode){
GameObject prefab = MonobitEngine.MonobitNetwork.Instantiate("CursorPointer", Vector3.zero, Quaternion.identity, 0);
            Debug.Log("CursorPointer複製完了");
        }
    }
        
    

    public void OnPointerEnter(){
Panel.SetActive(true);
    }

    public void OnPointerExit(){
        Panel.SetActive(false);
    }
}