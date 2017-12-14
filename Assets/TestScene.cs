using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScene : MonoBehaviour 
{
    [SerializeField]
    private Button insertButton;

    private void Start()
    {
        insertButton.interactable = false;   
    }

    public void PressedLoginButton()
    {
        FacebookLogin.Instance.LogInUser();
    }

    public void ClickedInsertButton()
    {
        EasyTablesClient.Instance.Insert<CrashInfo>(
            new CrashInfo { x = 0, y = 0, z = 0 }, 
            insertResponse =>
            {
                if (insertResponse.Status == CallBackResult.Success)
                {
                    string result = "Insert completed";
                    Debug.Log(result);
                }
                else
                {
                    Debug.Log(insertResponse.Exception.Message);
                }
            }
        );
    }

    private void OnFacebookLoggedIn()
    {
        insertButton.interactable = true;
        EasyTablesClient.Instance.GetAllEntries<CrashInfo>
            (
                response => { }
            
            );
    }

    private void OnEnable()
    {
        FacebookLogin.LoggedIn += OnFacebookLoggedIn;
    }

    private void OnDisable()
    {
        FacebookLogin.LoggedIn -= OnFacebookLoggedIn;
    }
}