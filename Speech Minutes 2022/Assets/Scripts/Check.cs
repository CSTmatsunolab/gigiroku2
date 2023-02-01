using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Check : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Application.streamingAssetsPath.Contains("/private/var/folders/"))
        {
            SceneManager.LoadScene("CheckAplication");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
