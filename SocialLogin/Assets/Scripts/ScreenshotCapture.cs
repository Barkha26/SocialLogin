using System.IO;
using UnityEngine;

public class ScreenshotCapture : MonoBehaviour
{

    public static ScreenshotCapture Instance;
    
    public enum Format { RAW, JPG, PNG, PPM };
    public Format format = Format.JPG;

    private string outputFolder;
    
	private void Start()
	{
        Instance = this;
    }

	//private void CreateDirectory()
 //   {
 //       //Debug.Log("CreateDirectory: " + GameManager.Instance.sceneName);
 //       outputFolder = Application.persistentDataPath + "/Certificates/"+ GameManager.Instance.sceneName;
 //       if (!Directory.Exists(outputFolder))
 //           Directory.CreateDirectory(outputFolder);
 //   }

	//private string CreateFileName()
 //   {
 //       var filename = string.Format("{0}/Certificate.{1}", outputFolder, format.ToString().ToLower());
 //       //Debug.Log("CreateFileName: " + filename);
 //       return filename;
 //   }

    public Texture2D CaptureScreenshot()
    {
        //Debug.Log("CaptureScreenshot");
        //CreateDirectory();

        //string filename = CreateFileName();

        return ScreenCapture.CaptureScreenshotAsTexture();
        //ScreenCapture.CaptureScreenshot(Path.Combine(outputFolder, filename));
    }

}
