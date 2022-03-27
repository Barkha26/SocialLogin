using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LoginMode
{
    google,
    facebook,
    email
}

public class UserInfo
{
    public string name;
    public string email;
    public string pictureUrl;
    public Texture2D picture;
    public LoginMode loginMode;

    public UserInfo(string _name, string _email, string _pitcureUrl, LoginMode loginMode, Texture2D tex = null)
    {
        this.name = _name;
        this.email = _email;
        this.pictureUrl = _pitcureUrl;
        this.picture = tex;
        this.loginMode = loginMode;
    }
}
