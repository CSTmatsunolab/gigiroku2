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

public void Start() {

}
        public void OnclickPointerButton_Always()
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
            pointerjudge = 1;
            Debug.Log(pointerjudge);
            GameObject prefab = MonobitEngine.MonobitNetwork.Instantiate("CursorPointer", Vector3.zero, Quaternion.identity, 0);
            //prefab.transform.SetParent(canvas.transform, false);
            Debug.Log("CursorPointer複製完了");
            //Cursor.visible = false;
            
            
        }
        else
        {
             //Cursor.visible = true;
             if(pointerjudge!=2)
            pointerjudge = 0;
            Debug.Log(pointerjudge);
        }
    }
         public void OnclickPointerButton_Only()
    {
        if(pointerjudge!=2){
        pointerjudge=2;
        Debug.Log(pointerjudge);
        }else{
            pointerjudge = 0;
            Debug.Log(pointerjudge);
        }
    }
    

    public void OnPointerEnter(){
Panel.SetActive(true);
    }

    public void OnPointerExit(){
        Panel.SetActive(false);
    }
}