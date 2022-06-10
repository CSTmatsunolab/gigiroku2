using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using MonobitEngine;

public class ZukeiControl : MonobitEngine.MonoBehaviour, IDragHandler
{
    // マウススクロール変数
    private float scroll;
    public Text chatComent;
    public bool Selectflag = false;
    public GameObject teO;
    public GameObject dropdown;
    private int touchCount = 0;
    

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    //付箋のテキスト内容を反映させるメソッド
    [MunRPC]
    public void RecvChattext(string text_)
    {
        Text text = this.GetComponentInChildren<Text>();
        text.text = text_;
        Debug.Log("receiveChattext");
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
            Debug.Log("Selectされました");
            scroll = Input.GetAxis("Mouse ScrollWheel");
        }
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
            if (!monobitView.isMine) { return; }
            scroll = Input.GetAxis("Mouse ScrollWheel");
            Text textfont = this.GetComponentInChildren<Text>();

            if (scroll > 0)
            {
                textfont.fontSize += 1;// (int)scroll*100;
                Debug.Log("大よ" + scroll);
            }
            else if (scroll < 0)
            {
                if (textfont.fontSize >= 32)
                {
                    textfont.fontSize -= 1;// (int)scroll*100;
                    Debug.Log("小よ" + scroll);
                }
                else { textfont.fontSize = 32; }
            }
        }
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
        if (!monobitView.isMine) { return; }
        m_rectTransform.position += new Vector3(e.delta.x, e.delta.y, 0f);
    }

}


