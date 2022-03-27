using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResetPasswordScreen : BaseUIScreen
{
    public InputField passwordInput;
    public InputField cnfpasswordInput;

    public Color errorColor;
    public Color correctColor;

    private const string resetEndApi = "reset";

    public override void OnBackButtonPressed()
    {
        Deactivate();
    }

    public override void OnScreenDisabled()
    {
        passwordInput.text = string.Empty;
        cnfpasswordInput.text = string.Empty;
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
                if (ValidatePassword())
                    CallResetPassword();
                break;
        }
    }

    private bool ValidatePassword()
    {
        if (string.Equals(passwordInput.text, cnfpasswordInput.text))
            return true;
        else
        {
            ShowPopup("Both the passwords do not match", errorColor);
            return false;
        }
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

    private void CallResetPassword()
    {
        Debug.Log(AppOfflineManager.instance.fetchedInfo.data.email);
        Debug.Log(passwordInput.text);
        ResetPassword resetPw = new ResetPassword(AppOfflineManager.instance.fetchedInfo.data.email, passwordInput.text);
        APIManager.instance.PostData(resetPw,resetEndApi,OnResetSuccess, OnResetFail);
    }

    private void OnResetSuccess(string obj)
    {
        ScreenManager.Instance.DeactivateAllScreens();
        ScreenManager.Instance.ActivateScreen<HomeScreen>();
    }

    private void OnResetFail(string obj)
    {
        ShowPopup(obj, errorColor);
    }
}
