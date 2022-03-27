using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	[SerializeField] private GameObject certificate;

	[SerializeField] private TextMeshProUGUI textName;
	[SerializeField] private TextMeshProUGUI textDescription;
	[SerializeField] private TextMeshProUGUI textCourseDirector;

	public static GameManager Instance;

	[HideInInspector]
	public bool isGameCompleted;
	[HideInInspector]
	public string sceneName;
	
	private string courseDirectorName;
	private DateTime currentDate;
    Action updateScene;
	private void Start()
	{
		Instance = this;

		isGameCompleted = false;

		sceneName = SceneManager.GetActiveScene().name;
	}

	public void EndGame(string status,Action success)
	{
        //if (status == "Complete")
        //	CreateCertificate();
        updateScene = success;
        APIManager.instance.UpdateActivityStatus(sceneName, status, OnUpdateAcivityStatusSuccess, OnUpdateAcivityStatusFail);
	}

	private void OnUpdateAcivityStatusSuccess(object obj)
	{
        Debug.Log("Success");
        updateScene.Invoke();

    }

	private void OnUpdateAcivityStatusFail(object obj)
	{
        Debug.Log("Fail: " + obj);
    }

    private void CreateCertificate()
	{
		certificate.SetActive(true);

		currentDate = DateTime.Now;

		textName.text = AppOfflineManager.instance.fetchedInfo.data.name.ToUpper();
		textDescription.text = "for the successful completion of '" + sceneName + "' certification program on " + currentDate.ToString("dd/MM/yyyy");

		if (sceneName == "Fire Safety - PASS")
			courseDirectorName = "Hari Sathya";
		else if (sceneName == "Smart Operator")
			courseDirectorName = "Amey Kulkarni";
		else if (sceneName == "Unconscious Bias")
			courseDirectorName = "Mohammad Ashraf";

		textCourseDirector.text = courseDirectorName;

		ScreenshotCapture.Instance.CaptureScreenshot();

		StartCoroutine(HideCertificate());
	}

	private IEnumerator HideCertificate()
	{
		yield return new WaitForSeconds(3.0f);
		//Debug.Log("HideCertificate " + certificate);
		certificate.SetActive(false);
	}

}
