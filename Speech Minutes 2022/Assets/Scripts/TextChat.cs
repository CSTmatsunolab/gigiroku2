using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MonobitEngine;
using UnityEngine.SceneManagement;
using MonobitEngine.VoiceChat;
using UnityEngine.UI;
public class TextChat : MonobitEngine.MonoBehaviour
{

    public CanvasGroup textChatAlpha;
    public CanvasGroup chatlistAlpha;

    public GameObject textchatPrefab;
    public GameObject chatplayerBtnPrefab;

    public RectTransform content;
    public RectTransform content2;

    public InputField inputField;


    DateTime dt;
    //public Text text;

    // bool delay = false;

    CanvasGroup chatred;
    List<GameObject> chatlist = new List<GameObject>();



    public
    // Start is called before the first frame update
    void Start()
    {
        textChatAlpha.alpha = 0;
        chatlistAlpha.alpha = 0;
        textChatAlpha.blocksRaycasts = false;
        chatred = GameObject.Find("chatred").GetComponent<CanvasGroup>();

    }

    // Update is called once per frame
    void Update()
    {
        //dt = DateTime.Now;
    }
    /*public void OnJoinedRoom()
    {
        foreach (MonobitPlayer player in MonobitNetwork.playerList)
        {

            GameObject btn = Instantiate(chatplayerBtnPrefab, content2);
            btn.GetComponent<chatplayerBtnController>().ButtonID = player.ID;
            Debug.Log("buttonID" + btn.GetComponent<chatplayerBtnController>().ButtonID);
            btn.transform.GetChild(0).gameObject.GetComponent<Text>().text = player.name;
        }
       //delay = true;
    }*/
    public void OnOtherPlayerConnected(MonobitEngine.MonobitPlayer newPlayer)
    {

        GameObject btn = Instantiate(chatplayerBtnPrefab, content2);
        //chatplayerBtnController ID = btn.GetComponent<chatplayerBtnController>();
        btn.GetComponent<chatplayerBtnController>().ButtonID = newPlayer.ID;
        Debug.Log("buttonID" + btn.GetComponent<chatplayerBtnController>().ButtonID);
        btn.transform.GetChild(0).gameObject.GetComponent<Text>().text = newPlayer.name;
        //Debug.Log("OnOtherPlayerConnected : playerName = " + newPlayer.name );

    }

    public void OnClickSendBtn()
    {
        // ルームに入室している場合
        if (MonobitNetwork.inRoom)
        {
            /*GameObject text = Instantiate(textchatPrefab, content);
            text.GetComponent<Text>().text = MonobitEngine.MonobitNetwork.player.name + " " + inputField.text;
            text.GetComponent<Text>().text.Replace(" ", "\u00A0");*/
            // monobitView.RPC("TextChatrcv", MonobitTargets.AllBuffered, inputField.text, MonobitEngine.MonobitNetwork.player.name);
            GameObject text = Instantiate(textchatPrefab, content);
            dt = DateTime.Now;
            text.transform.GetChild(0).gameObject.GetComponent<Text>().text = MonobitEngine.MonobitNetwork.player.name + " " + dt.Hour.ToString() + "時" + dt.Minute.ToString() + "分" + "\n" + inputField.text;
            text.transform.GetChild(0).gameObject.GetComponent<Text>().text.Replace(" ", "\u00A0");
            chatlist.Add(text);
            text.GetComponent<TextCopy>().indexnum = chatlist.IndexOf(text);
            text.GetComponent<TextCopy>().parentobj = this.gameObject;
            monobitView.RPC("TextChatrcv", MonobitTargets.Others, inputField.text, MonobitEngine.MonobitNetwork.player.name);
            inputField.text = "";
        }
        else { return; }
    }
    [MunRPC]
    public void TextChatrcv(string textcontent, string name)
    {
        GameObject text = Instantiate(textchatPrefab, content);
        dt = DateTime.Now;
        text.transform.GetChild(0).gameObject.GetComponent<Text>().text = name + " " + dt.Hour.ToString() + "時" + dt.Minute.ToString() + "分" + "\n" + textcontent;
        text.transform.GetChild(0).gameObject.GetComponent<Text>().text.Replace(" ", "\u00A0");
        chatlist.Add(text);
        text.GetComponent<TextCopy>().indexnum = chatlist.IndexOf(text);
        text.GetComponent<TextCopy>().parentobj = this.gameObject;
        if (textChatAlpha.alpha == 0)
        {
            chatred.alpha = 1;
        }
        //text.GetComponent<Text>().text = name + " " + dt.Hour.ToString() + "時" + dt.Minute.ToString() + "分" + "\n" + textcontent;
        //text.GetComponent<Text>().text.Replace(" ", "\u00A0");
    }
    public void OnClickCloseBtn()
    {
        textChatAlpha.alpha = 0;
        textChatAlpha.blocksRaycasts = false;
    }
    public void OnClickTextChatBtn()
    {
        if (textChatAlpha.alpha == 1)
        {
            textChatAlpha.alpha = 0;
            textChatAlpha.blocksRaycasts = false;
        }
        else
        {
            textChatAlpha.alpha = 1;
            textChatAlpha.blocksRaycasts = true;
        }
    }
    public void Enter()
    {
        chatlistAlpha.alpha = 1;
        chatlistAlpha.blocksRaycasts = true;
    }
    public void Exit()
    {
        chatlistAlpha.alpha = 0;
        chatlistAlpha.blocksRaycasts = false;

    }

    public void OnClickchaticon()
    {
        chatred.alpha = 0;
    }


    [MunRPC]
    public void Delete(int indexnum)
    {
        Destroy(chatlist[indexnum]);
    }



}
