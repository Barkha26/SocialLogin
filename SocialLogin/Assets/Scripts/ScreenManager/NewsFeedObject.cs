using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsFeedObject : MonoBehaviour
{
    public Text headingText;
    public Text descriptionText;
    public string url;
    public RawImage image;
    public Text dateText;

    public void OpenUrlInBrowser()
    {
        Application.OpenURL(url);
    }
}
