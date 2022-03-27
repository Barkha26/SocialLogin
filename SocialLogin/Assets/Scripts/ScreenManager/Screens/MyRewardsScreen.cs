using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class MyRewardsScreen : BaseUIScreen
{
    public GameObject defaultScreen;
    public GameObject rewardWonScreen;
    public GameObject claimedScreen;

    private const string socialEndApi = "social-login";

    public override void OnScreenEnabled()
    {
        Data data = AppOfflineManager.instance.fetchedInfo.data;
        string name = data.name;
        string email = data.email;
        string imageUrl = data.image;
        string token = data.token;
        string loginMode = data.mode;

        SocialLogin login = new SocialLogin(name, email, token, imageUrl, loginMode);
        APIManager.instance.PostData(login, socialEndApi, OnUpdatingLoginSuccess, OnUpdateLoginError);

        ScreenManager.Instance.ActivateScreen<CommonButtonBottomScreen>();
        ScreenManager.Instance.ActivateScreen<CommonButtonTopScreen>();
    }

    private void OnUpdateLoginError(string obj)
    {
       
    }

    private void OnUpdatingLoginSuccess(string json)
    {
        AppOfflineManager.instance.OnFetchingLoginDetails(json);
        if (AppOfflineManager.instance.fetchedInfo.data.reward_notified && !AppOfflineManager.instance.fetchedInfo.data.reward_claimed)
        {
            rewardWonScreen.SetActive(true);
            defaultScreen.SetActive(false);
            claimedScreen.SetActive(false);
        }
        else if (AppOfflineManager.instance.fetchedInfo.data.reward_claimed && AppOfflineManager.instance.fetchedInfo.data.reward_claimed)
        {
            rewardWonScreen.SetActive(false);
            defaultScreen.SetActive(false);
            claimedScreen.SetActive(true);
        }
        else
        {
            rewardWonScreen.SetActive(false);
            defaultScreen.SetActive(true);
            claimedScreen.SetActive(false);

        }
    }

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
        switch (btnName)
        {
            case "CopmanyUrl_Btn":
                Application.OpenURL("https://www.bellimmersive.com/");
                break;
        }
    }
}
