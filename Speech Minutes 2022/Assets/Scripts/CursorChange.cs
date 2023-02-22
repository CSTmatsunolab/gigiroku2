using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MonobitEngine;

public class CursorChange : MonobitEngine.MonoBehaviour
{

    [SerializeField]
    GameObject CursorImage;

    [SerializeField]
    GameObject canvas;

    public Texture2D RedCursor;

    public int pointerjudge = 0;

    public GameObject Panel;

    public bool Always = false;

    public bool Only = false;

    public Button Alwaysbtn;

    public Button Onlybtn;

    //GameObject prefab;
    public void Start()
    {

    }
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            //Debug.Log(pointerjudge);
            pointerjudge = 0;
            if (Alwaysbtn.interactable == false)
            {
                if (freehand.penmode == true)
                {
                    MonobitEngine.MonobitNetwork.Instantiate("CursorPointer", Vector3.zero, Quaternion.identity, 0);
                    Debug.Log("CursorPointer複製");
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (Alwaysbtn.interactable == false)
            {
                pointerjudge = 1;
            }
        }
    }
    public void OnclickPointerButton_Always()
    {
        pointerjudge = 0;
        Always = !Always;
        if (Always == true)
        {
            Only = false;
            Onlybtn.interactable = false;
            MonobitEngine.MonobitNetwork.Instantiate("CursorPointer", Vector3.zero, Quaternion.identity, 0);
            //prefab.transform.SetParent(canvas.transform, false);
            Debug.Log("CursorPointer複製");
            //Cursor.visible = false;
        }
        else
        {
            pointerjudge = 1;
            Onlybtn.interactable = true;
        }
    }

    public void OnclickPointerButton_Only()
    {
        pointerjudge = 0;
        Only = !Only;
        if (Only == true)
        {
            Debug.Log("true");
            Always = false;
            Alwaysbtn.interactable = false;
        }
        else
        {
            Debug.Log("false");
            Alwaysbtn.interactable = true;
        }
    }

    /*public void OnPD()
    {
        Debug.Log("生きてる");
        pointerjudge = 0;
        if (Alwaysbtn.interactable == false)
        {
            MonobitEngine.MonobitNetwork.Instantiate("CursorPointer", Vector3.zero, Quaternion.identity, 0);
            Debug.Log("CursorPointer複製");

        }
    }

    public void OnPU()
    {
        Debug.Log("生きてる");
        if (Alwaysbtn.interactable == false)
        {
            pointerjudge = 1;
        }

    }*/


    public void OnPointerEnter()
    {
        Panel.SetActive(true);
    }

    public void OnPointerExit()
    {
        Panel.SetActive(false);
    }
}