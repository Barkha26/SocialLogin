using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UserProfileScreen : BaseUIScreen
{
    public InputField nameInputField;
    public InputField companyInputField;
    public InputField phoneInputField;

    public Color errorColor;
    public Color correctColor;

    private const string updateUserEndApi = "user/update";

    public override void OnButtonPressed()
    {
        string btnName = EventSystem.current.currentSelectedGameObject.name;
        ButtonPressed(btnName);
    }

    private void ButtonPressed(string btnName)
    {
        switch (btnName)
        {
            case "Save_Btn":
                if(ValidateInputFields())
                    PostUserUpdate();
                break;
        }
    }

    private void PostUserUpdate()
    {
        Data data = AppOfflineManager.instance.fetchedInfo.data;
        UpdateUserInfo updateUserInfo = new UpdateUserInfo(data.id, nameInputField.text, companyInputField.text,phoneInputField.text);
        APIManager.instance.PostData(updateUserInfo, updateUserEndApi, OnUpdateSuccessfull, OnUpdateFail);
    }

    private void OnUpdateSuccessfull(string jsonRecieved)
    {
        AppOfflineManager.instance.OnFetchingLoginDetails(jsonRecieved);
        Data data = AppOfflineManager.instance.fetchedInfo.data;
        UserInfo userInfo = new UserInfo(data.name, data.email, "", LoginMode.email, null);
        ObjectManager.SetObject(ObjectEventVariables.userData, userInfo);
        ScreenManager.Instance.DeactivateAllScreens();
        ScreenManager.Instance.ActivateScreen<HomeScreen>();
    }

    private bool ValidateInputFields()
    {
        if (string.IsNullOrEmpty(nameInputField.text))
        {
            ShowPopup("Name is mandatory", errorColor);
            return false;
        }
        else
            return true;
    }

    private void OnUpdateFail(string obj)
    {
        ShowPopup(obj, errorColor);
    }

    private void ShowPopup(string message, Color color)
    {
        ScreenManager.Instance.errorPopups.SetPopup(message, false, false);
        ScreenManager.Instance.errorPopups.GetComponent<Image>().color = color;
        StartCoroutine(DeactivatePopup());
    }

    IEnumerator DeactivatePopup()
    {
        yield return new WaitForSeconds(3f);
        ScreenManager.Instance.errorPopups.DisablePopup();
    }

    public override void OnScreenEnabled()
    {
        nameInputField.text = string.Empty;
        phoneInputField.text = string.Empty;
        companyInputField.text = string.Empty;
        ShowPopup("Voila! Password created successfully.", correctColor);
    }

    public override void OnScreenDisabled()
    {
        ScreenManager.Instance.errorPopups.DisablePopup();
    }
}
