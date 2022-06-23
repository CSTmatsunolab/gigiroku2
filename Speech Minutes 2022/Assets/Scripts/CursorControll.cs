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
 public Text Text;

   void Start() {
        if (monobitView.isMine){
    mousePos=Input.mousePosition;

     ChangeRedCursor = GameObject.Find("ChangeRedCursor");
    CursorChange = ChangeRedCursor.GetComponent<CursorChange>();
    Text.text="";
        //自分のユーザーネーム取得ののち送信
        Text.text=MonobitEngine.MonobitNetwork.player.name;
        string text_ = Text.text;
        monobitView.RPC("UsernameForPointer", MonobitTargets.OthersBuffered, text_);
        }
        
  }

   [MunRPC]
    public void UsernameForPointer(string text_)
    {
        Text.text = text_;
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

