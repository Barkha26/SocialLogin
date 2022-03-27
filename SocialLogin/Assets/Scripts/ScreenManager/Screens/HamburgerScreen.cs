using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using System.Collections;

public class HamburgerScreen : BaseUIScreen
{
    public RawImage profilePic;
    public Text username;
    public Text designation;

    LoginMode loginmode;
    const string popupMessage = "Are you sure you want to Sign Out?";

    private void Start()
    {
        ObjectManager.RegisterCallBackObject(ObjectEventVariables.userData, OnGettingUserDetails);
    }

    public void OnGettingUserDetails(object param)
    {

        UserInfo userInfo = param as UserInfo;
        StartCoroutine(StartSetup(userInfo));

    }

    IEnumerator StartSetup(UserInfo info)
    {
        yield return null;
        username.text = info.name;
        loginmode = info.loginMode;
        SetPictureSize(info.picture);
    }

    private void SetPictureSize(Texture texture)
    {
        profilePic.texture = texture;
        profilePic.rectTransform.anchorMin = new Vector2(1, 1);
        profilePic.rectTransform.anchorMax = new Vector2(1, 1);
        profilePic.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        profilePic.rectTransform.sizeDelta = Vector2.one * 100;
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
            case "signout_button":
                Debug.Log(ScreenManager.Instance.signoutPopup);
                ScreenManager.Instance.signoutPopup.SetPopup(popupMessage, true, true, OnSubmit, OnCancel);
                break;
            case "Reward_Btn":
                ActivateDeactivateCommonStuff();
                ScreenManager.Instance.ActivateScreen<MyRewardsScreen>();
                break;
            case "Certificate_Btn":
                ActivateDeactivateCommonStuff();
                ScreenManager.Instance.ActivateScreen<MyLearningScreen>();
                break;
            case "AboutUS_Btn":

                break;
        }
    }

    private void ActivateDeactivateCommonStuff()
    {
        ScreenManager.Instance.DeactivateAllScreens();
        ScreenManager.Instance.ActivateScreen<CommonButtonBottomScreen>();
        ScreenManager.Instance.ActivateScreen<CommonButtonTopScreen>();
        GetComponent<SliderMenuAnim>().ShowHideMenu();
    }

    private void OnSubmit(IPopup obj)
    {
        switch (loginmode)
        {
            case LoginMode.facebook:
                FacebookLogin.instance.FacebookLogout();
                break;
            case LoginMode.google:
                FacebookLogin.instance.FacebookLogout();
                break;
            case LoginMode.email:
                //FacebookLogin.instance.FacebookLogout();
                break;
        }
        obj.DisablePopup();
        ScreenManager.Instance.DeactivateAllScreens();
        ScreenManager.Instance.ActivateScreen<SignInScreen>();
        GetComponent<SliderMenuAnim>().ShowHideMenu();
        PlayerPrefs.DeleteAll();
        File.WriteAllText(AppOfflineManager.instance.pathToSaveJson, "");
        AppOfflineManager.instance.fetchedInfo = new FetchedUSerInfo();
    }

    private void OnCancel(IPopup obj)
    {
        obj.DisablePopup();
    }

    public void OpenLinkInBrowser(string url)
    {
        Application.OpenURL(url);
    }

    private void OnDisable()
    {
        ObjectManager.UnRegisterCallBackObject(OnGettingUserDetails, ObjectEventVariables.userData);
    }
}