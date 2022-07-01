using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZukeiSize : MonoBehaviour {

    public GameObject image;
    Vector3 pos; // 最初にクリックしたときの位置
    Quaternion position; // 最初にクリックしたときのBoxの角度
    Vector3 size;

    //Vector2 vecA; // Boxの中心からposへのベクトル
    Vector3 vecA; // Boxの中心から現在のマウス位置へのベクトル
    Vector3 mousediff;
    Vector3 nowmouse;

    float angle; // vecAとvecBが成す角度
    Vector3 AxB; // vecAとvecBの外積

    // PointerDownで呼び出す
    // クリック時にパラメータの初期値を求める
    public void SetPos(){
        Debug.Log("クリックされた");
        size = transform.parent.localScale;//図形のスケール取得
        //pos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        pos = Input.mousePosition;// マウス位置をワールド座標で取得
        Debug.Log(size);
        //position = transform.parent.position; // Boxの真ん中の位置を取得

    }

    // ハンドルをドラッグしている間に呼び出す
    public void Rotate(){
        Debug.Log("回そうとしてる");
        nowmouse = Input.mousePosition;
        mousediff = nowmouse - pos; //ある地点からのベクトルを求めるときはこう書くんだった
        //Debug.Log(mousediff);
        transform.parent.localScale = size + (mousediff/100);
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