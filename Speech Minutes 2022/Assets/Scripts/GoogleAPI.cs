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
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
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

    public int NowBottonPushed = -1;

    public ScrollRect[] ScrollRect;

    GameObject MUN;
    // Start is called before the first frame update

    //public GameObject go;
    private MonobitMicrophone Mc = null;
    //public AudioClip Mc;
    public AudioClip AC;

    public GameObject WadaiText;
    public GameObject RoomNameText;

    string LogDataFilePath;
    string LogDataFilePath1;

    GameObject go;

    string nowwadai;
    string roomname;
    String result1;
    String result2;
    String result3="";
    int namenumber;
    int namenumber2;

    void Start()
    {
        // サービスアカウントの鍵ファイルパス
        string secretPath = Application.streamingAssetsPath + @"/GoogleAPI/secretkey.json";

        // GoogleCredentialを取得
        System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", secretPath, EnvironmentVariableTarget.Process);

        //現在のスレッドを取得
        MainThread = SynchronizationContext.Current;


        //LogDataFilePath1 = Application.streamingAssetsPath + @"/LogDatas/LogData1.txt";
        //using (var fileStream = new FileStream(LogDataFilePath1, FileMode.Open))
        //{
        //    fileStream.SetLength(0);
        //}
        LogDataFilePath1 = Application.streamingAssetsPath + @"/LogDatas/LogData2.txt";
        using (var fileStream = new FileStream(LogDataFilePath1, FileMode.Open))
        {
            fileStream.SetLength(0);
        }
        LogDataFilePath1 = Application.streamingAssetsPath + @"/LogDatas/LogData3.txt";
        using (var fileStream = new FileStream(LogDataFilePath1, FileMode.Open))
        {
            fileStream.SetLength(0);
        }
        LogDataFilePath1 = Application.streamingAssetsPath + @"/LogDatas/LogData4.txt";
        using (var fileStream = new FileStream(LogDataFilePath1, FileMode.Open))
        {
            fileStream.SetLength(0);
        }
        LogDataFilePath1 = Application.streamingAssetsPath + @"/LogDatas/LogData5.txt";
        using (var fileStream = new FileStream(LogDataFilePath1, FileMode.Open))
        {
            fileStream.SetLength(0);
        }
        LogDataFilePath1 = Application.streamingAssetsPath + @"/LogDatas/LogData6.txt";
        using (var fileStream = new FileStream(LogDataFilePath1, FileMode.Open))
        {
            fileStream.SetLength(0);
        }
        LogDataFilePath1 = Application.streamingAssetsPath + @"/LogDatas/LogData7.txt";
        using (var fileStream = new FileStream(LogDataFilePath1, FileMode.Open))
        {
            fileStream.SetLength(0);
        }
        LogDataFilePath1 = Application.streamingAssetsPath + @"/LogDatas/LogData8.txt";
        using (var fileStream = new FileStream(LogDataFilePath1, FileMode.Open))
        {
            fileStream.SetLength(0);
        }

    }

    //RecStartButtonが押された時の処理
    public void RecStartButtonOnClick()
    {
        go = GameObject.Find("VoiceActor(Clone)");
        // myVoice = go.GetComponent<MonobitVoice>();
        Mc = go.GetComponent<MonobitMicrophone>();
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
        monobitView.RPC("WadaiPannelText", MonobitTargets.All, recognition_word, NowBottonPushed);
    }

    [MunRPC]
    public void WadaiPannelText(string word, int push)
    {
        Debug.Log("値 " + push);
        if (push == -1)
        {
            LogText[8].GetComponent<Text>().text += Environment.NewLine;
            LogText[8].GetComponent<Text>().text += word;
        }
        else
        {
            LogText[push].GetComponent<Text>().text += Environment.NewLine;
            LogText[push].GetComponent<Text>().text += word;
            
            ///ログが更新された時ログパネルのスクロールバーを一番下まで自動でスクロールさせる
            StartCoroutine(ForceScrollDown(push));

            switch (push)
            {
                case 0:
                    LogDataFilePath = Application.streamingAssetsPath + @"/LogDatas/LogData1.txt";
                    break;

                case 1:
                    LogDataFilePath = Application.streamingAssetsPath + @"/LogDatas/LogData2.txt";
                    break;

                case 2:
                    LogDataFilePath = Application.streamingAssetsPath + @"/LogDatas/LogData3.txt";
                    break;

                case 3:
                    LogDataFilePath = Application.streamingAssetsPath + @"/LogDatas/LogData4.txt";
                    break;

                case 4:
                    LogDataFilePath = Application.streamingAssetsPath + @"/LogDatas/LogData5.txt";
                    break;

                case 5:
                    LogDataFilePath = Application.streamingAssetsPath + @"/LogDatas/LogData6.txt";
                    break;

                case 6:
                    LogDataFilePath = Application.streamingAssetsPath + @"/LogDatas/LogData7.txt";
                    break;

                case 7:
                    LogDataFilePath = Application.streamingAssetsPath + @"/LogDatas/LogData8.txt";
                    break;
            }

            File.AppendAllText(LogDataFilePath, MonobitNetwork.playerName + "," + word + "\n");
        }
    }
    IEnumerator ForceScrollDown(int push)
    {

        // 1フレーム待たないと完全に実行されない
        yield return new WaitForEndOfFrame();
        ScrollRect[push].verticalNormalizedPosition = 0.0f;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LogPanelShare(int number)
    {

        switch (number)
        {
            case 0:
                NowBottonPushed = 0;
                break;

            case 1:
                NowBottonPushed = 1;
                break;

            case 2:
                NowBottonPushed = 2;
                break;

            case 3:
                NowBottonPushed = 3;
                break;

            case 4:
                NowBottonPushed = 4;
                break;

            case 5:
                NowBottonPushed = 5;
                break;

            case 6:
                NowBottonPushed = 6;
                break;

            case 7:
                NowBottonPushed = 7;
                break;
        }
    }

    public void csvflie()
    {
        // DateTime.Nowをコンソール表示する
        DateTime dt = DateTime.Now;
        Document doc = new Document();
        PdfWriter.GetInstance(doc, new FileStream("01_Hello.pdf", FileMode.Create));
        doc.Open();

        nowwadai = WadaiText.GetComponent<Text>().text;
        nowwadai = nowwadai.Replace("現在の話題：","");
        iTextSharp.text.Font fnt1 = new iTextSharp.text.Font(BaseFont.CreateFont(Application.streamingAssetsPath + @"/msgothic.ttc,0", BaseFont.IDENTITY_H, true), 20);
        Paragraph title = new Paragraph(nowwadai + "議事録", fnt1);
        title.Alignment = Element.ALIGN_CENTER;
        doc.Add(title);
        

        iTextSharp.text.Font fnt2 = new iTextSharp.text.Font(BaseFont.CreateFont(Application.streamingAssetsPath + @"/msgothic.ttc,0", BaseFont.IDENTITY_H, true), 13);
        iTextSharp.text.Font fnt3 = new iTextSharp.text.Font(BaseFont.CreateFont(Application.streamingAssetsPath + @"/msgothic.ttc,0", BaseFont.IDENTITY_H, true), 10);
        doc.Add(new Paragraph("", fnt2));
        doc.Add(new Paragraph("1. 日時    ", fnt2));
        doc.Add(new Paragraph("     "+dt.Year + "年" + dt.Month + "月" + dt.Day + "日", fnt3));

        roomname = RoomNameText.GetComponent<Text>().text;
        roomname = roomname.Replace("roomName:", "");
        doc.Add(new Paragraph("2. 場所", fnt2));
        doc.Add(new Paragraph("     オンライン(" + roomname + ")", fnt3));

        doc.Add(new Paragraph("3. 出席者",fnt2));
      
        string str = "";
        foreach (MonobitEngine.MonobitPlayer player in MonobitEngine.MonobitNetwork.playerList)
        {
            str += player.name + "\r\n";
            doc.Add(new Paragraph("     "+str, fnt3));

        }

        doc.Add(new Paragraph("4.議事録", fnt2));

        string file = LogDataFilePath;
        StreamReader sr = new StreamReader(file, Encoding.GetEncoding("UTF-8"));
        while (sr.EndOfStream == false)
        {
            string line = sr.ReadLine();
            int index = line.IndexOf(",");
            result1 = line.Substring(0, index);
            result2 = line.Substring(index+1);
            if (result3 != result1)
            {
                doc.Add(new Phrase("    " + result1, fnt3));
                result3 = result1;
            }
            else
            {
                string a="";
                doc.Add(new Phrase("    "+a.PadRight(result1.Length*2,' '), fnt3));
            }

            namenumber =5-result1.Length;
            doc.Add(new Phrase("    "+result2.PadLeft(namenumber,' ')+"\n", fnt3));
        }
        doc.Close();
        Debug.Log("終了");
    }

}


