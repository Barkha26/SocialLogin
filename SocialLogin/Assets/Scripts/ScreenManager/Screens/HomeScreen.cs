using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;

public class HomeScreen : BaseUIScreen
{
    public Sprite selected;
    public Sprite UnSelected;
    public Button homeBtn;
    public Transform container;

    const string activityEndApi = "activity/get-status?id=";

    public Sprite unconciousSprite;
    public Sprite checkVerifyprite;
    public Sprite fireSafetySprite;
    public Sprite smartOperatorSprite;

    public AcivityObject activityPrefab;

    public List<GameObject> activitySceneObj;

    public override void OnBackButtonPressed()
    {
        //ScreenManager.Instance.ActivateScreen<RegistrationScreen>();
      
    }

    public override void OnButtonPressed()
    {
        string btnName = EventSystem.current.currentSelectedGameObject.name;
        ButtonPressed(btnName);
    }

    private void ButtonPressed(string btnName)
    {
        SceneManager.LoadScene(btnName);
    }

    public override void OnScreenEnabled()
    {
        homeBtn.GetComponent<Image>().sprite = selected;
        homeBtn.GetComponent<Image>().SetNativeSize();
        for (int i = 0; i < activitySceneObj.Count; i++)
        {
            Destroy(activitySceneObj[i]);
        }
        activitySceneObj = new List<GameObject>();
        APIManager.instance.GetData(activityEndApi + AppOfflineManager.instance.fetchedInfo.data.id, OnFetchingDataSuccess, OnFetchingDataFail);
        ScreenManager.Instance.ActivateScreen<CommonButtonBottomScreen>();
        ScreenManager.Instance.ActivateScreen<CommonButtonTopScreen>();
    }

    private void OnFetchingDataSuccess(object obj)
    {
        UnityWebRequest request = obj as UnityWebRequest;
        if (request.error != null || string.IsNullOrEmpty(request.downloadHandler.text))
        {
            Debug.LogError(request.error);
            return;
        }

        string json = request.downloadHandler.text;
        List<Activity> activity = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Activity>>(json);

        for (int i = 0; i < activity.Count; i++)
        {
            AcivityObject activityObj = Instantiate(activityPrefab, container);
            activityObj.gameObject.SetActive(true);
            activitySceneObj.Add(activityObj.gameObject);
            activityObj.name = activity[i].name;
            activityObj.statusButton.gameObject.name = activity[i].name;
            activityObj.activityPic.sprite = GetActivitySrite(activity[i]);
            activityObj.dscrText.text = activity[i].description;
            activityObj.subjectHeading.text = activity[i].name;
            switch (activity[i].status)
            {
                case "NotStarted":
                    SetForNotStarted(activity[i], activityObj);
                    break;

                case "Complete":
                    SetForComplete(activity[i], activityObj);
                    break;;

                case "Incomplete":
                    SetForIncomplete(activity[i], activityObj);
                    break;
            }
        }
    }

    private void OnFetchingDataFail(object obj)
    {

    }

    private void SetForNotStarted(Activity activity, AcivityObject activityObj)
    {
        activityObj.statusButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        activityObj.statusButton.transform.GetChild(0).GetComponent<Text>().text = "Not Started";
        activityObj.completedSprite.SetActive(false);
    }

    private void SetForIncomplete(Activity activity, AcivityObject activityObj)
    {
        activityObj.statusButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        activityObj.statusButton.transform.GetChild(0).GetComponent<Text>().text = "Incomplete";
        activityObj.completedSprite.SetActive(false);
    }
    private void SetForComplete(Activity activity, AcivityObject activityObj)
    {
        activityObj.statusButton.GetComponent<Image>().color = new Color32(103, 201, 113, 255);
        activityObj.statusButton.transform.GetChild(0).GetComponent<Text>().text = "Restart Course";
        activityObj.completedSprite.SetActive(true);
    }

    public override void OnScreenDisabled()
    {
        homeBtn.GetComponent<Image>().sprite = UnSelected;
        homeBtn.GetComponent<Image>().SetNativeSize();
    }
    private Sprite activitySprite;
    private Sprite GetActivitySrite(Activity activity)
    {
        switch(activity.name)
        {
            case "Fire Safety - PASS":
                activitySprite = fireSafetySprite;
                break;

            case "Unconscious Bias":
                activitySprite = unconciousSprite;
                break;

            case "Smart Operator":
                activitySprite = smartOperatorSprite;
                break;

            case "Cheque Verification":
                activitySprite = checkVerifyprite;
                break;
        }
        return activitySprite;
    }

    
}
