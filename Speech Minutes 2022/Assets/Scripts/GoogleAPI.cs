using UnityEngine;
using System;
using Google.Cloud.Speech.V1;
using System.IO;
using Google.Protobuf;
using System.Threading.Tasks;
using MonobitEngine;
using UnityEngine.UI;
using System.Threading;

public class GoogleAPI : MonobitEngine.MonoBehaviour
{
    //サウンドデータの格納
    AudioClip tmp;

    //ログパネルのテキストの格納
    [SerializeField]
    public GameObject[] LogText;

    //音声認識結果の文字
    string recognition_word;

    // メインスレッドに処理を戻すためのオブジェクト
    private SynchronizationContext MainThread;

    // Start is called before the first frame update
    void Start()
    {
        // サービスアカウントの鍵ファイルパス
        string secretPath = Application.streamingAssetsPath + @"/GoogleAPI/secretkey.json";

        // GoogleCredentialを取得
        System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", secretPath, EnvironmentVariableTarget.Process);

        //現在のスレッドを取得
        MainThread = SynchronizationContext.Current;
    }

    //RecStartButtonが押された時の処理
    public void RecStartButtonOnClick()
    {
        //デバイス名を指定して録音を開始する
        tmp = Microphone.Start(Microphone.devices[0], true, 5, 44100);

        //マイクがオンになるまで待機
        while (Microphone.GetPosition(null) <= 0) { }

        //録音開始タイミングの出力
        Debug.Log("音声認識スタート！");

        //録音ファイルのループ作成
        InvokeRepeating(nameof(Audio), 4f, 5f);
    }

    //RecStopButtonが押された時の処理
    public void RecStopButtonOnClick()
    {
        //録音の終了
        Microphone.End(Microphone.devices[0]);

        //録音ファイルの作成を終了
        CancelInvoke();
    }

    //wavファイル作成の非同期処理
    async void Audio()
    {
        //wavファイルの作成
        SavWav.Save("sample_voice", tmp);

        //wavファイルをGoogleへ
        Task task = Task.Run(() => {
            Audio2();
        });
    }

    //音声認識の非同期処理
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
            //音声認識結果
            recognition_word = result.Alternatives[0].Transcript;

            // メインスレッドに処理を戻す
            MainThread.Post(_ => RecognitionRPC(), null);
        }
    }

    //他の端末と共有させる
    public void RecognitionRPC()
    {
        //RPCメッセージを送信
        monobitView.RPC("WadaiPannelText", MonobitTargets.All, recognition_word);
    }

    //ログパネルのテキストに音声認識結果を表示
    [MunRPC]
    public void WadaiPannelText(string word)
    {
        LogText[0].GetComponent<Text>().text += Environment.NewLine;
        LogText[0].GetComponent<Text>().text += word;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}


