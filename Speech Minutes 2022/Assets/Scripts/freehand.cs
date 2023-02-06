
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using System.Linq;
using UnityEngine.UI;

public class freehand : MonobitEngine.MonoBehaviour
{
    /// <summary>
    /// 描く線のコンポーネントリスト
    /// </summary>
    public List<LineRenderer> lineRendererList;
    /// <summary>
    /// 描く線のマテリアル
    /// </summary>
    public Material lineMaterial;
    /// <summary>
    /// 描く線の色
    /// </summary>
    //public Color lineColor;
    /// <summary>
    /// 描く線の太さ
    /// </summary>
    public float lineWidth = 1.0f;
    //[SerializeField] private AnimationCurve _animationCurve;
    public Transform parent;
    GameObject obj;
    public Vector3 mousePosition_;

    public bool penmode = false;

    public Text Buttontext;

    public Color pencolor;

    public Dropdown colordropdown;
    public Dropdown widthdropdown;

    public string colorname;

    void Awake()
    {
        lineRendererList = new List<LineRenderer>();
    }
    void Update()
    {
        //MUNサーバに接続している場合
        if (MonobitNetwork.isConnect)
        {
            // ルームに入室している場合
            if (MonobitNetwork.inRoom)
            {
                if (penmode)
                {

                    if(Input.GetKeyDown("s"))
                    {
				        GameObject prefab = MonobitEngine.MonobitNetwork.Instantiate("ZukeiCanvas", Vector3.zero, Quaternion.identity, 0,null,false,true,true);
            	        Debug.Log("複製完了");
			        }
			        if(Input.GetKeyDown("c"))
                    {
				        GameObject prefab = MonobitEngine.MonobitNetwork.Instantiate("CircleCanvas", Vector3.zero, Quaternion.identity, 0,null,false,true,true);
            	        Debug.Log("複製完了");
			        }
			        if(Input.GetKeyDown("t"))
                    {
				        GameObject prefab = MonobitEngine.MonobitNetwork.Instantiate("TriangleCanvas", Vector3.zero, Quaternion.identity, 0,null,false,true,true);
            	        Debug.Log("複製完了");
                    }

                    if (Input.GetMouseButtonDown(1))
                    {
                        UndoLine();
                    }
                    // ボタンが押された時に線オブジェクトの追加を行う
                    if (Input.GetMouseButtonDown(0))
                    {
                        this.AddLineObject();
                    }
                    // ボタンが押されている時、LineRendererに位置データの設定を指定していく
                    /*if (Input.GetMouseButton(0))
                    {
                        //this.AddPositionDataToLineRendererList();
                    }*/
                }
            }
        }


    }
    /// <summary>
    /// 線オブジェクトの追加を行うメソッド
    /// </summary>
    private void AddLineObject()
    {
        colorset();
        widthset();
        // 追加するオブジェクトをインスタンス
        //GameObject lineObject = new GameObject();
        GameObject lineObject = MonobitNetwork.Instantiate("lineobject", Vector3.zero, Quaternion.identity, 0);
        //obj = lineObject;
        Debug.Log("おk");
        //lineObject.GetComponent<Transform>().SetParent(parent);

        //lineObject.GetComponent<Transform>().SetAsLastSibling();
        // オブジェクトにLineRendererを取り付ける
        //lineObject.AddComponent<LineRenderer>();
        //lineObject.AddComponent<MonobitTransformView>();
        //registercmp(lineObject);

        //lineObject.GetComponent<LineRenderer>().widthCurve = _animationCurve;
        lineObject.GetComponent<LineRenderer>().numCapVertices = 10;
        lineObject.GetComponent<LineRenderer>().numCornerVertices = 10;

        // 描く線のコンポーネントリストに追加する
        lineRendererList.Add(lineObject.GetComponent<LineRenderer>());
        // 線と線をつなぐ点の数を0に初期化
        lineRendererList.Last().positionCount = 0;
        // マテリアルを初期化
        //lineRendererList.Last().material = this.lineMaterial;
        // 線の色を初期化
        lineRendererList.Last().material.color = pencolor;
        // 線の太さを初期化
        lineRendererList.Last().startWidth = lineWidth;
        lineRendererList.Last().endWidth = lineWidth;
        lineRendererList.Last().sortingOrder = lineRendererList.Count();
    }

    public void Mode()
    {
        if (penmode)
        {
            penmode = false;
            Debug.Log("PenMode:" + penmode);
            Buttontext.text = "PenMode:false";
        }
        else
        {
            penmode = true;
            Debug.Log("PenMode:" + penmode);
            Buttontext.text = "PenMode:true";
        }
    }
    void colorset()
    {
        if (colordropdown.value == 0)
        {
            pencolor = Color.black;
            colorname = "black";
        }

        if (colordropdown.value == 1)
        {
            pencolor = Color.red;
            colorname = "red";
        }

        if (colordropdown.value == 2)
        {
            pencolor = Color.blue;
            colorname = "blue";
        }

        if (colordropdown.value == 3)
        {
            pencolor = Color.green;
            colorname = "green";
        }

        if (colordropdown.value == 4)
        {
            pencolor = Color.yellow;
            colorname = "yellow";
        }

        if (colordropdown.value == 5)
        {
            pencolor = Color.magenta;
            colorname = "magenta";
        }
        if (colordropdown.value == 6)
        {
            pencolor = Color.white;
            colorname = "white";
        }
    }

    void widthset()
    {
        if (widthdropdown.value == 0)
        {
            lineWidth = 0.1f;
        }

        if (widthdropdown.value == 1)
        {
            lineWidth = 0.3f;
        }

        if (widthdropdown.value == 2)
        {
            lineWidth = 0.6f;
        }
        if (widthdropdown.value == 3)
        {
            lineWidth = 1.0f;
        }
    }
    // スクリプト Boo の同期処理を、Observed Component Registration List に登録します。
    /*public void registercmp(GameObject obj)
    {
        // Boo のコンポーネントを取得します。
        UnityEngine.Component component = obj.GetComponent<MonobitTransformView>();
        // 二重登録を防止するため、Contains で見つからなかった場合、リストに登録するようにします。
        if (!this.monobitView.ObservedComponents.Contains(component))
        {
            this.monobitView.ObservedComponents.Add(component);
            // 登録内容を monobitView オブジェクトに反映させます。
            monobitView.UpdateSerializeViewMethod();
        }
    }
    // スクリプト Boo の同期処理を、Observed Component Registration List から削除します。
    public void removecmp(GameObject obj)
    {
        // Boo のコンポーネントを取得します。
        UnityEngine.Component component = obj.GetComponent<MonobitTransformView>();

        // Contains で見つかった場合、リストから削除するようにします。
        if (monobitView.ObservedComponents.Contains(component))
        {
            monobitView.ObservedComponents.Remove(component);
            // 削除内容を monobitView オブジェクトに反映させます。
            monobitView.UpdateSerializeViewMethod();
        }
    }*/
    /// <summary>
    /// /// 描く線のコンポーネントリストに位置情報を登録していく
    /// </summary>
    /*private void AddPositionDataToLineRendererList()
    {
        // 座標の変換を行いマウス位置を取得
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
        var mousePosition = Camera.main.ScreenToWorldPoint(screenPosition);
        mousePosition_ = mousePosition;
        // 線と線をつなぐ点の数を更新
        lineRendererList.Last().positionCount += 1;
        // 描く線のコンポーネントリストを更新
        lineRendererList.Last().SetPosition(lineRendererList.Last().positionCount - 1, mousePosition);
        //Debug.Log(lineRendererList.Last().positionCount - 1);
        //Debug.Log("indexは" + mousePosition);
        //monobitView.RPC("lineparameters", MonobitTargets.OthersBuffered, lineRendererList.Last().positionCount - 1, mousePosition);
    }*/
    /* 
    [MunRPC]
     public void lineparameters(Vector3 newvec3, int newindex)
     {
         //obj.GetComponent<LineRenderer>().SetPosition(newindex, newvec3);
         /*lineRenderer.numCapVertices = numCapVertices;
         lineRenderer.numCornerVertices = numCornerVertices;
         // マテリアルを初期化
         lineRenderer.material = lineMaterial;
         // 線の色を初期化
         lineRenderer.material.color = lineColor;
         // 線の太さを初期化
         lineRenderer.startWidth = lineWidth;
         lineRenderer.endWidth = lineWidth;*/
    //}
    void UndoLine()
    {
        try
        {
            var lastLineRenderer = lineRendererList.Last();
            //removecmp(lastLineRenderer.gameObject);
            lastLineRenderer.GetComponent<WhiteBoardparameterSend>().OnDestroy();
            //MonobitNetwork.Destroy(lastLineRenderer.gameObject);
            lineRendererList.Remove(lastLineRenderer);
        }
        catch (System.InvalidOperationException)
        {
            Debug.Log("線がないためUndoされませんでした");
        }
    }
    public void clear()
    {
        while (lineRendererList.Count != 0)
        {
            var lastLineRenderer = lineRendererList.Last();
            //removecmp(lastLineRenderer.gameObject);
            lastLineRenderer.GetComponent<WhiteBoardparameterSend>().OnDestroy();
            //MonobitNetwork.Destroy(lastLineRenderer.gameObject);
            lineRendererList.Remove(lastLineRenderer);
        }

    }
}
