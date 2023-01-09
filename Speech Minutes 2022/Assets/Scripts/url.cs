using System.Collections;
using Hypertext;
using UnityEngine;
using UnityEngine.UI;

public class url : MonoBehaviour
{
    [SerializeField] private RegexHypertext _MessageText;
    const string RegexURL = "http(s)?://([\\w-]+\\.)+[\\w-]+(/[\\w- ./?%&=]*)?";
    private Color32 _HyperLinkColor = new Color32(0, 65, 125, 255);

    void Start()
    {
        _MessageText.OnClick(RegexURL, _HyperLinkColor, url => OpenBrowser(url));
    }

    public void OpenBrowser(string url)
    {
        Application.OpenURL(url);
    }
}