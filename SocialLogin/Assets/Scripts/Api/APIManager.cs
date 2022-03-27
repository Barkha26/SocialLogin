using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;
using System.IO;

public class APIManager : MonoBehaviour
{
    public static APIManager instance;

    const string baseUrl = "http://128.199.27.184:3030/";
    
    const string activityEndApi = "activity/get-status?id=";
    const string updateActivityEndApi = "activity/update?=";


    const string noInternetHeading = "No Internet Connection";
    const string noInternetMessage = "Check you internet connection and try again.";

    public GameObject loader;
    public GameObject noInternetPopup;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
            instance = this;

    }

    private void Update()
    {
        if (noInternetPopup != null)
		{
            if (Application.internetReachability == NetworkReachability.NotReachable)
                noInternetPopup.SetActive(true);
            else
                noInternetPopup.SetActive(false);
        }
    }

   /// <summary>
   /// Call this method with a data set to post data
   /// </summary>
   /// <param objecttype="data"></param>
   /// <param apitype="endApi"></param>
   /// <param successAction="successcallback"></param>
   /// <param failaction="errorcallback"></param>
    public void PostData(object data, string endApi, Action<string> successcallback, Action<string> errorcallback)
    {
        string json = JsonConvert.SerializeObject(data);
        StartCoroutine(CallAPIBodyType(baseUrl + endApi, json, "POST", successcallback, errorcallback));

    }

    public void GetData(string endApi, Action<object> SuccessCallback, Action<object> FailureCallback)
    {
        StartCoroutine(CallAPIGetType(baseUrl + endApi, SuccessCallback, FailureCallback));
    }

    ///// <summary>
    ///// To Fetch news feeds
    ///// </summary>
    ///// <param name="SuccessCallback"></param>
    //public void FetchNewsFeed(Action<object> SuccessCallback, Action<object> FailureCallback)
    //{
    //    StartCoroutine(CallAPIGetType(baseUrl + newsFeedEndApi, SuccessCallback, FailureCallback));
    //}

    /// <summary>
    /// To Fetch Activity feeds
    /// </summary>
    /// <param name="SuccessCallBack"></param>
    //public void FetchActivityStatus(Action<object> SuccessCallBack, Action<object> FailCallback)
    //{
    //    string extendedUrl = activityEndApi + AppOfflineManager.instance.fetchedInfo.data.id;
    //    StartCoroutine(CallAPIGetType(baseUrl + extendedUrl, SuccessCallBack, FailCallback));
    //}

    /// <summary>
    /// To fetch images from any image Url
    /// </summary>
    /// <param name="url"></param>
    /// <param name="action"></param>
    public void FetchImages(string url, Action<Texture2D> action)
    {
        StartCoroutine(CallAPITextureType(url, action));
    }

    /// <summary>
    /// Update activity status with current data
    /// </summary>
    /// <param name="courseName"></param>
    /// <param name="status"></param>
    /// <param name="successCallback"></param>
    public void UpdateActivityStatus(string courseName, string status, Action<object> successCallback, Action<object> errorCallback)
    {
        UpdateActivityStatus activityStatus = new UpdateActivityStatus
        {
            user_id = AppOfflineManager.instance.fetchedInfo.data.id,
            name = courseName,
            status = status
        };
        string json = JsonConvert.SerializeObject(activityStatus);
        StartCoroutine(CallAPIBodyType(baseUrl + updateActivityEndApi, json, "POST", successCallback, errorCallback));
    }

    /// <summary>
    /// It's a get type API call
    /// </summary>
    /// <param name="url"></param>
    /// <param name="success"></param>
    /// <returns></returns>
    public IEnumerator CallAPIGetType(string url, Action<object> success, Action<object> fail)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            loader.SetActive(true);
            yield return request.SendWebRequest();
            loader.SetActive(false);

            if (request.error != null)
                fail.Invoke(request);
            else
            {
                //Debug.Log(request.downloadHandler.text);
                success.Invoke(request);
                ObjectManager.Triggerevent(ObjectEventVariables.responseRecieved);
            }
        }
    }
   
    /// <summary>
    /// Call this API to download textures from an URL
    /// </summary>
    /// <param name="url"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public IEnumerator CallAPITextureType(string url, Action<Texture2D> action)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            loader.SetActive(true);
            yield return request.SendWebRequest();
            loader.SetActive(false);

            if (request.error != null)
                Debug.LogError(request.error);
            else
            {
                Texture2D myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                action.Invoke(myTexture);
            }
        }
    }

    /// <summary>
    /// Call this API for Body type(JSON)
    /// </summary>
    /// <param name="url"></param>
    /// <param name="logindataJsonString"></param>
    /// <param name="ApiMethod"></param>
    /// <param name="succesCallback"></param>
    /// <returns></returns>
    public IEnumerator CallAPIBodyType(string url, string logindataJsonString, string ApiMethod, Action<string> succesCallback, Action<string> errorCallback)
    {
        UnityWebRequest request = new UnityWebRequest(url, ApiMethod);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(logindataJsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        loader.SetActive(true);
        yield return request.SendWebRequest();
        loader.SetActive(false);
        if (request.error != null)
            errorCallback.Invoke(request.downloadHandler.text);
        else
        {
            succesCallback.Invoke(request.downloadHandler.text);
        }
    }
}

[Serializable]
public class EmaiSignUpRegisteration
{
    public string password;
    public string email;
    public EmaiSignUpRegisteration(string _password, string _email)
    {
        password = _password;
        email = _email;
    }
}

[Serializable]
public class OTPData
{
    public string email;
    public int otp;

    public OTPData(string email, int otp)
    {
        this.email = email;
        this.otp = otp;
    }
}

public class NormalLogin
{
    public string password;
    public string email;

    public NormalLogin(string _password, string _email)
    {
        password = _password;
        email = _email;
    }
}

public class ForgotPassword
{
    public string email;
    public ForgotPassword(string _email)
    {
        email = _email;
    }
}

public class ResetPassword
{
    public string email;
    public string password;
    public ResetPassword(string _email, string _password)
    {
        email = _email;
        password = _password;
    }
}

public class ResendOTP
{
    public string id;
    public string email;
    public ResendOTP(string _id, string _email)
    {
        email = _email;
        id = _id;
    }
}

public class UpdateUserInfo
{
    public string id;
    public string name;
    public string company;
    public string phone;

    public UpdateUserInfo(string id, string name, string company, string phone)
    {
        this.id = id;
        this.name = name;
        this.company = company;
        this.phone = phone;
    }
}

[Serializable]
public class SocialLogin
{
    public string name;
    public string email;
    public string image;
    public string token;
    public string mode;

    public SocialLogin(string _name, string _email, string _firebaseToken, string _imgUrl, string _loginMode)
    {
        name = _name;
        email = _email;
        image = _imgUrl;
        token = _firebaseToken;
        mode = _loginMode;
    }
}

[Serializable]
public class FetchedUSerInfo
{
    public int status;
    public string message;
    public Data data;
    public string token;
}

[Serializable]
public class Data
{
    public string id;
    public string name;
    public string email;
    public string password;
    public string phone;
    public string bio;
    public string image;
    public bool reward_claimed;
    public bool reward_notified;
    public string company_name;
    public string token;
    public string mode;
    public double created_at;
    public string confirmed;
    public int otp;
}

[Serializable]
public class NewsFeed
{
    public string id;
    public string title;
    public string description;
    public string type;
    public string url;
    public string media;
    public string created_at;
}

[Serializable]
public class UpdateActivityStatus
{
    public string user_id;
    public string name;
    public string status;
}