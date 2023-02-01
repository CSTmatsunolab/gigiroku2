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

    // Start is called before the first frame update
    void Start()
    {
        //使用するパス(Application.streamingAssetsPath)が変だったら
        if (Application.streamingAssetsPath.Contains("/private/var/folders/"))
        {
            //エラー番号1
            ErrorNumber = 1;
            //エラー画面に遷移する
            SceneManager.LoadScene("ErrorScene");
        }
        ErrorNumber = 3;
        //利用可能なデバイスのリストの取得
        WebCamDevice[] devices = WebCamTexture.devices;
        //カメラの取得
        webcamTexture = new WebCamTexture(devices[0].name, 640, 480, 30);
        //LoadingSceneの設定
        StartCoroutine(LoadScene());
    }

    // Update is called once per frame
    void Update()
    {
        //円の回転
        circle.transform.rotation *= Quaternion.AngleAxis(1, Vector3.back);
    }


    IEnumerator LoadScene()
    {
        //1秒待機
        yield return new WaitForSeconds(1);
        //Loadingのsliderを10%進める
        LoadingSlider.value = 0.1f;
        //たまごちゃんのsliderを10%進める
        EggSlider.value = 0.1f;
        //1秒待機
        yield return new WaitForSeconds(1);
        //Loadingのsliderを20%進める
        LoadingSlider.value = 0.2f;
        //たまごちゃんのsliderを20%進める
        EggSlider.value = 0.2f;
        //1秒待機
        yield return new WaitForSeconds(1);
        //StartSceneの事前ロードをする
        AsyncOperation async = SceneManager.LoadSceneAsync("StartScene");
        //ロードしている間
        while (!async.isDone)
        {
            //Loadingのsliderをロードに応じて進める
            LoadingSlider.value = async.progress;
            //たまごちゃんのsliderをロードに応じて%進める
            EggSlider.value = async.progress;
            //1フレーム待機
            yield return null;
        }
    }
}
