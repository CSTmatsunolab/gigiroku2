using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteSize : MonoBehaviour
{
    public Vector3 defaultScale = Vector3.zero;
    void Start()
    {
        defaultScale = this.transform.lossyScale;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lossScale = transform.lossyScale;
        Vector3 localScale = transform.localScale;
        //Vector3 targetPos = target.transform.localPosition;
        //Vector3 diff = targetPos - pivot;
        transform.localScale = new Vector3(
                localScale.x / lossScale.x * defaultScale.x,
                localScale.y / lossScale.y * defaultScale.y,
                localScale.z / lossScale.z * defaultScale.z
        );

    
    // float relativeScale = newScale.x / target.transform.localScale.x;

    // Vector3 resultPos = pivot + diff * relativeScale; 
    // target.transform.localScale = newScale;
    // target.transform.localPosition = resultPos;
    }
}
