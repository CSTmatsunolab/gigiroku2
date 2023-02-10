using System;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MonobitEngine;
using UnityEngine;
using UnityEngine.UI;

public class CreatePDF : MonobitEngine.MonoBehaviour
{
    //リセットファイルの格納
    string ResetFilePath;

    //ログデータの格納
    string LogDataFilePath = @"/LogDatas/LogData.txt";

    //現在の話題名の格納
    string nowwadai;

    //ルームの名前の格納
    public GameObject RoomNameText;

    //ルーム名の格納
    string roomname;

    //発言者の格納
    string speaker;

    //発言内容の格納
    string content;

    //現在の発言者名の格納
    string nowspeaker = "";

    //発言者名の文字数の格納
    int namenumber;

    //話題の格納
    public GameObject WadaiText;

    //開始時間の格納
    string startHour;

    //終了時間の格納
    string startMinute;


    // Start is called before the first frame update
    void Start()
    {
        //現在の日付を取得
        DateTime starttime = DateTime.Now;

        //開始時間
        startHour = starttime.Hour.ToString();

        //開始分
        startMinute = starttime.Minute.ToString();

        //logdata(1~8).txtの中身をリセット
        ResetLogData();
    }

    // Update is called once per frame
    void Update()
    {
        //RPCメッセージを送信
        monobitView.RPC("NowLogPath", MonobitTargets.All, LogDataFilePath);
    }


    //logdata(1~8).txtの中身をリセット
    public void ResetLogData()
    {
        //LogData1.txtのリセット
        ResetFilePath = Application.streamingAssetsPath + @"/LogDatas/LogData1.txt";
        using (var fileStream = new FileStream(ResetFilePath, FileMode.Open))
        {
            fileStream.SetLength(0);
        }

        //LogData2.txtのリセット
        ResetFilePath = Application.streamingAssetsPath + @"/LogDatas/LogData2.txt";
        using (var fileStream = new FileStream(ResetFilePath, FileMode.Open))
        {
            fileStream.SetLength(0);
        }

        //LogData3.txtのリセット
        ResetFilePath = Application.streamingAssetsPath + @"/LogDatas/LogData3.txt";
        using (var fileStream = new FileStream(ResetFilePath, FileMode.Open))
        {
            fileStream.SetLength(0);
        }

        //LogData4.txtのリセット
        ResetFilePath = Application.streamingAssetsPath + @"/LogDatas/LogData4.txt";
        using (var fileStream = new FileStream(ResetFilePath, FileMode.Open))
        {
            fileStream.SetLength(0);
        }

        //LogData5.txtのリセット
        ResetFilePath = Application.streamingAssetsPath + @"/LogDatas/LogData5.txt";
        using (var fileStream = new FileStream(ResetFilePath, FileMode.Open))
        {
            fileStream.SetLength(0);
        }

        //LogData6.txtのリセット
        ResetFilePath = Application.streamingAssetsPath + @"/LogDatas/LogData6.txt";
        using (var fileStream = new FileStream(ResetFilePath, FileMode.Open))
        {
            fileStream.SetLength(0);
        }

        //LogData7.txtのリセット
        ResetFilePath = Application.streamingAssetsPath + @"/LogDatas/LogData7.txt";
        using (var fileStream = new FileStream(ResetFilePath, FileMode.Open))
        {
            fileStream.SetLength(0);
        }

        //LogData8.txtのリセット
        ResetFilePath = Application.streamingAssetsPath + @"/LogDatas/LogData8.txt";
        using (var fileStream = new FileStream(ResetFilePath, FileMode.Open))
        {
            fileStream.SetLength(0);
        }
    }

    //話題ボタンが選択された
    public void LogPanelShare(int number)
    {
        //ホストだったら
        if (MonobitEngine.MonobitNetwork.isHost)
        {
            switch (number)
            {
                //話題1ボタンが押された
                case 1:

                    //LogData1.txtのファイルパスを選択
                    LogDataFilePath = @"/LogDatas/LogData1.txt";

                    //switch文を抜ける
                    break;

                //話題2ボタンが押された
                case 2:
                    LogDataFilePath = @"/LogDatas/LogData2.txt";
                    break;

                //話題3ボタンが押された
                case 3:
                    LogDataFilePath = @"/LogDatas/LogData3.txt";
                    break;

                //話題4ボタンが押された
                case 4:
                    LogDataFilePath = @"/LogDatas/LogData4.txt";
                    break;

                //話題5ボタンが押された
                case 5:
                    LogDataFilePath = @"/LogDatas/LogData5.txt";
                    break;

                //話題6ボタンが押された
                case 6:
                    LogDataFilePath = @"/LogDatas/LogData6.txt";
                    break;

                //話題7ボタンが押された
                case 7:
                    LogDataFilePath = @"/LogDatas/LogData7.txt";
                    break;

                //話題8ボタンが押された
                case 8:
                    LogDataFilePath = @"/LogDatas/LogData8.txt";
                    break;
            }
        }
        //ファイルパスの変更を他の端末に共有
        NowRPC();
    }

    //他の端末に共有
    public void NowRPC()
    {
        //RPCメッセージを送信
        monobitView.RPC("NowLogPath", MonobitTargets.All, LogDataFilePath);
    }

    //現在の話題のテキストのファイルパスを共有
    [MunRPC]
    public void NowLogPath(string Path)
    {
        //ファイルパスのnull対策
        if (Path != null)
        {
            LogDataFilePath = Path; 

        }
        else
        {
            return;
        }
    }

    public void csvfile()
    {
        //現在の日付を取得
        DateTime dt = DateTime.Now;

        //ドキュメントを作成
        Document doc = new Document();

        //出力先の定義(ダウンロード)
        string fileName = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Downloads/minutes.pdf";

        //pdfの名前の設定
        PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.Create));

        //ドキュメントを開く
        doc.Open();

        //Font1の作成
        iTextSharp.text.Font fnt1 = new iTextSharp.text.Font(BaseFont.CreateFont(Application.streamingAssetsPath + @"/msgothic.ttc,0", BaseFont.IDENTITY_H, true), 20);

        //Font2の作成
        iTextSharp.text.Font fnt2 = new iTextSharp.text.Font(BaseFont.CreateFont(Application.streamingAssetsPath + @"/msgothic.ttc,0", BaseFont.IDENTITY_H, true), 13);

        //Font3の作成
        iTextSharp.text.Font fnt3 = new iTextSharp.text.Font(BaseFont.CreateFont(Application.streamingAssetsPath + @"/msgothic.ttc,0", BaseFont.IDENTITY_H, true), 10);

        //現在の話題名の取得
        nowwadai = WadaiText.GetComponent<Text>().text;

        //ワード(現在の話題：)を消す
        nowwadai = nowwadai.Replace("現在の話題：", "");

        //タイトルの作成
        Paragraph title = new Paragraph(nowwadai + "議事録", fnt1);

        //中央揃え
        title.Alignment = Element.ALIGN_CENTER;

        //ドキュメントにタイトルを追加
        doc.Add(title);

        //改行
        doc.Add(new Paragraph("", fnt2));

        //ドキュメントに日時の追加
        doc.Add(new Paragraph("1. 日時    ", fnt2));
        doc.Add(new Paragraph("     " + dt.Year + "年" + dt.Month + "月" + dt.Day + "日("+startHour+"時"+startMinute+"分〜"+dt.Hour+"時"+dt.Minute+"分)", fnt3));

        //部屋名の取得
        roomname = RoomNameText.GetComponent<Text>().text;

        //ワード(roomName:)を消す
        roomname = roomname.Replace("roomName:", "");

        //ドキュメントに場所の追加
        doc.Add(new Paragraph("2. 場所", fnt2));
        doc.Add(new Paragraph("     オンライン(" + roomname + ")", fnt3));

        //出席者名の格納
        string attendance = "";

        //出席者の取得を取得しドキュメントに追加
        doc.Add(new Paragraph("3. 出席者", fnt2));
        foreach (MonobitEngine.MonobitPlayer player in MonobitEngine.MonobitNetwork.playerList)
        {
            attendance += "     "+player.name+ "\n";
        }
        doc.Add(new Paragraph(attendance, fnt3));

        //参加人数のカウント
        doc.Add(new Paragraph("3.5. 参加人数", fnt2));

        //音声認識の議事録を作成
        doc.Add(new Paragraph("4.議事録", fnt2));

        //話題番号のLogData.txrの取得
        string file = Application.streamingAssetsPath + LogDataFilePath;

        //txtデータをUTF-8で読み込み
        StreamReader sr = new StreamReader(file, Encoding.GetEncoding("UTF-8"));
        while (sr.EndOfStream == false)
        {
            //一行づつ読み込む
            string line = sr.ReadLine();

            //,で発言者と発言内容を分ける
            int index = line.IndexOf(",");
            speaker = line.Substring(0, index);
            content = line.Substring(index + 1);

            //名前のバイト数を得る
            Encoding en = Encoding.GetEncoding("Shift_JIS");
            int count = en.GetByteCount(speaker);

            //空文字の作成
            string blank = "";

            //前の発言者と違う
            if (nowspeaker != speaker)
            {
                //ドキュメントに名前の追加
                doc.Add(new Phrase("    " + speaker, fnt3));

                //現在の発言者の更新
                nowspeaker = speaker;

            }
            //前の発言者と同じ
            else
            {
                //ドキュメントに空白を追加
                doc.Add(new Phrase("    " + blank.PadRight(count, ' '), fnt3));
            }

            //内容のスタート位置を統一する
            namenumber = 10 - count;
            doc.Add(new Phrase(blank.PadLeft(namenumber, ' ') + content+"\n", fnt3));
        }

        //ドキュメントを閉じる
        doc.Close();
        Debug.Log("PDFの作成終了");
    }
}
