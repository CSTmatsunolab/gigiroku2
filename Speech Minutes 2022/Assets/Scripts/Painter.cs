﻿using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// お絵描き
/// </summary>
public class Painter : MonoBehaviour
{
    Texture2D texture;
    Vector3 beforeMousePos;

    public Color bgColor ;
    public Color lineColor;
    /// <summary>
    /// ボタンと連動
    /// </summary>
    public GameObject PenMode;
    public bool mode = true;
    public Text Buttontext;
    public Material fontMaterial;
    [Range(0,10)] public float lineWidth;

    void Start()
    {
        var img = GetComponent<Image>();
        var rt = GetComponent<RectTransform>();
        var width = (int)rt.rect.width;
        var height = (int)rt.rect.height;
        texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

        PenMode = GameObject.Find("PenMode");

        //背景が透明なTexture2Dを作る
        //http://d.hatena.ne.jp/shinriyo/20140520/p2
        Color32[] texColors = Enumerable.Repeat<Color32>(bgColor, width * height).ToArray();
        texture.SetPixels32(texColors);
        texture.Apply();
    }

    void Update()
    {
        Start();
        
        if (mode)
        {


            if (Input.GetMouseButtonDown(0))
            {
                beforeMousePos = GetPosition();
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 v = GetPosition();
                LineTo(beforeMousePos, v, lineColor);
                beforeMousePos = v;
                texture.Apply();
            }
        }
    }

    //PenMode切り替え用
    public void Mode()
    {
        if (mode)
        {
            mode = false;
            Debug.Log("PenMode:" + mode);
            Buttontext.text = "PenMode:false";
        }
        else
        {
            mode = true;
            Debug.Log("PenMode:" + mode);
            Buttontext.text = "PenMode:true";
        }
    }
    /// <summary>
    /// UIのクリック座標のx、y座標を求める - 新しいguiシステムの画像 - Unity Answers
    /// https://answers.unity.com/questions/892333/find-xy-cordinates-of-click-on-uiimage-new-gui-sys.html
    /// </summary>
    public Vector3 GetPosition()
    {
        var dat = new PointerEventData(EventSystem.current);
        dat.position = Input.mousePosition;

        var rect1 = GetComponent<RectTransform>();
        var pos1 = dat.position;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rect1, pos1,
            null, out Vector2 localCursor))
            return localCursor;

        int xpos = (int)(localCursor.x);
        int ypos = (int)(localCursor.y);

        if (xpos < 0) xpos = xpos + (int)rect1.rect.width / 2;
        else xpos += (int)rect1.rect.width / 2;

        if (ypos > 0) ypos = ypos + (int)rect1.rect.height / 2;
        else ypos += (int)rect1.rect.height / 2;

        Debug.Log("Correct Cursor Pos: " + xpos + " " + ypos);
        return new Vector3(xpos, ypos, 0);
    }

    public void Clear()
    {
        Start();
    }
    /// <summary>
    /// Unityでお絵描きしてみる
    /// http://tech.gmo-media.jp/post/56101930112/draw-a-picture-with-unity
    /// </summary>
    public void LineTo(Vector3 start, Vector3 end, Color color)
    {
        float x = start.x, y = start.y;
        // color of pixels
        Color[] wcolor = { color };

        if (Mathf.Abs(start.x - end.x) > Mathf.Abs(start.y - end.y))
        {
            float dy = Math.Abs(end.x - start.x) < float.Epsilon ? 0 : (end.y - start.y) / (end.x - start.x);
            float dx = start.x < end.x ? 1 : -1;
            //draw line loop
            while (x > 0 && x < texture.width && y > 0 && y < texture.height)
            {
                try
                {
                    texture.SetPixels((int)x, (int)y, 1, 1, wcolor);
                    x += dx;
                    y += dx * dy;
                    if (start.x < end.x && x > end.x ||
                        start.x > end.x && x < end.x)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    break;
                }
            }
        }
        else if (Mathf.Abs(start.x - end.x) < Mathf.Abs(start.y - end.y))
        {
            float dx = Math.Abs(start.y - end.y) < float.Epsilon ? 0 : (end.x - start.x) / (end.y - start.y);
            float dy = start.y < end.y ? 1 : -1;
            while (x > 0 && x < texture.width && y > 0 && y < texture.height)
            {
                try
                {
                    texture.SetPixels((int)x, (int)y, 1, 1, wcolor);
                    x += dx * dy;
                    y += dy;
                    if (start.y < end.y && y > end.y ||
                        start.y > end.y && y < end.y)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    break;
                }
            }
        }
    }
}