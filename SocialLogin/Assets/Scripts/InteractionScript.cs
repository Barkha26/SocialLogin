using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionScript : MonoBehaviour
{

    [SerializeField] GameObject imageObject1;
    [SerializeField] GameObject imageObject2;
    [SerializeField] GameObject imageObject3;
    [SerializeField] GameObject imageObject4;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

	private void Update()
	{
	    if (imageObject1.activeSelf && imageObject2.activeSelf && imageObject3.activeSelf && imageObject4.activeSelf)
		{
            if (!GameManager.Instance.isGameCompleted)
            {
                GameManager.Instance.isGameCompleted = true;
                GameManager.Instance.EndGame("Complete", success);
            }
        }
	}

    private void success()
    {
    }

    public void exit()
    {
        CheckIfCourseCompleted();

        Application.Quit();
    }

    public void SceneChange()
    {
        CheckIfCourseCompleted();

        SceneManager.LoadScene(0);
    }

    private void CheckIfCourseCompleted()
    {
        if (GameManager.Instance.isGameCompleted)
            GameManager.Instance.EndGame("Complete", success);
        else
            GameManager.Instance.EndGame("Incomplete", success);
    }

}
