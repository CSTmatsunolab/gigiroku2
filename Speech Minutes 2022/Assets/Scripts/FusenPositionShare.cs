using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using System.Linq;
using UnityEngine.UI;
public class FusenPositionShare : MonobitEngine.MonoBehaviour
{

    public GameObject fusenpanel;
    public void OnOtherPlayerConnected(MonobitEngine.MonobitPlayer newPlayer)
    {
        monobitView.RPC("sharepos", MonobitEngine.MonobitTargets.Others, newPlayer.ID, fusenpanel.transform.position);
    }

    [MunRPC]
    public void sharepos(int id, Vector3 pos)
    {
        if (id == MonobitEngine.MonobitNetwork.player.ID)
        {
            fusenpanel.transform.position = pos;
        }
        else { return; }
    }
}
