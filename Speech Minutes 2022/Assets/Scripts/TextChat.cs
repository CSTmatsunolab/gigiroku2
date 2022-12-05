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

    public GameObject textchatPrefab;

    public RectTransform content;

    public InputField inputField;



    public
    // Start is called before the first frame update
    void Start()
    {
        textChatAlpha.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickSendBtn()
    {
        // ルームに入室している場合
        if (MonobitNetwork.inRoom)
        {
            /*GameObject text = Instantiate(textchatPrefab, content);
            text.GetComponent<Text>().text = MonobitEngine.MonobitNetwork.player.name + " " + inputField.text;
            text.GetComponent<Text>().text.Replace(" ", "\u00A0");*/
            monobitView.RPC("TextChatrcv", MonobitTargets.AllBuffered, inputField.text, MonobitEngine.MonobitNetwork.player.name);
            inputField.text = "";
        }
        else { return; }
    }
    [MunRPC]
    public void TextChatrcv(string textcontent, string name)
    {
        GameObject text = Instantiate(textchatPrefab, content);
        text.GetComponent<Text>().text = name + " " + textcontent;
        text.GetComponent<Text>().text.Replace(" ", "\u00A0");
    }
    public void OnClickCloseBtn()
    {
        textChatAlpha.alpha = 0;
    }
    public void OnClickTextChatBtn()
    {
        if (textChatAlpha.alpha == 1)
        {
            textChatAlpha.alpha = 0;
        }
        else
        {
            textChatAlpha.alpha = 1;
        }
    }

}
