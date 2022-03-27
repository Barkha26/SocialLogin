using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OTPScreen : BaseUIScreen
{
    public InputField otpInputField;

    private const string otpEndApi = "confirm-email";
    private const string otpResendApi = "confirm/resend";

    [SerializeField] private Color errorColor;
    [SerializeField] private Color correctColor;

    public override void DeviceBackButtonPressed()
    {
        ScreenManager.Instance.DeactivateAllScreens();
        ScreenManager.Instance.ActivateScreen<RegistrationScreen>();
    }

    public override void OnScreenEnabled()
    {
        otpInputField.text = string.Empty;
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
            case "Next_Btn":
                CallConfirmOTP();
                break;
            case "Back_Btn":
                break;
            case "ResendOtp_Btn":
                ResendOtp();
                break;
            case "Login_Btn":
                ScreenManager.Instance.DeactivateAllScreens();
                ScreenManager.Instance.ActivateScreen<LoginScreen>();
                break;
        }
    }

    private void ResendOtp()
    {
        ResendOTP resendOtp = new ResendOTP(AppOfflineManager.instance.fetchedInfo.data.id, AppOfflineManager.instance.fetchedInfo.data.email);
        APIManager.instance.PostData(resendOtp, otpResendApi, OnOTPResendSuccess, OnOTPResendFail);
    }

    private void OnOTPResendSuccess(string obj)
    {
    }

    private void OnOTPResendFail(string obj)
    {
        ShowPopup(obj, errorColor);
    }

    private void ShowPopup(string popupMessage, Color popupColor)
    {
        ScreenManager.Instance.errorPopups.SetPopup(popupMessage, false, false);
        StartCoroutine(DeActivatePopup());
    }

    private void CallConfirmOTP()
    {
        OTPData otpData = new OTPData(AppOfflineManager.instance.fetchedInfo.data.email, int.Parse(otpInputField.text));
        APIManager.instance.PostData(otpData, otpEndApi, OnOTPSuccess, OnOTPFail);
    }

    private void OnOTPSuccess(string jsonRecieved)
    {
        AppOfflineManager.instance.OnFetchingLoginDetails(jsonRecieved);
        Data data = AppOfflineManager.instance.fetchedInfo.data;
        UserInfo userInfo = new UserInfo(data.name, data.email, "", LoginMode.email, null);
        ObjectManager.SetObject(ObjectEventVariables.userData, userInfo);
        ScreenManager.Instance.DeactivateAllScreens();
        if (ObjectManager.GetBool(ObjectEventVariables.isForgetPw))
            ScreenManager.Instance.ActivateScreen<ResetPasswordScreen>();
        else
            ScreenManager.Instance.ActivateScreen<UserProfileScreen>();
    }

    private void OnOTPFail(string errorMessage)
    {
        ScreenManager.Instance.errorPopups.SetPopup(errorMessage, false, false);
        StartCoroutine(DeActivatePopup());
    }

    IEnumerator DeActivatePopup()
    {
        yield return new WaitForSeconds(3f);
        ScreenManager.Instance.errorPopups.DisablePopup();
    }
}
