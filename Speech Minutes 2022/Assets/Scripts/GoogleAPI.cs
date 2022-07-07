using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Grpc.Auth;
using Grpc.Core;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Speech.V1;
using System.IO;
using System.Text;
using Google.Protobuf;

public class GoogleAPI : MonoBehaviour
{
    AudioClip tmp;
    AudioClip tmp2;
    public float span = 1f;
    private float currentTime = 0f;
    public bool Test = true;
    public int a = 0;

    // Start is called before the first frame update
    void Start()
    {
        // サービスアカウントの鍵ファイルパス
        string secretPath = Application.streamingAssetsPath + @"/GoogleAPI/secretkey.json";

        // GoogleCredentialを取得
        System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", secretPath, EnvironmentVariableTarget.Process);

        AudioSource audio = GetComponent<AudioSource>();
        foreach (string device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
        Debug.Log("a");
        audio.clip = tmp = Microphone.Start(Microphone.devices[0], true, 5, 44100);

        audio.mute = true;

        while (Microphone.GetPosition(null) <= 0) { }
        if (!audio.isPlaying)
        {
            //AudioClip.Destroy("sample_voice");
            //Debug.Log("tmpを削除しました。");
            //audio.clip = tmp = Microphone.Start(Microphone.devices[0], true, 5, 44100);
        }
        //Audio();

        InvokeRepeating(nameof(Audio), 1f, 1f);
    }

    private void Audio()
    {
        SavWav.Save("sample_voice", tmp);

        //サンプル音声ファイル読み込み
        string filePath = Application.streamingAssetsPath + @"/GoogleAPI/sample_voice.wav";

        ////録音データの読み込み
        RecognitionAudio audio1 = new RecognitionAudio();
        System.IO.FileStream fileStream = new FileStream(filePath, FileMode.Open);
        Google.Protobuf.ByteString vs = ByteString.FromStream(fileStream);
        fileStream.Dispose();
        audio1.Content = vs;

        ////録音データの設定
        RecognitionConfig config = new RecognitionConfig();
        config.Encoding = RecognitionConfig.Types.AudioEncoding.Linear16;
        config.SampleRateHertz = 44100;
        config.LanguageCode = "ja-JP";

        ////クライアントの生成
        SpeechClient client = SpeechClient.Create();

        ////録音データをテキストに変換
        RecognizeResponse responce = client.Recognize(config, audio1);

        foreach (SpeechRecognitionResult result in responce.Results)
        {
            //テキストの出力
            Debug.Log(result.Alternatives[0].Transcript);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

