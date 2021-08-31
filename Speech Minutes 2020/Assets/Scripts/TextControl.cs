﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class TextControl : MonoBehaviour, IDragHandler
{
    // マウススクロール変数
    private float scroll;
    public Text chatComent;
    public bool Selectflag = false;
   // public Color texcolor;
    public GameObject teO;
    public GameObject EdittingTextPanel;
    public InputField EdittingTextField;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        // Textコンポーネントを取得
        Text text = this.GetComponentInChildren<Text>();
        // 色を指定
        text.color = Color.black;
        EdittingTextPanel.SetActive(false);
    }
    /// <summary>
    /// テキストコメントを選択するための関数
    /// </summary>
    public void Selecter()
    {
     /*  if (Input.GetMouseButtonDown(0) && Cursor.lockState != CursorLockMode.Locked)
            {
                Debug.Log("lockされてない");
            Selectflag = true;
            // OnClick();  //クリックされた時の処理
        }*/
        Selectflag = true;
        if (Selectflag == true)
        {
            // Textコンポーネントを取得
               chatComent = this.GetComponentInChildren<Text>();
               // 色を指定
               chatComent.color = Color.white;
               Debug.Log("Selectされました");
            scroll = Input.GetAxis("Mouse ScrollWheel");
          /*  if (scroll > 0)
            {
                text.fontSize += 14;
                Debug.Log("回ったよ");
            }*/
            


        }
       /* EventSystem ev = EventSystem.current;
        if (ev.alreadySelecting)
        {
            Debug.Log("何かを選択しています");
        }*/
    }
    /// <summary>
    /// テキストのフォントサイズ変更及び削除
    /// </summary>
    void Update()
    {
        /* if (GameObject.Find("TextBox").GetComponent<InputField>().text != "")
         {
             Selectflag == false;
         }*/
        //Start();
        if (Selectflag == true)
        {
            scroll = Input.GetAxis("Mouse ScrollWheel");
            Text textfont = this.GetComponentInChildren<Text>();

            if (scroll > 0)
            {
                textfont.fontSize += 1;// (int)scroll*100;
                Debug.Log("大よ"+scroll);
            }else if (scroll < 0)
            {
                if (textfont.fontSize >= 10)
                {
                    textfont.fontSize -= 1;// (int)scroll*100;
                    Debug.Log("小よ" + scroll);
                }
                else { textfont.fontSize = 10; }
            }

            if (Input.GetMouseButtonDown(0))
                 {
                      Selectflag = false;
                if (Selectflag == false)
                {
                    // Textコンポーネントを取得
                    Text text = this.GetComponentInChildren<Text>();
                    // 色を指定
                    text.color = Color.black;

                    Debug.Log("falseですよ");
                }

                 }

        }
        if(Selectflag==true && Input.GetKey(KeyCode.Backspace))
        {
            Destroy(this.gameObject);
            Selectflag = false;
            Debug.Log("false&destroy");
        }
        if (Selectflag == true && Input.GetKey(KeyCode.Space))
        {
            //inputFieldのTextコンポーネントを取得
            EdittingTextField = EdittingTextField.GetComponent<InputField>();
            // Textコンポーネントを取得
            Text text = this.GetComponentInChildren<Text>();
            EdittingTextField.text =text.text;
            Selectflag = false;
            EdittingTextPanel.SetActive(true);
        }
        if (Selectflag == true && Input.GetKey(KeyCode.Return) && EdittingTextPanel.activeSelf==true)
        {
            Text text = this.GetComponentInChildren<Text>();
            text.text = EdittingTextField.text;
            EdittingTextField.text = "";
            Selectflag = false;
            EdittingTextPanel.SetActive(false);
            Debug.Log("editing text");
        }



        if (Input.GetMouseButtonDown(1))
            Debug.Log("Pressed secondary button.１");

        if (Input.GetMouseButtonDown(2))
            Debug.Log("Pressed middle click.ｑ２");
    }

    public RectTransform m_rectTransform = null;

    /// <summary>
    /// テキストの位置取得
    /// </summary>
    private void Reset()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }
    /// <summary>
    /// テキストの位置操作
    /// </summary>
    /// <param name="e"></param>
    public void OnDrag(PointerEventData e)
    {
        m_rectTransform.position += new Vector3(e.delta.x, e.delta.y, 0f);
    }

    /*//テキストボックスの削除(バックスペースで削除)
    public void Destroy()
    {
        if (Input.GetKey(KeyCode.Backspace)) {
            Destroy(this.gameObject);
            Debug.Log("ok");
        }
    }*/
}
