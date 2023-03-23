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
    public static int NowBottonPushed = -1;

    //スクロールの格納
    public ScrollRect[] ScrollRect;

    //MonobitMicrophoneコンポーネントの格納
    MonobitMicrophone Mc = null;

    //ログデータの格納
    public static string LogDataFilePath= @"/LogDatas/LogData.txt";

    //マイクの格納
    GameObject go;

    int a = 0;

    public Text wadaitittle;

    public GameObject[] Button;

    public GameObject WadaiElementsPanel;

    // Start is called before the first frame update
    void Start()
    {

        // サービスアカウントの鍵ファイルパス *ご自身で作成されたキーを置いてください
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
            LogText[0].GetComponent<Text>().text += name+" "+word;
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
        //ホストではなかったら
        if (!MonobitEngine.MonobitNetwork.isHost)
        {
            WadaiElementsPanel.SetActive(false);
            return;
        }
        WadaiElementsPanel.SetActive(true);

        //RPCメッセージを送信
        monobitView.RPC("NowBotton", MonobitTargets.All, LogDataFilePath, NowBottonPushed);
    }

    //どの話題が選択されたか
    public void LogPanelShare(int number)
    {
        //ホストだったら
        if (MonobitEngine.MonobitNetwork.isHost)
        {
            
            switch (number)
            {
                //話題1が選択された
                case 1:
                    if (a == number)
                    {
                        NowBottonPushed = -1;
                        LogDataFilePath = @"/LogDatas/LogData.txt";
                        a = 0;
                        break;
                    }
                    //現在選択されているのは話題1
                    NowBottonPushed = 1;

                    //LogData1.txtのファイルパスを選択
                    LogDataFilePath = @"/LogDatas/LogData1.txt";

                    a = number;

                    //swich文を抜ける
                    break;

                //話題2が選択された
                case 2:
                    if (a == number)
                    {
                        NowBottonPushed = -1;
                        LogDataFilePath = @"/LogDatas/LogData.txt";
                        a = 0;
                        break;
                    }
                    NowBottonPushed = 2;
                    LogDataFilePath = @"/LogDatas/LogData2.txt";
                    a = number;
                    break;

                //話題3が選択された
                case 3:
                    if (a == number)
                    {
                        NowBottonPushed = -1;
                        LogDataFilePath = @"/LogDatas/LogData.txt";
                        a = 0;
                        break;
                    }
                    NowBottonPushed = 3;
                    LogDataFilePath = @"/LogDatas/LogData3.txt";
                    a = number;
                    break;

                //話題4が選択された
                case 4:
                    if (a == number)
                    {
                        NowBottonPushed = -1;
                        LogDataFilePath = @"/LogDatas/LogData.txt";
                        a = 0;
                        break;
                    }
                    NowBottonPushed = 4;
                    LogDataFilePath = @"/LogDatas/LogData4.txt";
                    a = number;
                    break;

                //話題5が選択された
                case 5:
                    if (a == number)
                    {
                        NowBottonPushed = -1;
                        LogDataFilePath = @"/LogDatas/LogData.txt";
                        a = 0;
                        break;
                    }
                    NowBottonPushed = 5;
                    LogDataFilePath = @"/LogDatas/LogData5.txt";
                    a = number;
                    break;

                //話題6が選択された
                case 6:
                    if (a == number)
                    {
                        NowBottonPushed = -1;
                        LogDataFilePath = @"/LogDatas/LogData.txt";
                        a = 0;
                        break;
                    }
                    NowBottonPushed = 6;
                    LogDataFilePath = @"/LogDatas/LogData6.txt";
                    a = number;
                    break;

                //話題7が選択された
                case 7:
                    if (a == number)
                    {
                        NowBottonPushed = -1;
                        LogDataFilePath = @"/LogDatas/LogData.txt";
                        a = 0;
                        break;
                    }
                    NowBottonPushed = 7;
                    LogDataFilePath = @"/LogDatas/LogData7.txt";
                    a = number;
                    break;

                //話題8が選択された
                case 8:
                    if (a == number)
                    {
                        NowBottonPushed = -1;
                        LogDataFilePath = @"/LogDatas/LogData.txt";
                        a = 0;
                        break;
                    }
                    NowBottonPushed = 8;
                    LogDataFilePath = @"/LogDatas/LogData8.txt";
                    a = number;
                    break;
                    
            }
            Debug.Log(NowBottonPushed);
            // メインスレッドに処理を戻す
            MainThread.Post(_ => NowRPC(), null);
        }
    }

    //他の端末と共有
    public void NowRPC()
    {
        //RPCメッセージを送信
        monobitView.RPC("NowBotton", MonobitTargets.All, LogDataFilePath, NowBottonPushed);
    }

    //現在選ばれている話題
    [MunRPC]
    public void NowBotton(string Path,int Botton)
    {
        NowBottonPushed = Botton;
        LogDataFilePath = Path;
        switch (NowBottonPushed)
        {
            case 1:
                wadaitittle.text = "現在の話題：" + Button[NowBottonPushed].GetComponentInChildren<Text>().text;
                wadaitittle.color = new Color32(189, 193, 74, 255);
                break;
            case 2:
                wadaitittle.text = "現在の話題：" + Button[NowBottonPushed].GetComponentInChildren<Text>().text;
                wadaitittle.color = new Color32(195, 160, 65, 255);
                break;
            case 3:
                wadaitittle.text = "現在の話題：" + Button[NowBottonPushed].GetComponentInChildren<Text>().text;
                wadaitittle.color = new Color32(207, 89, 81, 255);
                break;
            case 4:
                wadaitittle.text = "現在の話題：" + Button[NowBottonPushed].GetComponentInChildren<Text>().text;
                wadaitittle.color = new Color32(207, 75, 200, 255);
                break;
            case 5:
                wadaitittle.text = "現在の話題：" + Button[NowBottonPushed].GetComponentInChildren<Text>().text;
                wadaitittle.color = new Color32(144, 82, 204, 255);
                break;
            case 6:
                wadaitittle.text = "現在の話題：" + Button[NowBottonPushed].GetComponentInChildren<Text>().text;
                wadaitittle.color = new Color32(74, 87, 202, 255);
                break;
            case 7:
                wadaitittle.text = "現在の話題：" + Button[NowBottonPushed].GetComponentInChildren<Text>().text;
                wadaitittle.color = new Color32(63, 197, 212, 255);
                break;
            case 8:
                wadaitittle.text = "現在の話題：" + Button[NowBottonPushed].GetComponentInChildren<Text>().text;
                wadaitittle.color = new Color32(62, 207, 69, 255);
                break;
            default:
                wadaitittle.text = "話題未選択";
                wadaitittle.color = new Color32(0, 0, 0, 255);
                break;
        }
    }
}