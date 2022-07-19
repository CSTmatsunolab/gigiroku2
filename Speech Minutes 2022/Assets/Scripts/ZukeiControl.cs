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
    public GameObject DeleteButton;
    //public GameObject EnlargeButton;
    //public GameObject ShrinkButton;
    private int touchCount = 0;



    RectTransform image;
    Vector3 originPos; // 最初にクリックしたときの位置
    Vector3 originMouse;
    Quaternion position; // 最初にクリックしたときのBoxの角度
    Vector3 size;
    
    //Vector2 vecA; // Boxの中心からposへのベクトル
    Vector3 vecA; // Boxの中心から現在のマウス位置へのベクトル
    Vector3 mousediff;
    Vector3 nowPos;
    Vector3 nowMouse;
    Vector3 moveScale;

    float angle; // vecAとvecBが成す角度
    Vector3 AxB; // vecAとvecBの外積



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
            Debug.Log("Selectされました");
            scroll = Input.GetAxis("Mouse ScrollWheel");
        }
    }
    void Update()
    {
        if (Selectflag == true)
        {
            //if (!monobitView.isMine) { return; }
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
            if (Input.GetMouseButtonDown(0))
            {
                Selectflag = false;
                if (Selectflag == false)
                {
                    Debug.Log("falseですよ");

                }
                    touchCount++;
                    //0.3秒後にHogeメソッドを呼び出す
                     Invoke("DoubleclickJudg", 0.3f);
            }
            if (EventSystem.current.IsPointerOverGameObject())
            {
                DeleteButton.SetActive(true);
                dropdown.SetActive(true);   
            }
            else
            {
                DeleteButton.SetActive(false);
                dropdown.SetActive(false);
            }
        }
        
        if (Selectflag == true && Input.GetKey(KeyCode.Backspace))
        {
            OnDestroy();
            Selectflag = false;
            Debug.Log("false&destroy");
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
        monobitView.RequestOwnership();
        //if (!monobitView.isMine) { return; }
        m_rectTransform.position += new Vector3(e.delta.x, e.delta.y, 0f);
    }

    public void DeleteButtonOnclick()
    {
        OnDestroy();
        Selectflag = false;
        Debug.Log("false&destroy");
    }
    void OnDestroy()
    {
        MonobitNetwork.Destroy(monobitView);
    }





    // PointerDownで呼び出す
    // クリック時にパラメータの初期値を求める
    public void SetPos(){
        Debug.Log("クリックされた");
        image = this.GetComponent<RectTransform>();
        size = image.sizeDelta;//図形のサイズ取得
        originPos = Input.mousePosition;// マウス位置をワールド座標で取得
        // originPos.z = 1.0f;
        // originMouse = Camera.main.ScreenToWorldPoint(originPos);
        //Debug.Log(Screen.height);
        moveScale = new Vector3(Screen.width*0.0004f,Screen.height*0.0007f,0); 
        //position = transform.parent.position; // Boxの真ん中の位置を取得

    }

    // ハンドルをドラッグしている間に呼び出す
    public void Rotate(){
        Debug.Log("回そうとしてる");
        nowPos = Input.mousePosition;
        // nowPos.z = 1.0f;
        // nowMouse = Camera.main.ScreenToWorldPoint(nowPos);
        mousediff = nowPos - originPos; //ある地点からのベクトルを求めるときはこう書くんだった
        //Debug.Log(mousediff);
        if(image.sizeDelta.x >= 20 &&  image.sizeDelta.y >= 20){
            image.sizeDelta = size + new Vector3(mousediff.x/moveScale.x,mousediff.y/moveScale.y,0);
        }
        if(image.sizeDelta.x <= 20){
            image.sizeDelta = new Vector3(20f,image.sizeDelta.y,0);
        }
        if(image.sizeDelta.y <= 20){
            image.sizeDelta = new Vector3(image.sizeDelta.x,20f,0);
        }
        //image.sizeDelta = size + mousediff;
        // vecA = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.parent.position; // 上に同じく
        // // Vector2にしているのはz座標が悪さをしないようにするためです

        // angle = Vector2.Angle(vecA, vecB); // vecAとvecBが成す角度を求める
        // AxB = Vector3.Cross(vecA, vecB); // vecAとvecBの外積を求める
        //transform.parent.localScale = 

        // // 外積の z 成分の正負で回転方向を決める
        // if (AxB.z > 0)
        // {
        //     transform.parent.localRotation = rotation * Quaternion.Euler(0,0, angle); // 初期値との掛け算で相対的に回転させる
        // }
        // else{
        //     transform.parent.localRotation = rotation * Quaternion.Euler(0, 0, -angle); // 初期値との掛け算で相対的に回転させる
        // }
    }




}


