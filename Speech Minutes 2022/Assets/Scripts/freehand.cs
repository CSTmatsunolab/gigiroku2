using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using UnityEngine.SceneManagement;
using MonobitEngine.VoiceChat;
using UnityEngine.UI;
using System.Linq;

public class freehand : MonobitEngine.MonoBehaviour
{

    /// <summary>
    /// 描く線のコンポーネントリスト
    /// </summary>
    private List<LineRenderer> lineRendererList;

    /// <summary>
    /// 描く線のマテリアル
    /// </summary>
    public Material lineMaterial;

    /// <summary>
    /// 描く線の色
    /// </summary>
    public Color lineColor;

    /// <summary>
    /// 描く線の太さ
    /// </summary>
    public float lineWidth = 1.0f;

    //[SerializeField] private AnimationCurve _animationCurve;

    public Transform parent;




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
                if (Input.GetMouseButton(0))
                {
                    this.AddPositionDataToLineRendererList();
                }
            }
        }
    }

    /// <summary>
    /// 線オブジェクトの追加を行うメソッド
    /// </summary>
    private void AddLineObject()
    {

        // 追加するオブジェクトをインスタンス
        GameObject lineObject = new GameObject();

        lineObject.GetComponent<Transform>().SetParent(parent);


        //lineObject.GetComponent<Transform>().SetAsLastSibling();
        // オブジェクトにLineRendererを取り付ける
        lineObject.AddComponent<LineRenderer>();
        lineObject.AddComponent<MonobitTransformView>();
        registercmp(lineObject);


        //lineObject.GetComponent<LineRenderer>().widthCurve = _animationCurve;
        lineObject.GetComponent<LineRenderer>().numCapVertices = 10;
        lineObject.GetComponent<LineRenderer>().numCornerVertices = 10;


        // 描く線のコンポーネントリストに追加する
        lineRendererList.Add(lineObject.GetComponent<LineRenderer>());

        // 線と線をつなぐ点の数を0に初期化
        lineRendererList.Last().positionCount = 0;

        // マテリアルを初期化
        lineRendererList.Last().material = this.lineMaterial;

        // 線の色を初期化
        lineRendererList.Last().material.color = this.lineColor;

        // 線の太さを初期化
        lineRendererList.Last().startWidth = this.lineWidth;
        lineRendererList.Last().endWidth = this.lineWidth;
    }
    // スクリプト Boo の同期処理を、Observed Component Registration List に登録します。
    public void registercmp(GameObject obj)
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
    }

    /// <summary>
    /// /// 描く線のコンポーネントリストに位置情報を登録していく
    /// </summary>
    private void AddPositionDataToLineRendererList()
    {

        // 座標の変換を行いマウス位置を取得
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
        var mousePosition = Camera.main.ScreenToWorldPoint(screenPosition);

        // 線と線をつなぐ点の数を更新
        lineRendererList.Last().positionCount += 1;

        // 描く線のコンポーネントリストを更新
        lineRendererList.Last().SetPosition(lineRendererList.Last().positionCount - 1, mousePosition);
    }


    void UndoLine()
    {
        try
        {
            var lastLineRenderer = lineRendererList.Last();
            removecmp(lastLineRenderer.gameObject);
            MonobitNetwork.Destroy(lastLineRenderer.gameObject);

            lineRendererList.Remove(lastLineRenderer);
        }
        catch (System.InvalidOperationException)
        {
            Debug.Log("線がないためUndoされませんでした");
        }
    }
}
