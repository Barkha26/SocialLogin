using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System.IO;
using TMPro;
using System;

public class MyLearningScreen : BaseUIScreen
{
    [SerializeField] private GameObject certificate;

    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textDescription;
    [SerializeField] private TextMeshProUGUI textCourseDirector;

    private string courseDirectorName;
    private DateTime currentDate;
    private bool takingScreenShot;

    public Sprite selected;
    public Sprite UnSelected;
    public Button learningBtn;

    public MyCertificateObject certificatePrefab;
    public Transform container;

    const string activityEndApi = "activity/get-status?id=";

    public List<MyCertificateObject> cerificateObject;

    public override void OnScreenEnabled()
    {
        learningBtn.GetComponent<Image>().sprite = selected;
        learningBtn.GetComponent<Image>().SetNativeSize();
        for (int i = 0; i < cerificateObject.Count; i++)
        {
            Destroy(cerificateObject[i].gameObject);
        }
        cerificateObject = new List<MyCertificateObject>();
        APIManager.instance.GetData(activityEndApi + AppOfflineManager.instance.fetchedInfo.data.id, OnFetchingActivitySuccess, OnFetchingActivityFail);
        ScreenManager.Instance.ActivateScreen<CommonButtonBottomScreen>();
        ScreenManager.Instance.ActivateScreen<CommonButtonTopScreen>();
    }

    private void OnFetchingActivitySuccess(object activityObj)
    {
        UnityWebRequest request = activityObj as UnityWebRequest;
        if (request.error != null || string.IsNullOrEmpty(request.downloadHandler.text))
        {
            Debug.LogError(request.error);
            return;
        }
        string json = request.downloadHandler.text;
        List<Activity> activity = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Activity>>(json);
        StartCoroutine(PopulateCertificates(activity));
        
    }

    private IEnumerator PopulateCertificates(List<Activity> activity)
    {
        Screen.orientation = ScreenOrientation.Landscape;
        for (int i = 0; i < activity.Count; i++)
        {
            if (activity[i].status == "Complete")
            {
                MyCertificateObject certificateObj = Instantiate(certificatePrefab, container);
                cerificateObject.Add(certificateObj);
                certificateObj.courseName.text = activity[i].name;
                SetCertificateProperties(activity[i].name);
                yield return new WaitForSeconds(0.5f);
                certificateObj.certificateImage.texture = CreateCertificate(activity[i].name);
                HideCertificate();
            }
        }
        Screen.orientation = ScreenOrientation.Portrait;
    }

    private void OnFetchingActivityFail(object obj)
    {
    }

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
    }

    private void OnSignIncomplete()
    {
        ScreenManager.Instance.ActivateScreen<SignInScreen>();
        Deactivate();
    }

    public override void OnScreenDisabled()
    {
        learningBtn.GetComponent<Image>().sprite = UnSelected;
        learningBtn.GetComponent<Image>().SetNativeSize();
    }

    private void SetCertificateProperties(string courseName)
    {
        certificate.SetActive(true);

        switch (courseName)
        {
            case "Fire Safety - PASS":
                courseDirectorName = "Hari Sathya";
                break;
            case "Unconscious Bias":
                courseDirectorName = "Mohammad Ashraf";
                break;
            case "Smart Operator":
                courseDirectorName = "Amey Kulkarni";
                break;
            case "Cheque Verification":
                courseDirectorName = "Mohammad Ashraf";
                break;
        }

        currentDate = DateTime.Now;

        textName.text = AppOfflineManager.instance.fetchedInfo.data.name.ToUpper();
        textDescription.text = "for the successful completion of '" + courseName + "' certification program on " + currentDate.ToString("dd/MM/yyyy");
        textCourseDirector.text = courseDirectorName;
    }

    private Texture2D CreateCertificate(string courseName)
    {
        Texture2D cerificateTex = ScreenCapture.CaptureScreenshotAsTexture();
        return cerificateTex;
    }

    private void HideCertificate()
    {
        certificate.SetActive(false);
    }
}
