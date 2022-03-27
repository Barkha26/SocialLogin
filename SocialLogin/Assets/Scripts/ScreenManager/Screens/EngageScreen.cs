using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System;

public class EngageScreen : BaseUIScreen
{
    public Sprite selected;
    public Sprite UnSelected;
    public Button engageBtn;

    const string newsFeedEndApi = "post/get-all?offset=0";

    public NewsFeedObject newsPrefabOnlyText;
    public NewsFeedObject newsPrefabMedia;
    public Transform newsParent;

    public List<GameObject> feedSceneObjects;

    public override void OnBackButtonPressed()
    {
        Deactivate();
    }

    public override void OnButtonPressed()
    {
        string btnName = EventSystem.current.currentSelectedGameObject.name;
        ButtonPressed(btnName);
    }

    private void ButtonPressed(string btnName)
    {
    }


    public override void OnScreenEnabled()
    {
        engageBtn.GetComponent<Image>().sprite = selected;
        engageBtn.GetComponent<Image>().SetNativeSize();
        DestroyExistingFeeds();
        APIManager.instance.GetData(newsFeedEndApi,OnFetchingFeedsSuccess, OnFetchingFeedsFail);
        ScreenManager.Instance.ActivateScreen<CommonButtonBottomScreen>();
        ScreenManager.Instance.ActivateScreen<CommonButtonTopScreen>();
    }

    private void OnFetchingFeedsSuccess(object recievedUnityRequest)
    {
        UnityWebRequest request = recievedUnityRequest as UnityWebRequest;
        if (request.error != null)
        {
            Debug.LogError(request.error);
            return;
        }

        string json = request.downloadHandler.text;
        List<NewsFeed> feeds = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NewsFeed>>(json);

        for (int i = 0; i < feeds.Count; i++)
        {
            if (string.Compare(feeds[i].type, "TEXT") == 0)
            {
                SetTextTypeFeed(feeds[i]);
            }
            else if (string.Compare(feeds[i].type, "MEDIA") == 0)
            {
                SetMediaTypeFeed(feeds[i]);

            }
            else if (string.Compare(feeds[i].type, "LINK") == 0)
            {
                SetTextTypeFeed(feeds[i]);
                //SetLinkTypeFeed(feeds[i]);
            }
        }
    }

    private void OnFetchingFeedsFail(object obj)
    {
        
    }

    private void DestroyExistingFeeds()
    {
        for (int i = 0; i < feedSceneObjects.Count; i++)
        {
            Destroy(feedSceneObjects[i]);
        }

        feedSceneObjects = new List<GameObject>();
    }

    private void SetTextTypeFeed(NewsFeed newsFeed)
    {
        NewsFeedObject newsFeedObject = Instantiate(newsPrefabOnlyText, newsParent);
        feedSceneObjects.Add(newsFeedObject.gameObject);
        newsFeedObject.headingText.text = newsFeed.title;
        newsFeedObject.url = newsFeed.url;
        newsFeedObject.descriptionText.text = newsFeed.description;
        newsFeedObject.dateText.text = GetDateTime(newsFeed.created_at).ToString();
    }

    private void SetMediaTypeFeed(NewsFeed newsFeed)
    {
        NewsFeedObject newsFeedObject = Instantiate(newsPrefabMedia, newsParent);
        feedSceneObjects.Add(newsFeedObject.gameObject);
        newsFeedObject.headingText.text = newsFeed.title;
        newsFeedObject.url = newsFeed.url;
        newsFeedObject.descriptionText.text = newsFeed.description;
        StartCoroutine(newsFeedObject.image.GetComponent<ImageDownloader>().CallAPITextureType(newsFeed.media));
        newsFeedObject.dateText.text = GetDateTime(newsFeed.created_at).ToString();
    }
        

    public override void OnScreenDisabled()
    {
        engageBtn.GetComponent<Image>().sprite = UnSelected;
        engageBtn.GetComponent<Image>().SetNativeSize();
    }

    private DateTime GetDateTime(string unixTimeStamp)
    {
        double dateTime = double.Parse(unixTimeStamp);
        DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(dateTime).ToLocalTime();
        return dtDateTime;
    }
}
