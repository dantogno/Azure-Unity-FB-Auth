using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System;

public class FacebookLogin : MonoBehaviour 
{
    public static event Action LoggedIn;
    private string accessToken;
    private static FacebookLogin instance;

    public static FacebookLogin Instance
    {
        get
        {
            if (instance == null)
            {
                var newGameObject = new GameObject(typeof(FacebookLogin).ToString());
                instance = newGameObject.AddComponent<FacebookLogin>();
                DontDestroyOnLoad(newGameObject);
            }

            return instance;
        }
    }
    public string AccessToken
    {
        get
        {
            return accessToken;
        }
        private set
        {
            accessToken = value;
        }
    }

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    public void LogInUser()
    {
        // If in the editor, you will need to test with a debug access token.
        // This can be found here: https://developers.facebook.com/tools/accesstoken/
#if (UNITY_EDITOR)
        Debug.Log("Using debug access token. It does expire!");
        accessToken = "EAAXtZARqNfmcBAIcWxNFo5W91xFIcCRzqCp6sEIalZBvzAU7ZCmTzXoTHjDQUXlv40rsQnytRMUi6XcuaWyN7oFHqgtcJBU3uOJ9rbkciWy17r8XnKpnlOyOHaxqLhoYd8dDoXGKQXZC9mNyoKH3oDXOIhiCjFrB2OIlEEorRnj8mwwMBbyMe0nZCZCwnqn94ZD";
        if (LoggedIn != null) LoggedIn.Invoke();
#else
        var perms = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
#endif
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            if (LoggedIn != null)
            {
                accessToken = Facebook.Unity.AccessToken.CurrentAccessToken.TokenString;
                LoggedIn.Invoke();
            }
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }
}