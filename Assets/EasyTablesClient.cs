using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class EasyTablesClient : MonoBehaviour 
{
    public const string Url = "https://testmobilesdk.azurewebsites.net/api/HttpTriggerCSharp2";

    private static EasyTablesClient instance;

    public static EasyTablesClient Instance
    {
        get
        {
            if (instance == null)
            {
                var newGameObject = new GameObject(typeof(EasyTablesClient).ToString());
                instance = newGameObject.AddComponent<EasyTablesClient>();
            }

            return instance;
        }
    }


    // TODO: add completed callback?
    public void Insert<T>(T instance)
    {
        StartCoroutine(InsertCoroutine<T>(instance));
    }

    private IEnumerator InsertCoroutine<T>(T instance)
    {
        var accessToken = Facebook.Unity.AccessToken.CurrentAccessToken.TokenString;
        string json = "[{\"access_token\": \"" + accessToken + "\"}, {\"tableName\": \"CrashInfo\"}, {\"x\": 4, \"y\": 2, \"z\": 0}]";
        using (UnityWebRequest www = WebRequestUtilities.BuildWebRequest(Url, UnityWebRequest.kHttpVerbPOST, json))
        {
            yield return www.SendWebRequest();

            if (WebRequestUtilities.IsWWWError(www))
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

    private void Awake()
    {
        Debug.Log("Made a new instance!");
    }
}