using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorChange : MonoBehaviour
{
    public Texture2D RedCursor;
    
    int pointerjudge = 0;

        public void OnclickCursorButton()
    {
        if(pointerjudge == 0)
        {
            Cursor.SetCursor(RedCursor,new Vector2(RedCursor.width / 2,RedCursor.height / 2), CursorMode.Auto);
            pointerjudge = 1;
        }
        else
        {
            Cursor.SetCursor(null,Vector2.zero, CursorMode.Auto);
            pointerjudge = 0;
        }
    }
}