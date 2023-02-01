using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    WebCamTexture webcamTexture;
    // Start is called before the first frame update
    void Start()
    {
        if (Application.streamingAssetsPath.Contains("/private/var/folders/"))
        {
            SceneManager.LoadScene("CheckAplication");
        }
        WebCamDevice[] devices = WebCamTexture.devices;
        webcamTexture = new WebCamTexture(devices[0].name, 640, 480, 30);
        StartCoroutine(LoadScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator LoadScene()
    {
        //yield return new WaitForSeconds(3);
        yield return null;
        AsyncOperation async = SceneManager.LoadSceneAsync("StartScene");
        while (!async.isDone)
        {
            yield return null;
        }
    }
}
