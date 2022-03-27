using System;
using UnityEngine;
using System.IO;

public class AppOfflineManager : BaseUIScreen
{
    public static AppOfflineManager instance;

    private string pictureImg = "pictureImg";

    public string pathToSaveJson;
    public FetchedUSerInfo fetchedInfo;

    LoginMode mode;
    private UserInfo info;

    public bool isOnline = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
            instance = this;

        pathToSaveJson = Application.persistentDataPath + "/UserInfo.json";
        
        if (!File.Exists(pathToSaveJson))
            File.Create(pathToSaveJson);
    }

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    public void OnFetchingLoginDetails(string json)
    {
        if (File.Exists(pathToSaveJson))
            File.WriteAllText(pathToSaveJson, json);
        fetchedInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<FetchedUSerInfo>(json);
    }

    public override void OnScreenEnabled()
    {
        string json = File.ReadAllText(pathToSaveJson);
        ObjectManager.RegisterCallBackObject(ObjectEventVariables.userData, OnGettingUserDetails);
        ScreenManager.Instance.DeactivateAllScreens();
        if (string.IsNullOrEmpty(json))
        {
            ScreenManager.Instance.ActivateScreen<SignInScreen>();
        }
        else
        {
            fetchedInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<FetchedUSerInfo>(json);
            if(string.IsNullOrEmpty(fetchedInfo.data.name))
            {
                ScreenManager.Instance.ActivateScreen<UserProfileScreen>();
            }
            else
            {
                       
                ScreenManager.Instance.ActivateScreen<HomeScreen>();
                ScreenManager.Instance.ActivateScreen<CommonButtonBottomScreen>();
                ScreenManager.Instance.ActivateScreen<CommonButtonTopScreen>();
            }
            Enum.TryParse(fetchedInfo.data.mode, out mode);
            Texture2D tex = ReadTexture();
            info = new UserInfo(fetchedInfo.data.name, fetchedInfo.data.email, fetchedInfo.data.image, mode, tex);
            ObjectManager.SetObject(ObjectEventVariables.userData, info);
        }
        ScreenManager.Instance.ActivateScreen<CommonButtonBottomScreen>();
        ScreenManager.Instance.ActivateScreen<CommonButtonTopScreen>();
    }

    private void OnGettingUserDetails(object param)
    {
        info = param as UserInfo;
        if (info.picture == null)
            return;
        byte[] byteData = info.picture.EncodeToPNG();
        string base64 = Convert.ToBase64String(byteData);
        PlayerPrefs.SetString(pictureImg, base64);

        PlayerPrefs.Save();
    }

    private Texture2D ReadTexture()
    {
        Texture2D tex = new Texture2D(2, 2);
        string base64Tex = PlayerPrefs.GetString(pictureImg);

        if (!string.IsNullOrEmpty(base64Tex))
        {
            // convert it to byte array
            byte[] texByte = Convert.FromBase64String(base64Tex);
            tex = new Texture2D(2, 2);

            //load texture from byte array
            if (tex.LoadImage(texByte))
                return tex;
        }
        return tex;
    }

    private void OnDisable()
    {
        ObjectManager.UnRegisterCallBackObject(OnGettingUserDetails, ObjectEventVariables.userData);
    }

}