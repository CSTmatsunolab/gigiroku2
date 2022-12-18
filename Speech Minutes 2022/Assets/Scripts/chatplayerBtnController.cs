using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chatplayerBtnController : MonobitEngine.MonoBehaviour
{
    public int ButtonID;
    public GameObject Personaltextchat;

    GameObject canvas;

    private GameObject tergetchatobj;

    private CanvasGroup canvasGroup;

    private void Start()
    {
        tergetchatobj = Instantiate(Personaltextchat);
        canvas = GameObject.Find("Canvas");
        tergetchatobj.transform.SetParent(canvas.transform, false);
        canvasGroup = tergetchatobj.GetComponent<CanvasGroup>();
        tergetchatobj.name = "chat" + ButtonID.ToString();
        tergetchatobj.GetComponent<Personaltextchat>().partnerID = ButtonID;

    }
    // Start is called before the first frame update
    public void OnOtherPlayerDisconnected(MonobitEngine.MonobitPlayer otherPlayer)
    {
        //Debug.Log("OnOtherPlayerDisconnected : playerName = " + otherPlayer.name);
        if (otherPlayer.ID == ButtonID)
        {
            Destroy(this.gameObject);
        }
    }
    public void OnClickbtn()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

    }
}
