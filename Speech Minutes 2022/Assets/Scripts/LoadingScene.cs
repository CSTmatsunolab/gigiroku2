using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    //ウェブカメラの格納
    WebCamTexture webcamTexture;
    //円のオブジェクトの格納
    public GameObject circle;
    //LoadingのSliderの格納
    public Slider LoadingSlider;
    //移動するたまごちゃんの格納
    public Slider EggSlider;
    //エラー番号の管理
    public static int ErrorNumber=0;
    //背景の格納
    public GameObject BackGround;


    // Start is called before the first frame update
    void Start()
    {
        //背景の決定
        BackgroundColor();
        //使用するパス(Application.streamingAssetsPath)が変だったら
        if (Application.streamingAssetsPath.Contains("/private/var/folders/"))
        {
            //エラー番号1
            ErrorNumber = 1;
            //エラー画面に遷移する
            SceneManager.LoadScene("ErrorScene");
        }
        //LoadingSceneの設定
        StartCoroutine(LoadScene());
        //利用可能なデバイスのリストの取得
        WebCamDevice[] devices = WebCamTexture.devices;
        //カメラの取得
        webcamTexture = new WebCamTexture(devices[0].name, 640, 480, 30);
    }

    //背景カラーの決定
    void BackgroundColor()
    {
        //ランダム
        int rondomcolor = UnityEngine.Random.Range(1, 6);
        if (rondomcolor == 1)
        {
            //パープル
            BackGround.GetComponent<Image>().color = new Color32(174, 162, 229, 255);  
        }
        else if(rondomcolor == 2)
        {
            //イエロー
            BackGround.GetComponent<Image>().color = new Color32(229, 220, 162, 255);
        }
        else if (rondomcolor == 3)
        {
            //グリーン
            BackGround.GetComponent<Image>().color = new Color32(175, 229, 162, 255);
        }
        else if (rondomcolor == 4)
        {
            //ブルー
            BackGround.GetComponent<Image>().color = new Color32(162, 210, 229, 255);
        }
        else
        {
            //ピンク
            BackGround.GetComponent<Image>().color = new Color32(229, 164, 162, 255);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //LoadingSceneの設定
    IEnumerator LoadScene()
    {
        //ランダム
        float rondomvolue = UnityEngine.Random.Range(0.1f, 0.4f);
        float volue;
        //1秒待機
        yield return new WaitForSeconds(1);
        //Sliderの値
        volue = rondomvolue;
        //Loadingのsliderを進める
        LoadingSlider.value = volue;
        //1秒待機
        yield return new WaitForSeconds(1);
        //ランダム
        rondomvolue = UnityEngine.Random.Range(0.1f, 0.4f);
        //Sliderの値
        volue = volue+rondomvolue;
        //Loadingのsliderを進める
        LoadingSlider.value = volue;
        //1秒待機
        yield return new WaitForSeconds(1);
        //ランダム
        rondomvolue = UnityEngine.Random.Range(0.1f, 0.4f);
        //Sliderの値
        volue = volue + rondomvolue;
        //Loadingのsliderを進める
        LoadingSlider.value = volue;
        //1秒待機
        yield return new WaitForSeconds(1);
        //StartSceneの事前ロードをする
        AsyncOperation async = SceneManager.LoadSceneAsync("StartScene");
        //ロードしている間
        while (!async.isDone)
        {
            //Loadingのsliderをロードに応じて進める
            LoadingSlider.value = async.progress;
            //1フレーム待機
            yield return null;
        }
    }
}
