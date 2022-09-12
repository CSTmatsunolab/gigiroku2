
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using UnityEngine.SceneManagement;
using MonobitEngine.VoiceChat;
using UnityEngine.UI;
using System.Linq;
public class WhiteBoardparameterSend : MonobitEngine.MonoBehaviour
{

    /// <summary>
    /// 描く線のマテリアル
    /// </summary>
    public Material lineMaterial;
    /// <summary>
    /// 描く線の色
    /// </summary>
    public Color pencolor;
    /// <summary>
    /// 描く線の太さ
    /// </summary>
    public float lineWidth = 1.0f;
    LineRenderer lineRenderer;

    //public int index_;
    public Vector3 Mpos;
    freehand freehand;
    List<Vector3> pos_list = new List<Vector3>();
    GameObject NewWhiteBoard;
    //int positioncount;

    int i = 0;


    void Start()
    {
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        NewWhiteBoard = GameObject.Find("NewWhiteBoard_");
        freehand = NewWhiteBoard.GetComponent<freehand>();
    }

    void Update()
    {
        if (i == 0)
        {
            if (monobitView.isMine)
            {
                if (freehand.penmode)
                {
                    if (Input.GetMouseButton(0) && freehand.lineRendererList.Last().gameObject == this.gameObject)
                    {
                        AddPositionData();
                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        Vector3[] pos_Array = pos_list.ToArray();
                        monobitView.RPC("linecreat", MonobitTargets.OthersBuffered, pos_Array, freehand.colorname, freehand.lineWidth, freehand.lineRendererList.Count());
                        i++;
                    }
                }
            }
        }

    }
    [MunRPC]
    public void linecreat(Vector3[] pos_Array, string colorname, float width, int layer)
    {
        Vector3[] Array = new Vector3[pos_Array.Length];
        for (int i = 0; i < pos_Array.Length; i++)
        {
            Array[i] = pos_Array[i];
            Debug.Log(Array + "[" + i + "]" + "は" + Array[i]);
        }
        // Vector3[] pos_Array_rcv =pos_Array[];
        this.lineRenderer.numCapVertices = 10;
        this.lineRenderer.numCornerVertices = 10;
        // 線の色を初期化
        //lineRenderer.material.color = color;
        colortartansform(colorname);
        // 線の太さを初期化
        this.lineRenderer.startWidth = width;
        this.lineRenderer.endWidth = width;
        this.lineRenderer.positionCount = pos_Array.Length;
        this.lineRenderer.sortingOrder = layer;
        this.lineRenderer.SetPositions(Array);
    }
    private void AddPositionData()
    {
        // 座標の変換を行いマウス位置を取得
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
        var mousePosition = Camera.main.ScreenToWorldPoint(screenPosition);
        // 線と線をつなぐ点の数を更新
        lineRenderer.positionCount += 1;
        // 描く線のコンポーネントリストを更新
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, mousePosition);
        pos_list.Add(mousePosition);
        //monobitView.RPC("sendlineparameters", MonobitTargets.OthersBuffered, mousePosition, lineRenderer.positionCount - 1);
        //Debug.Log(lineRendererList.Last().positionCount - 1);
        //Debug.Log("indexは" + mousePosition);
        //monobitView.RPC("lineparameters", MonobitTargets.OthersBuffered, lineRendererList.Last().positionCount - 1, mousePosition);
    }
    /*[MunRPC]
        public void sendlineparameters(Vector3 mousePosition, int index_)
        {
            // this.lineRenderer.material=;
            Debug.Log("レンダラーデータ共有");
            pos_list.Add(mousePosition);
            //this.lineRenderer.SetPosition(index_, mousePosition);
            Debug.Log(index_);
            Debug.Log(mousePosition);
        }*/
    public void OnDestroy()
    {
        MonobitNetwork.Destroy(this.gameObject);
    }

    void colortartansform(string a)
    {
        if (a == "black")
        {
            this.lineRenderer.material.color = Color.black;
            /*lineRenderer.material = new Material(Shader.Find("Resources/textures/forWhiteBoard"));
            
            Material mat = Resources.Load<Material>("textures/forWhiteBoardblack");
            lineRenderer.material = new Material(mat); // コピーを使う。*/
        }

        if (a == "red")
        {
            this.lineRenderer.material.color = Color.red;
            /*lineRenderer.material = new Material(Shader.Find("Resources/textures/forWhiteBoard"));
            */
            //Material mat = Resources.Load<Material>("textures/forWhiteBoardred");
            //lineRenderer.material = new Material(mat); // コピーを使う。
        }

        if (a == "blue")
        {
            this.lineRenderer.material.color = Color.blue;
            //lineRenderer.material = new Material(Shader.Find("Resources/textures/forWhiteBoard"));
            //Material mat = Resources.Load<Material>("textures/forWhiteBoardblue");
            //lineRenderer.material = new Material(mat); // コピーを使う。
        }

        if (a == "green")
        {
            this.lineRenderer.material.color = Color.green;
            //lineRenderer.material = new Material(Shader.Find("Resources/textures/forWhiteBoard"));
            //Material mat = Resources.Load<Material>("textures/forWhiteBoardgreen");
            //lineRenderer.material = new Material(mat); // コピーを使う。
        }

        if (a == "yellow")
        {
            this.lineRenderer.material.color = Color.yellow;
            //lineRenderer.material = new Material(Shader.Find("Resources/textures/forWhiteBoard"));
            Material mat = Resources.Load<Material>("textures/forWhiteBoardyellow");
            lineRenderer.material = new Material(mat); // コピーを使う。
        }

        if (a == "magenta")
        {
            this.lineRenderer.material.color = Color.magenta;
            //lineRenderer.material = new Material(Shader.Find("Resources/textures/forWhiteBoard"));
            //Material mat = Resources.Load<Material>("textures/forWhiteBoardpink");
            //lineRenderer.material = new Material(mat); // コピーを使う。
        }
        if (a == "white")
        {
            this.lineRenderer.material.color = Color.white;
            //lineRenderer.material = new Material(Shader.Find("Resources/textures/forWhiteBoard"));
            //Material mat = Resources.Load<Material>("textures/forWhiteBoardwhite");
            //lineRenderer.material = new Material(mat); // コピーを使う。
        }
    }
}
