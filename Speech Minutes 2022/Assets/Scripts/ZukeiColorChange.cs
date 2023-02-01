using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MonobitEngine;

public class ZukeiColorChange : MonobitEngine.MonoBehaviour
{
    [SerializeField]
    GameObject Image;
    Dropdown dropdown;
    void Start()
    {
        dropdown = GetComponent<Dropdown>();
        
    }

    //付箋の色を変更するメソッド
    public void ChangeColor(Dropdown dropdown)
    {
        Debug.Log("Color Change");
        switch (dropdown.value)
        {
            case 0:
                //自分の付箋の色を変更する処理
                Image.GetComponentInChildren<Image>().color = ToColor("#ffc0cb");
                //自分以外にも上記の付箋の色変更が反映される処理
                Debug.Log("2 Color Change");
                monobitView.RPC("RecvTextColor", MonobitTargets.OthersBuffered, dropdown.value);
                Debug.Log("3 Color Change");
                break;
                
            case 1:
                Image.GetComponentInChildren<Image>().color = ToColor("#fffacd");
                Debug.Log("2 Color Change");
                monobitView.RPC("RecvTextColor", MonobitTargets.OthersBuffered, dropdown.value);
                Debug.Log("3 Color Change");
                break;

            case 2:
                Image.GetComponentInChildren<Image>().color = ToColor("#98fb98");
                Debug.Log("2 Color Change");
                monobitView.RPC("RecvTextColor", MonobitTargets.OthersBuffered, dropdown.value);
                Debug.Log("3 Color Change");
                break;

            case 3:
                Image.GetComponentInChildren<Image>().color = ToColor("#fa8072");
                Debug.Log("2 Color Change");
                monobitView.RPC("RecvTextColor", MonobitTargets.OthersBuffered, dropdown.value);
                Debug.Log("3 Color Change");
                break;

            case 4:
                Image.GetComponentInChildren<Image>().color = ToColor("#87cefa");
                Debug.Log("2 Color Change");
                monobitView.RPC("RecvTextColor", MonobitTargets.OthersBuffered, dropdown.value);
                Debug.Log("3 Color Change");
                break;
    
            default:
                break;
        }
    }

    //カラーコードから色を呼び出すメソッド
    public  Color ToColor(string Color)
    {
         var color = default(Color);
        if (!ColorUtility.TryParseHtmlString(Color, out color))
         {
        }
        return color;
    }

    //ドロップダウンの値から付箋の色を変える処理を自分以外にMunRPCを使って送るメソッド
    [MunRPC]
    public void RecvTextColor(int colorValue)
    {
        Debug.Log("RPC Color Change");
        switch (colorValue)
        {
            
            case 0:
                Image.GetComponentInChildren<Image>().color = ToColor("#ffc0cb");
                dropdown.value = colorValue;
                break;
            case 1:
                Image.GetComponentInChildren<Image>().color = ToColor("#fffacd");
                dropdown.value = colorValue;
                break;
            case 2:
                Image.GetComponentInChildren<Image>().color = ToColor("#98fb98");
                dropdown.value = colorValue;
                break;
            case 3:
                Image.GetComponentInChildren<Image>().color = ToColor("#fa8072");
                dropdown.value = colorValue;
                break;
            case 4:
                Image.GetComponentInChildren<Image>().color = ToColor("#87cefa");
                dropdown.value = colorValue;
                break;
            default:
                break;
        }
    }
}
