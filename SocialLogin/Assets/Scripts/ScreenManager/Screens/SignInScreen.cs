using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Newtonsoft.Json;

public class SignInScreen : BaseUIScreen
{
    private UserInfo userInfo;  
    private const string socialLoginEndApi = "social-login";


    public override void OnBackButtonPressed()
    {
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
            case "GoogleSignIn_Btn":
                GoogleSignInManager.instance.SignIn(OnLoginGetdata);
                
                break;

            case "FbSignIn_Btn":
                FacebookLogin.instance.Facebooklogin(OnLoginGetdata);
                break;

            case "SimpleLogin_Btn":
                ScreenManager.Instance.ActivateScreen<LoginScreen>();
                Deactivate();
                break;

            case "EmailSignUp_Btn":
                ScreenManager.Instance.ActivateScreen<RegistrationScreen>();
                Deactivate();
                break;
        }
    }
  
    private void OnLoginGetdata(object userData)
    {   
        userInfo = userData as UserInfo;
        SocialLogin user = new SocialLogin(userInfo.name, userInfo.email, FirebaseNotification.instance.firebaseToken, userInfo.pictureUrl, userInfo.loginMode.ToString());
        HamburgerScreen hamburgerScreen = ScreenManager.Instance.GetScreen<HamburgerScreen>();
        APIManager.instance.FetchImages(userInfo.pictureUrl, OnFetchingProfilePic);
        APIManager.instance.PostData(user, socialLoginEndApi, OnSignIncompleteSuccess, OnSignIncompleteFailure);
    }

    private void OnSignIncompleteFailure(string obj)
    {
        
    }

    private void OnFetchingProfilePic(Texture2D obj)
    {
        userInfo.picture = obj;
        ObjectManager.SetObject(ObjectEventVariables.userData, userInfo);
    }

    private void OnSignIncompleteSuccess(string jsonRecieved)
    {
        AppOfflineManager.instance.OnFetchingLoginDetails(jsonRecieved);
        ScreenManager.Instance.ActivateScreen<HomeScreen>();
        Deactivate();
    }

    public override void OnScreenEnabled()
    {
    }

    public override void OnScreenDisabled()
    {
    }

    public override void DeviceBackButtonPressed()
    {
        ScreenManager.Instance.signoutPopup.SetPopup("Are you sure you want to exit?", true, true, OnSubmit, OnCancel);
    }

    private void OnCancel(IPopup obj)
    {
        obj.DisablePopup();
    }

    private void OnSubmit(IPopup obj)
    {
        Application.Quit();
    }
}
