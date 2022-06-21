using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MonobitEngine;

public class CursorControll : MonobitEngine.MonoBehaviour
{
     private GameObject ChangeRedCursor;
    CursorChange CursorChange;
 Vector3 mousePos ;

   void Start() {
        if (monobitView.isMine){
    mousePos=Input.mousePosition;

     ChangeRedCursor = GameObject.Find("ChangeRedCursor");
    CursorChange = ChangeRedCursor.GetComponent<CursorChange>();
        }
  }
    // Update is called once per frame
    void Update () {
        if (monobitView.isMine){
        if(CursorChange.pointerjudge ==0){
            OnDestroy();
            Debug.Log("ポインタ削除済み");
        }
        
         mousePos = Input.mousePosition;
            this.transform.position=mousePos;
        }
           
    }
        void OnDestroy()
    {
        MonobitNetwork.Destroy(monobitView);
    }
}

