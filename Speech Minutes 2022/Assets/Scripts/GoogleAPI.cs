using UnityEngine;
using System;
using Google.Cloud.Speech.V1;
using System.IO;
using Google.Protobuf;
using System.Threading.Tasks;

public class GoogleAPI : MonoBehaviour
{
    //サウンドデータの格納
    AudioClip tmp;
    
    // Start is called before the first frame update
    void Start()
    {
        // サービスアカウントの鍵ファイルパス
        string secretPath = Application.streamingAssetsPath + @"/GoogleAPI/secretkey.json";

        // GoogleCredentialを取得
        System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", secretPath, EnvironmentVariableTarget.Process);
       
        //デバイス名を指定して録音を開始する
        tmp = Microphone.Start(Microphone.devices[0], true, 5, 44100);

        //マイクがオンになるまで待機
        while (Microphone.GetPosition(null) <= 0) { }

        //設定した時間にメソッドを呼び出し設定した時間ごとにリピートする
        InvokeRepeating(nameof(Audio), 4f, 5f);
    }

    async void Audio()
    {
        //wavファイルの作成
        SavWav.Save("sample_voice", tmp);
        Task task = Task.Run(() => {
            //Audio2();
        });
       
    }
    void Audio2()
    {
        //サンプル音声ファイル読み込み
        string filePath = Application.streamingAssetsPath + @"/GoogleAPI/sample_voice.wav";

        //録音データの読み込み
        RecognitionAudio audio = new RecognitionAudio();
        System.IO.FileStream fileStream = new FileStream(filePath, FileMode.Open);
        Google.Protobuf.ByteString vs = ByteString.FromStream(fileStream);
        fileStream.Dispose();
        audio.Content = vs;

        //録音データの設定
        RecognitionConfig config = new RecognitionConfig();
        config.Encoding = RecognitionConfig.Types.AudioEncoding.Linear16;
        config.SampleRateHertz = 44100;
        config.LanguageCode = "ja-JP";

        //クライアントの生成
        SpeechClient client = SpeechClient.Create();

        ////録音データをテキストに変換
        RecognizeResponse responce = client.Recognize(config, audio);

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

