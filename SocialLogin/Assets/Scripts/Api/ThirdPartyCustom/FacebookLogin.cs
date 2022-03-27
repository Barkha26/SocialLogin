using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using System;
using Newtonsoft.Json.Linq;

public class FacebookLogin : MonoBehaviour
{
    public static FacebookLogin instance;
    Action<object> loginAction;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void Facebooklogin(Action<object> action)
    {
        var permissions = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(permissions, AuthCallback);
        loginAction = action;
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
                Debug.Log(perm);
            GetDetail();
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }
    public void FacebookLogout()
    {
        FB.LogOut();
    }

    public void GetDetail()
    {
        FB.API("me?fields=name,email,picture", HttpMethod.GET, delegate (IGraphResult result)
        {
            if (result.Error != null)
            {
                Debug.LogError(result.Error);
            }
            else
            {
                Debug.LogError(result.RawResult);

                var userDetail = JObject.Parse(result.RawResult);
                Debug.Log(userDetail);
                string username = userDetail["name"].Value<string>();
                string email = "";
                string imageUrl = "";

                if (userDetail["email"].Value<string>()!=null)
                    email = userDetail["email"].Value<string>();                
                if(userDetail["picture"]["data"]["url"].Value<string>()!=null)
                    imageUrl = userDetail["picture"]["data"]["url"].Value<string>();

                UserInfo userInfo = new UserInfo(username, email, imageUrl, LoginMode.facebook);
                loginAction.Invoke(userInfo);
            }
        });
    }
}
