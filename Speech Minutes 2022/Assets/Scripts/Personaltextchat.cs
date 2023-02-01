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

    CanvasGroup chatred;

    List<GameObject> chatlist = new List<GameObject>();

    TextChat textChat;

    public Text chatname;
    private void Start()
    {
        TextChatOrigin = GameObject.Find("TextChat");
        textChat = TextChatOrigin.GetComponent<TextChat>();
        foreach (MonobitPlayer player in MonobitNetwork.playerList)
        {
            if (player.ID == partnerID)
            {
                chatname.text = player.name + "さんとのチャット";
            }
        }
        textChatAlpha.alpha = 0;
        textChatAlpha.blocksRaycasts = false;
        chatred = GameObject.Find("chatred").GetComponent<CanvasGroup>();
    }
    void Update()
    {
        if (inputField.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnClickSendBtn();
            }
            //dt = DateTime.Now;
        }
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
            dt = DateTime.Now;
            text.transform.GetChild(0).gameObject.GetComponent<Text>().text = MonobitEngine.MonobitNetwork.player.name + " " + dt.Hour.ToString() + "時" + dt.Minute.ToString() + "分" + "\n" + inputField.text;
            text.transform.GetChild(0).gameObject.GetComponent<Text>().text.Replace(" ", "\u00A0");
            chatlist.Add(text);
            text.GetComponent<TextCopy>().indexnum = chatlist.IndexOf(text);
            text.GetComponent<TextCopy>().parentobj = this.gameObject;
            monobitView.RPC("Personalrcv", MonobitEngine.MonobitTargets.Others, MonobitEngine.MonobitNetwork.player.ID, partnerID, inputField.text, MonobitEngine.MonobitNetwork.player.name);
            inputField.text = "";
        }
        else { return; }
    }
    [MunRPC]
    public void Personalrcv(int partner, int myid, string textcontent, string name)
    {
        if (myid == MonobitEngine.MonobitNetwork.player.ID)
        {
            Debug.Log("itsme...");
            GameObject obj = GameObject.Find("chat" + partner.ToString()).transform.Find("Scroll View/Viewport/Content").gameObject;
            RectTransform rect = obj.GetComponent<RectTransform>();
            //GameObject obj = GameObject.Find("chat" + partner.ToString());
            GameObject text = Instantiate(textchatPrefab, rect);
            dt = DateTime.Now;
            text.transform.GetChild(0).gameObject.GetComponent<Text>().text = name + " " + dt.Hour.ToString() + "時" + dt.Minute.ToString() + "分" + "\n" + textcontent;
            text.transform.GetChild(0).gameObject.GetComponent<Text>().text.Replace(" ", "\u00A0");
            chatlist.Add(text);
            text.GetComponent<TextCopy>().indexnum = chatlist.IndexOf(text);
            text.GetComponent<TextCopy>().parentobj = this.gameObject;
            if (textChatAlpha.alpha == 0)
            {
                textChat.count++;
                textChat.newcount.text = textChat.count.ToString();
                chatred.alpha = 1;
            }
            //text.GetComponent<Text>().text = name + " " + dt.Hour.ToString() + "時" + dt.Minute.ToString() + "分" + "\n" + textcontent;
            //text.GetComponent<Text>().text.Replace(" ", "\u00A0");
        }
        else { Debug.Log("notme..."); }

    }
    public void OnClickCloseBtn()
    {
        textChatAlpha.alpha = 0;
        textChatAlpha.blocksRaycasts = false;
    }

    public void chatdelete(int indexnum)
    {
        monobitView.RPC("Delete2", MonobitEngine.MonobitTargets.Others, partnerID, indexnum);
        Destroy(chatlist[indexnum]);
    }
    [MunRPC]
    public void Delete2(int myid, int indexnum)
    {
        if (myid == MonobitEngine.MonobitNetwork.player.ID)
        {
            Destroy(chatlist[indexnum]);
        }
    }

}
