using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MonobitEngine;

public class CursorControll : MonobitEngine.MonoBehaviour
{
    GameObject Pointer;
    CursorChange CursorChange;
    Vector3 mousePos;
    public Text Text;


    void Start()
    {
        Pointer = GameObject.Find("Pointer");
        CursorChange = Pointer.GetComponent<CursorChange>();
        // if (monobitView.isMine){
        mousePos = Input.mousePosition;
        Text.text = "";
        //自分のユーザーネーム取得ののち送信
        Text.text = MonobitEngine.MonobitNetwork.player.name;
        string text_ = Text.text;
        monobitView.RPC("UsernameForPointer", MonobitTargets.OthersBuffered, text_);
        //}
    }
    [MunRPC]
    public void UsernameForPointer(string text_)
    {
        Text.text = text_;
    }

    // Update is called once per frame
    void Update()
    {
        /* if(CursorChange.pointerjudge ==0){
             OnDestroy();
             Debug.Log("ポインタ削除済み");
         }else if(CursorChange.pointerjudge ==4){
                 CursorChange.pointerjudge=2;
                 OnDestroy();
                 Debug.Log("ポインタ削除済み");

         }*/
        if (CursorChange.pointerjudge == 1)
        {
            OnDestroy();
        }
        mousePos = Input.mousePosition;
        this.transform.position = mousePos;
        //Debug.Log(mousePos);
    }


    public void OnDestroy()
    {
        MonobitNetwork.Destroy(monobitView);
    }
}