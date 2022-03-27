using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class Timer : MonoBehaviour
{
    public float timevalue = 10;
    public float timevalue2 = 10;
    public TMP_Text timeText;
    public TMP_Text timeText2;
    public GameObject questionOne;
    public GameObject answerOne;
    public GameObject questionTwo;
    public GameObject answerTwo;
    public bool timerStart = false;
    public bool timerStart2 = false;
    public bool trackerFound = false;

    public TMP_Text DefaultText;
    public TMP_Text DefaultText2;

    public GameObject barloading;
    public GameObject barloading2;
    public GameObject barloadinghide;
    public GameObject barloadinghide2;
    public int time;
    public int time2;

    public GameObject bg;

    void Start()
    {
        bg.SetActive(false);
        timeText.text = timevalue.ToString();

        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    void Update()
    {
        if (timerStart == true && trackerFound == true)
        {
            bg.SetActive(false);
            timevalue -= Time.deltaTime;
            timeText.text = Mathf.Round(timevalue).ToString();

        }
        if(timevalue <= 1)
        {
            bg.SetActive(true);
            barloadinghide.SetActive(false);
            DefaultText.enabled = false;
            timeText.enabled = false;
            answerOne.SetActive(false);
            questionOne.SetActive(true);
        }

        if (timerStart2 == true )
        {
            bg.SetActive(false);
            timevalue2 -= Time.deltaTime;
            timeText2.text = Mathf.Round(timevalue2).ToString();
        }
        if (timevalue2 <= 1)
        {
            bg.SetActive(true);
            barloadinghide2.SetActive(false);
            DefaultText2.enabled = false;
            timeText2.enabled = false;
            answerTwo.SetActive(false);
            questionTwo.SetActive(true);
        }
    }

    public void Onclickenter()
    {
        
        timerStart = true;
        
    }
    public void TargetFoundOne()
    {
        
        Animatebar();
        trackerFound = true;
    }

    public void NextLevelButton()
    {
        Animatebar2();
        timerStart2 = true;
    }

    public void changeMainscene()
    {
        GameManager.Instance.isGameCompleted = true;
        GameManager.Instance.EndGame("Complete", ChangeSceneOSuccess);

    }

    private void ChangeSceneOSuccess()
    {
        SceneManager.LoadScene(0);
    }

    public void MainMenu()
    {
        CheckIfCourseCompleted();

        SceneManager.LoadScene(0);
    }

    public void exit()
    {
        CheckIfCourseCompleted();

        Application.Quit();
    }

    public void Animatebar()
    {
        LeanTween.scaleX(barloading, 1, time);
    }

    public void Animatebar2()
    {
        LeanTween.scaleX(barloading2, 1, time2);
    }

    private void CheckIfCourseCompleted()
    {
        if (GameManager.Instance.isGameCompleted)
            GameManager.Instance.EndGame("Complete", success);
        else
            GameManager.Instance.EndGame("Incomplete", success);
    }

    private void success()
    {
    }
}