using System;
using UnityEngine;
using UnityEngine.UI;

public class TestScene : MonoBehaviour 
{
    [SerializeField]
    private Button insertButton, getAllEntriesButton;

    [SerializeField]
    private Text outputText;

    private void Start()
    {
        // Will block buttons until logged in.
        insertButton.interactable = false;
        getAllEntriesButton.interactable = false;
    }

    #region Functions called from Unity UI buttons' OnClick
    public void PressedLoginButton()
    {
        outputText.text += "\n Logging in...";
        FacebookLogin.Instance.LogInUser();
    }
    public void ClickedInsertButton()
    {
        EasyTablesClient.Instance.Insert<TestPlayerData>(
            new TestPlayerData { name = "George", highScore = 999 }, 
            insertResponse =>
            {
                if (insertResponse.Status == CallBackResult.Success)
                {
                    string result = "Insert completed";
                    Debug.Log(result);
                    outputText.text += "\n" + result + "\n" + insertResponse.Result.name + " inserted.";
                }
                else
                {
                    Debug.Log(insertResponse.Exception.Message);
                    outputText.text += "\n" + insertResponse.Exception.Message;
                }
            }
        );
    }
    public void ClickedGetAllEntriesButton()
    {
        EasyTablesClient.Instance.GetAllEntries<TestPlayerData>
           (
               response =>
               {
                   if (response.Status == CallBackResult.Success)
                   {
                       string result = response.Result.ToString();
                       Debug.Log(result);
                       outputText.text += "\n" + "Get All Entries succeeded." + "\n" + result + 
                       " Count: " + response.Result.Count;
                   }
                   else
                   {
                       Debug.Log(response.Exception.Message);
                       outputText.text += "\n" + response.Exception.Message;
                   }
               }
           );
    }
    #endregion

    #region Event handler / subscribe and unsubscribe for FacebookLogin.LoggedIn
    private void OnFacebookLoggedIn()
    {
        insertButton.interactable = true;
        getAllEntriesButton.interactable = true;
        outputText.text += "\n Log in completed. Access token: \n" + FacebookLogin.Instance.AccessToken;
    }
    private void OnEnable()
    {
        FacebookLogin.LoggedIn += OnFacebookLoggedIn;
    }
    private void OnDisable()
    {
        FacebookLogin.LoggedIn -= OnFacebookLoggedIn;
    }
    #endregion
}

// Test data model class. Requires matching easy table on Azure.
[Serializable]
public class TestPlayerData : EasyTablesObjectBase
{
    public string name;
    public int highScore;
}