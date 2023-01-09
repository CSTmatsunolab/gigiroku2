using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MonobitEngine;
using UnityEngine.SceneManagement;
using MonobitEngine.VoiceChat;
using UnityEngine.UI;
public class TextCopy : MonobitEngine.MonoBehaviour
{
    public Text text;

    public int indexnum;

    public GameObject parentobj;

    // Start is called before the first frame update
    public void onclicktext()
    {
        GUIUtility.systemCopyBuffer = text.text;
    }
    public void onclicktextdelete()
    {
        if (parentobj.name == "TextChat")
        {
            parentobj.GetComponent<TextChat>().monobitView.RPC("Delete", MonobitTargets.AllBuffered, indexnum);
        }
        else
        {
            parentobj.GetComponent<Personaltextchat>().chatdelete(indexnum);
        }

    }
}
