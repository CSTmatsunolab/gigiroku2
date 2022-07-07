using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using UnityEngine.SceneManagement;
using MonobitEngine.VoiceChat;
using UnityEngine.UI;
public class WhiteBoardparameterSend : MonobitEngine.MonoBehaviour
{


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

    LineRenderer lineRenderer;



    int index_ = 0;


    void Start()
    {
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
    }


    void Update()
    {
        if (monobitView.isMine)
        {
            //MUNサーバに接続している場合
            if (MonobitNetwork.isConnect)
            {
                // ルームに入室している場合
                if (MonobitNetwork.inRoom)
                {

                    // ボタンが押された時に線オブジェクトの追加を行う
                    if (Input.GetMouseButtonDown(0))
                    {
                        monobitView.RPC("initializesend", MonobitTargets.OthersBuffered, this.GetComponent<LineRenderer>().numCapVertices, this.GetComponent<LineRenderer>().numCornerVertices);
                    }

                    // ボタンが押されている時、LineRendererに位置データの設定を指定していく
                    if (Input.GetMouseButton(0))
                    {

                        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
                        var mousePosition = Camera.main.ScreenToWorldPoint(screenPosition);



                        monobitView.RPC("sendlineparameters", MonobitTargets.OthersBuffered, index_, mousePosition);
                        index_++;
                    }
                }
            }
        }

    }
    [MunRPC]
    public void initializesend(int numCapVertices, int numCornerVertices)
    {

        lineRenderer.numCapVertices = numCapVertices;
        lineRenderer.numCornerVertices = numCornerVertices;
        // マテリアルを初期化
        lineRenderer.material = lineMaterial;

        // 線の色を初期化
        lineRenderer.material.color = lineColor;

        // 線の太さを初期化
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }
    [MunRPC]
    public void sendlineparameters(int index_, Vector3 mousePosition)
    {
        lineRenderer.SetPosition(index_, mousePosition);
    }

}

