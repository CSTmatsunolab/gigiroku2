﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActive : MonoBehaviour
{
    public GameObject Wadai;

    public Toggle toggle;


    //wadaiパネルのアクティブ・非アクティブ化
    public void OnTolggle0()
    {
        if (toggle.isOn) Wadai.SetActive(true);
        else Wadai.SetActive(false);
    }
}
