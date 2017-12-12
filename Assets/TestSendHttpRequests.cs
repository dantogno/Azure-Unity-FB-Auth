using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Facebook.Unity;
using UnityEngine.UI;

public class TestSendHttpRequests : MonoBehaviour 
{
    [SerializeField]
    private Text outputText;

    private string url = "https://testmobilesdk.azurewebsites.net/api/HttpTriggerCSharp2"; 
     // private string url = "http://localhost:7071/api/Insert";
    
    public static bool IsWWWError(UnityWebRequest www)
    {
        return www.isNetworkError || (www.responseCode >= 400L && www.responseCode <= 511L);
    }


    public void ButtonClicked()
    {
        StartCoroutine(SendJArrayTest());
    }
	private void TestJson()
    {
        string jsonString = "{\"playerId\":\"8484239823\",\"playerLoc\":\"Powai\",\"playerNick\":\"Random Nick\"}";
        Player player = JsonUtility.FromJson<Player>(jsonString);
        Debug.Log(player.playerLoc);


        /// 

        string jsonString2 = "{\"id\":\"f4002940-7d61-4e8c-ae34-b87e30589284\",\"createdAt\":\"2017-10-19T21:03:33.418Z\",\"updatedAt\":\"2017-10-19T21:03:33.433Z\",\"version\":\"AAAAAAAAB9c=\",\"deleted\":false,\"X\":10.9999924,\"Y\":-3.87123073E-05,\"Z\":241.380035}";
        CrashInfo crash = JsonUtility.FromJson<CrashInfo>(jsonString2);
        Debug.Log(crash.id);
    }
    private IEnumerator SendTestRequest()
    {
        using (UnityWebRequest www = BuildWebRequest(url, UnityWebRequest.kHttpVerbGET, "", ""))
        {
            yield return www.SendWebRequest();

            if (IsWWWError(www))
            {
                Debug.Log("Error!!!!");
            }
            else if(www.downloadHandler !=null) // all OK.
            {
                //let's get the new object that was created
                try
                {
                    var newObject = JsonUtility.FromJson<CrashInfo>(www.downloadHandler.text);
                    Debug.Log("Got this back from the server: " + newObject.ToString());
                     
                }
                catch (Exception ex)
                {
                    Debug.Log("Exception!: " + ex.ToString());
                }
            }
        }
    }

    private IEnumerator SendInsertTestRequest()
    {
        CrashInfo testCrash = new CrashInfo
        {
            x = 4f,
            y = 2f,
            z = 0f
        };

        string json = JsonUtility.ToJson(testCrash);

        using (UnityWebRequest www = BuildWebRequest(url, UnityWebRequest.kHttpVerbPOST, json, ""))
        {
            yield return www.SendWebRequest();

            if (IsWWWError(www))
            {
                Debug.Log("Error!!!!");
            }
            else if (www.downloadHandler != null) // all OK.
            {
                //let's get the new object that was created
                try
                {
                    var newObject = JsonUtility.FromJson<CrashInfo>(www.downloadHandler.text);
                    Debug.Log("Got this back from the server: " + newObject.ToString());

                }
                catch (Exception ex)
                {
                    Debug.Log("Exception!: " + ex.ToString());
                }
            }
        }
    }

    private IEnumerator SendJArrayTest()
    {
        var accessToken = Facebook.Unity.AccessToken.CurrentAccessToken.TokenString;
        //accessToken = "EAAXtZARqNfmcBACyN7QjBlrNvT9Lq082ixDlkEG218tBDVVtznVnevLB2QLVHZAb1zX924ZCsJt31v8YgytvXy6dmFL7Acah4m7Aq1aqDSG3ekySed8QMvc6YvI6ZAPB2wiGhfl9NB6D6pk7ZASr5cjYZCkt3rvR8po7lhyuRExVptDxcPkf0NpS3L79P7YB4ZD";
        string json = "[{\"access_token\": \"" + accessToken + "\"}, {\"tableName\": \"CrashInfo\"}, {\"x\": 4, \"y\": 2, \"z\": 0}]";
        //string json = "[{\"access_token\": \"EAAXtZARqNfmcBACyN7QjBlrNvT9Lq082ixDlkEG218tBDVVtznVnevLB2QLVHZAb1zX924ZCsJt31v8YgytvXy6dmFL7Acah4m7Aq1aqDSG3ekySed8QMvc6YvI6ZAPB2wiGhfl9NB6D6pk7ZASr5cjYZCkt3rvR8po7lhyuRExVptDxcPkf0NpS3L79P7YB4ZD\"}, {\"tableName\": \"CrashInfo\"}, {\"x\": 777, \"y\": 2, \"z\": 1}]";


        outputText.text += "\n JSON: " + json;
        using (UnityWebRequest www = BuildWebRequest(url, UnityWebRequest.kHttpVerbPOST, json, ""))
        {
            yield return www.SendWebRequest();

            if (IsWWWError(www))
            {
                Debug.Log("Error!!!!");
            }
            else if (www.downloadHandler != null) // all OK.
            {
                //let's get the new object that was created
                try
                {
                    var newObject = JsonUtility.FromJson<CrashInfo>(www.downloadHandler.text);
                    Debug.Log("Got this back from the server: " + newObject.ToString());

                }
                catch (Exception ex)
                {
                    Debug.Log("Exception!: " + ex.ToString());
                }
            }
        }
    }

    UnityWebRequest BuildWebRequest(string url, string method, string json, string authenticationToken)
    {
        UnityWebRequest www = new UnityWebRequest(url, method);

        www.SetRequestHeader(Constants.Accept, Constants.ApplicationJson);
        www.SetRequestHeader(Constants.Content_Type, Constants.ApplicationJson);
        www.SetRequestHeader(Constants.ZumoString, Constants.ZumoVersion);

        //if (!string.IsNullOrEmpty(authenticationToken))
        //    www.SetRequestHeader(Constants.ZumoAuth, authenticationToken.Trim());

        if (!string.IsNullOrEmpty(json))
        {
            byte[] payload = Encoding.UTF8.GetBytes(json);
            UploadHandler handler = new UploadHandlerRaw(payload);
            handler.contentType = Constants.ApplicationJson;
            www.uploadHandler = handler;
        }

        www.downloadHandler = new DownloadHandlerBuffer();

        return www;
    }

}

[Serializable]
public class CrashInfo
{
    public float x;
    public float y;
    public float z;
    public string id;
}

[Serializable]
public class Player
{

    public string playerId;
    public string playerLoc;
    public string playerNick;
}