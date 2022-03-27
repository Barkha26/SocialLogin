using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Messaging;
using UnityEngine;

public class FirebaseNotification : MonoBehaviour
{
    public static FirebaseNotification instance;
    public bool haveWonReward;
    public string firebaseToken;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        FirebaseMessaging.TokenReceived += OnTokenReceived;
        FirebaseMessaging.MessageReceived += OnMessageReceived;
    }

    public void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Debug.Log("OnMessageReceived"+ e.Message.From);
        haveWonReward = true;
    }

    public void OnTokenReceived(object sender, TokenReceivedEventArgs e)
    {
        firebaseToken = e.Token;
        Debug.Log("OnTokenReceived" + e.Token);
    }
}
