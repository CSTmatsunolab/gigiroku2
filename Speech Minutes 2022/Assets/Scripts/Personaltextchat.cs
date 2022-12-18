using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MonobitEngine;
using UnityEngine.SceneManagement;
using MonobitEngine.VoiceChat;
using UnityEngine.UI;

public class Personaltextchat : MonobitEngine.MonoBehaviour
{

    public bool firstrcv = true;
    public int partnerID;


    public CanvasGroup textChatAlpha;

    public GameObject textchatPrefab;

    public RectTransform content;

    public InputField inputField;
    public GameObject TextChatOrigin;
    DateTime dt;

    public Text chatname;
    private void Start()
    {
        TextChatOrigin = GameObject.Find("TextChat");
        foreach (MonobitPlayer player in MonobitNetwork.playerList)
        {
            if (player.ID == partnerID)
            {
                chatname.text = player.name + "とのチャット";
            }
        }
        textChatAlpha.alpha = 0;
        textChatAlpha.blocksRaycasts = false;
        dt = DateTime.Now;
    }

    public void OnClickSendBtn()
    {
        // ルームに入室している場合
        if (MonobitNetwork.inRoom)
        {
            /*GameObject text = Instantiate(textchatPrefab, content);
            text.GetComponent<Text>().text = MonobitEngine.MonobitNetwork.player.name + " " + inputField.text;
            text.GetComponent<Text>().text.Replace(" ", "\u00A0");*/
            GameObject text = Instantiate(textchatPrefab, content);
            text.transform.GetChild(0).gameObject.GetComponent<Text>().text = name + " " + dt.Hour.ToString() + "時" + dt.Minute.ToString() + "分" + "\n" + inputField.text;
            text.transform.GetChild(0).gameObject.GetComponent<Text>().text.Replace(" ", "\u00A0");
            monobitView.RPC("Personalrcv", MonobitEngine.MonobitTargets.Others, MonobitEngine.MonobitNetwork.player.ID, partnerID, inputField.text, System.DateTime.Now.ToString());
            inputField.text = "";
        }
        else { return; }
    }
    [MunRPC]
    public void Personalrcvv(int partner, int myid, string textcontent, string name)
    {
        if (myid == MonobitEngine.MonobitNetwork.player.ID)
        {
            //GameObject obj = GameObject.Find("chat" + partner.ToString());
            GameObject text = Instantiate(textchatPrefab, content);
            text.transform.GetChild(0).gameObject.GetComponent<Text>().text = name + " " + dt.Hour.ToString() + "時" + dt.Minute.ToString() + "分" + "\n" + textcontent;
            text.transform.GetChild(0).gameObject.GetComponent<Text>().text.Replace(" ", "\u00A0");
            //text.GetComponent<Text>().text = name + " " + dt.Hour.ToString() + "時" + dt.Minute.ToString() + "分" + "\n" + textcontent;
            //text.GetComponent<Text>().text.Replace(" ", "\u00A0");
        }
        else { return; }

    }
    public void OnClickCloseBtn()
    {
        textChatAlpha.alpha = 0;
        textChatAlpha.blocksRaycasts = false;
    }

}
