using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;


public class RegistrationScreen : BaseUIScreen
{
    public InputField emailInput;
    public InputField passwordInput;
    private UserInfo userInfo;

    public Color errorColor;
    public Color correctColor;

    private const string registerEndApi = "register";
    private const string socialLoginEndApi = "social-login";


    public override void DeviceBackButtonPressed()
    {
        ScreenManager.Instance.DeactivateAllScreens();
        ScreenManager.Instance.ActivateScreen<SignInScreen>();
        
    }

    public override void OnBackButtonPressed()
    {
        Deactivate();
    }

    public override void OnScreenEnabled()
    {
        emailInput.text = string.Empty;
        passwordInput.text = string.Empty;
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

            case "Register_Btn":
                if (ValidateEmailId())
                    PostUserRegisterData();
                break;

            case "Login_txt_Btn":
                ScreenManager.Instance.ActivateScreen<LoginScreen>();
                Deactivate();
                break;
        }
    }

    private void OnLoginGetdata(object userData)
    {
        userInfo = userData as UserInfo;
        Debug.LogWarning(userInfo + userInfo.name + "name : " + userInfo.email + "email");
        SocialLogin user = new SocialLogin(userInfo.name, userInfo.email, FirebaseNotification.instance.firebaseToken, userInfo.pictureUrl, userInfo.loginMode.ToString());
        APIManager.instance.FetchImages(userInfo.pictureUrl, OnFetchingProfilePic);
        APIManager.instance.PostData(user, socialLoginEndApi, OnSignIncompleteSuccess, OnSignIncompleteFailure);
    }

    private void OnSignIncompleteSuccess(string jsonRecieved)
    {
        AppOfflineManager.instance.OnFetchingLoginDetails(jsonRecieved);
        ScreenManager.Instance.DeactivateAllScreens();
        ScreenManager.Instance.ActivateScreen<HomeScreen>();
    }

    private void OnSignIncompleteFailure(string obj)
    {
    }


    private void OnFetchingProfilePic(Texture2D obj)
    {
        userInfo.picture = obj;
        ObjectManager.SetObject(ObjectEventVariables.userData, userInfo);
    }

    private void PostUserRegisterData()
    {
        EmaiSignUpRegisteration registerationData = new EmaiSignUpRegisteration(passwordInput.text, emailInput.text);
        APIManager.instance.PostData(registerationData, registerEndApi, OnSignUpSucessful, OnSignUpFailure);
    }

    private void OnSignUpSucessful(string jsonRecieved)
    {
        Debug.LogWarning(jsonRecieved + " jsonRecived");
        AppOfflineManager.instance.OnFetchingLoginDetails(jsonRecieved);
        Data data = AppOfflineManager.instance.fetchedInfo.data;
        UserInfo userInfo = new UserInfo(data.name, data.email, "", LoginMode.email, null);
        ObjectManager.SetObject(ObjectEventVariables.userData, userInfo);
        ScreenManager.Instance.DeactivateAllScreens();
        ScreenManager.Instance.ActivateScreen<OTPScreen>();
        ObjectManager.SetBool(ObjectEventVariables.isForgetPw, false);
        Deactivate();
    }

    private void OnSignUpFailure(string errorMessage)
    {
        if (errorMessage == "email exists, please login")
        {
            ScreenManager.Instance.errorPopups.SetPopup(errorMessage, false, false);
            StartCoroutine(DeactivatePopup());
        }
    }

    IEnumerator DeactivatePopup()
    {
        yield return new WaitForSeconds(3f);
        ScreenManager.Instance.errorPopups.DisablePopup();
    }

    private bool ValidateEmailId()
    {
        if(string.IsNullOrEmpty(emailInput.text) || string.IsNullOrEmpty(passwordInput.text))
        {
            ShowPopup("Feilds can not be empty",errorColor);
            return false;
        }
        else if (!emailInput.text.Contains("@"))
        {
            ShowPopup("Not a valid email id", errorColor);
            return false;
        }
        else
            return true;
    }

    private void ShowPopup(string popupMessage, Color popupColor)
    {
        ScreenManager.Instance.errorPopups.SetPopup(popupMessage, false, false);
        StartCoroutine(DeActivatePopup());
    }

    private IEnumerator DeActivatePopup()
    {
        yield return new WaitForSeconds(3f);
        ScreenManager.Instance.errorPopups.DisablePopup();
    }

    public override void OnScreenDisabled()
    {
        ScreenManager.Instance.errorPopups.DisablePopup();
    }
}
