using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationLifeCycle : MonoBehaviour
{

	private void Start()
	{
		Screen.orientation = ScreenOrientation.LandscapeLeft;	
	}

	public void Exit()
    {
		CheckIfCourseCompleted();

		Application.Quit();
	}

    public void ReloadScene()
	{
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void MainMenu()
	{
		CheckIfCourseCompleted();

		SceneManager.LoadScene(0);
    }

	private void CheckIfCourseCompleted()
	{
		if (GameManager.Instance.isGameCompleted)
			GameManager.Instance.EndGame("Complete", Sucess);
		else
			GameManager.Instance.EndGame("Incomplete", Sucess );
	}

    private void Sucess()
    {
    }
}
