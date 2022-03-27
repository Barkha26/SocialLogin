using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class LoginScreen : BaseUIScreen
{
    public InputField emailInputField;
    public InputField passwordInputField;

    private UserInfo userInfo;

    private const string socialLoginEndApi = "social-login";

    public Color errorColor;
    public Color correctColor;
    private string forgotEndApi = "forgot";
    private string loginEndApi = "login";

    public override void DeviceBackButtonPressed()
    {
        ScreenManager.Instance.DeactivateAllScreens();
        ScreenManager.Instance.ActivateScreen<SignInScreen>();
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
            case "Submit_Btn":
                if (ValidateEmailId())
                    PostLogin();
                break;
            case "Forgot_Btn":
                if (ValidateEmailId())
                    PostForgotPassword();
                break;
            case "GoogleSignIn_Btn":
                GoogleSignInManager.instance.SignIn(OnLoginGetdata);
                break;

            case "FbSignIn_Btn":
                FacebookLogin.instance.Facebooklogin(OnLoginGetdata);
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

    private void OnSignIncompleteFailure(string obj)
    {

    }

    private bool ValidateEmailId()
    {
        if (string.IsNullOrEmpty(emailInputField.text))
        {
            ShowPopup("Feilds can not be empty", errorColor);
            return false;
        }
        else if (!emailInputField.text.Contains("@"))
        {
            ShowPopup("Not a valid email id", errorColor);
            return false;
        }
        else
            return true;
    }

    private void PostForgotPassword()
    {
        if(!string.IsNullOrEmpty(emailInputField.text))
        {
            AppOfflineManager.instance.fetchedInfo.data.email = emailInputField.text;
            ForgotPassword forgotpw = new ForgotPassword(emailInputField.text);
            APIManager.instance.PostData(forgotpw, forgotEndApi, OnForgetSuccess, OnForgetFailure);
        }
        else
        {
            ShowPopup("Email can't be empty", errorColor);
        }
    }

    private void OnForgetSuccess(string obj)
    {
        ObjectManager.SetBool(ObjectEventVariables.isForgetPw, true);
        ScreenManager.Instance.DeactivateAllScreens();
        ScreenManager.Instance.ActivateScreen<OTPScreen>();
    }

    private void OnForgetFailure(string obj)
    {
        ShowPopup(obj, errorColor);
    }

    private void PostLogin()
    {
        NormalLogin signInData = new NormalLogin(passwordInputField.text, emailInputField.text);
        APIManager.instance.PostData(signInData, loginEndApi, OnSignInSuccessFull, OnSignInFail);
    }

    private void OnSignInSuccessFull(string jsonRecieved)
    {
        AppOfflineManager.instance.OnFetchingLoginDetails(jsonRecieved);
        Data data = AppOfflineManager.instance.fetchedInfo.data;
        UserInfo userInfo = new UserInfo(data.name, data.email, "", LoginMode.email, null);
        ObjectManager.SetObject(ObjectEventVariables.userData, userInfo);

        ScreenManager.Instance.DeactivateAllScreens();

        if (string.IsNullOrEmpty(data.name))
            ScreenManager.Instance.ActivateScreen<UserProfileScreen>();
        else
            ScreenManager.Instance.ActivateScreen<HomeScreen>();

       
    }

    private void OnSignInFail(string obj)
    {
        ShowPopup(obj, errorColor);
    }

    private void ShowPopup(string message, Color color)
    {
        ScreenManager.Instance.errorPopups.SetPopup(message, false, false);
        ScreenManager.Instance.errorPopups.image.color = color;
        StartCoroutine(DeactivatePopup());
    }

    IEnumerator DeactivatePopup()
    {
        yield return new WaitForSeconds(3f);
        ScreenManager.Instance.errorPopups.DisablePopup();
    }

    public override void OnScreenEnabled()
    {
        emailInputField.text = string.Empty;
        passwordInputField.text = string.Empty;

    }

    public override void OnScreenDisabled()
    {
        ScreenManager.Instance.errorPopups.DisablePopup();
    }
}