using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ErrorScene : MonoBehaviour
{
    //エラー0の説明画像
    public GameObject Error0Image;
    //エラー1の説明画像
    public GameObject Error1Image;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //エラー番号0
        if (LoadingScene.ErrorNumber == 0)
        {
            //エラー0の説明画像の表示
            Error0Image.SetActive(true);
        }
        else if(LoadingScene.ErrorNumber == 1)
        {
            //エラー1の説明画像の表示
            Error1Image.SetActive(true);
        }
    }
}
