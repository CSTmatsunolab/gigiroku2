using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;





using MonobitEngine;
using UnityEngine.SceneManagement;
using MonobitEngine.VoiceChat;



public class VoiceActor : MonobitEngine.MonoBehaviour
{


[SerializeField] private string m_DeviceName;
IconCreate script;

    private const int SAMPLE_RATE = 48000;
    private const float MOVING_AVE_TIME = 0.05f;

    //MOVING_AVE_TIMEに相当するサンプル数
    private const int MOVING_AVE_SAMPLE = (int)(SAMPLE_RATE * MOVING_AVE_TIME);
    
    private AudioSource m_MicAudioSource;

        public int OnSpeechID;

   // [SerializeField] private GameObject m_Cube;
    [SerializeField, Range(10, 300)] private float m_AmpGain = 100;

    private void Awake() {
        m_MicAudioSource = GetComponent<AudioSource>();
    }



    void Start() {
        string targetDevice = "";
        
        foreach (var device in Microphone.devices) {
            Debug.Log($"Device Name: {device}");
            if (device.Equals(m_DeviceName)) {
                targetDevice = device;
            }
        }
        
        Debug.Log($"=== Device Set: {targetDevice} ===");
        MicStart(targetDevice);
    }

    void Update() {
        if (!m_MicAudioSource.isPlaying) return;
        
        float[] waveData = new float[MOVING_AVE_SAMPLE];
        m_MicAudioSource.GetOutputData(waveData, 0);

        //バッファ内の平均振幅を取得（絶対値を平均する）
        float audioLevel = waveData.Average(Mathf.Abs);
        //m_Cube.transform.localScale = new Vector3(1, 1 + m_AmpGain * audioLevel, 1);
        Debug.Log(audioLevel );
        if(audioLevel >= 0){
            // monobitView.RPC("script.GetComponent<IconCreate>().OnSpeech", MonobitTargets.OthersBuffered, OnSpeechID);
            // Player owner = MonobitView.Owner;
        script = GameObject.Find("UserIcon").GetComponent<IconCreate>();
        script.GetComponent<IconCreate>().OnSpeech();
        //script.OnSpeech();
        Debug.Log("audio");
        }
    }
    
public void example(){
    //ここに処理書こう

}

    private void MicStart(string device) {
        if (device.Equals("")) return;
        
        m_MicAudioSource.clip = Microphone.Start(device, true, 1, SAMPLE_RATE);

        //マイクデバイスの準備ができるまで待つ
        while (!(Microphone.GetPosition("") > 0)) { }
        
        m_MicAudioSource.Play();
    }
    
}