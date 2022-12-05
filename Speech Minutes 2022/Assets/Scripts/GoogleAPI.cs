using UnityEngine;
using System;
using Google.Cloud.Speech.V1;
using System.IO;
using Google.Protobuf;
using System.Threading.Tasks;
using MonobitEngine;
using UnityEngine.UI;
using System.Threading;
using System.Collections;
using MonobitEngine.VoiceChat;


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

    //話題ボタンの管理
    int NowBottonPushed = -1;

    //スクロールの格納
    public ScrollRect[] ScrollRect;

    //MonobitMicrophoneコンポーネントの格納
    MonobitMicrophone Mc = null;

    //ログデータの格納
    string LogDataFilePath;
    

    GameObject go;

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
        //VoiceActor(Clone)のGameObjectを探す
        go = GameObject.Find("VoiceActor(Clone)");

        //MonobitMicrophoneを取得
        Mc = go.GetComponent<MonobitMicrophone>();

        //AudioClipの取得
        tmp = Mc.GetAudioClip();

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
        //Microphone.End(Microphone.devices[0]);

        //録音ファイルの作成を終了
        CancelInvoke();
    }

    //wavファイル作成の非同期処理
    async void Audio()
    {
        //wavファイルの作成
        SavWav.Save("sample_voice", tmp);

        //wavファイルをGoogleへ
        Task task = Task.Run(() =>
        {
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
        monobitView.RPC("WadaiPannelText", MonobitTargets.All, MonobitNetwork.playerName,recognition_word, NowBottonPushed);
    }

    //ログパネルに音声認識結果の表示
    [MunRPC]
    public void WadaiPannelText(string name,string word, int push)
    {
        //話題が未選択
        if (push == -1)
        {
            //存在しないログに格納
            LogText[8].GetComponent<Text>().text += name+" "+word;
        }
        //話題番号が選択されている
        else
        {
            //改行する
            LogText[push].GetComponent<Text>().text += Environment.NewLine;

            //話題番号に応じたログパネルに表示
            LogText[push].GetComponent<Text>().text += name+" "+word;
            
            ///ログが更新された時ログパネルのスクロールバーを一番下まで自動でスクロールさせる
            StartCoroutine(ForceScrollDown(push));

            //話題番号に応じたLogData.txtに音声認識結果を格納
            File.AppendAllText(Application.streamingAssetsPath + LogDataFilePath, name + "," + word + "\n");
        }
    }

    //ログパネルのスクロールの設定
    IEnumerator ForceScrollDown(int push)
    {
        // 1フレーム待たないと完全に実行されない
        yield return new WaitForEndOfFrame();

        //スクロールの位置を1番下にする
        ScrollRect[push].verticalNormalizedPosition = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //どの話題が選択されたか
    public void LogPanelShare(int number)
    {
        switch (number)
        {
            //話題1が選択された
            case 1:

                //現在選択されているのは話題1
                NowBottonPushed = 1;

                //LogData1.txtのファイルパスを選択
                LogDataFilePath = @"/LogDatas/LogData1.txt";

                //swich文を抜ける
                break;

            //話題2が選択された
            case 2:
                NowBottonPushed = 2;
                LogDataFilePath = @"/LogDatas/LogData2.txt";
                break;

            //話題3が選択された
            case 3:
                NowBottonPushed = 3;
                LogDataFilePath = @"/LogDatas/LogData3.txt";
                break;

            //話題4が選択された
            case 4:
                NowBottonPushed = 4;
                LogDataFilePath = @"/LogDatas/LogData4.txt";
                break;

            //話題5が選択された
            case 5:
                NowBottonPushed = 5;
                LogDataFilePath = @"/LogDatas/LogData5.txt";
                break;

            //話題6が選択された
            case 6:
                NowBottonPushed = 6;
                LogDataFilePath = @"/LogDatas/LogData6.txt";
                break;

            //話題7が選択された
            case 7:
                NowBottonPushed = 7;
                LogDataFilePath = @"/LogDatas/LogData7.txt";
                break;

            //話題8が選択された
            case 8:
                NowBottonPushed = 8;
                LogDataFilePath = @"/LogDatas/LogData8.txt";
                break;
        }
        // メインスレッドに処理を戻す
        MainThread.Post(_ => NowRPC(), null);

    }

    public void NowRPC()
    {
        //RPCメッセージを送信
        monobitView.RPC("NowBotton", MonobitTargets.All, LogDataFilePath, NowBottonPushed);
    }

    [MunRPC]
    public void NowBotton(string Path,int Botton)
    {
        NowBottonPushed = Botton;
        LogDataFilePath = Path;
        Debug.Log("NowBottonPushedGoogle*" + NowBottonPushed);
        Debug.Log("LogDataFilePathGoogle*" + LogDataFilePath);
    }
}