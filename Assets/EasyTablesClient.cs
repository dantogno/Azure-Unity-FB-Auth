using Facebook.Unity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class EasyTablesClient : MonoBehaviour 
{
    // public const string Url = "https://testmobilesdk.azurewebsites.net/api/HttpTriggerCSharp2";
    public const string Url = "http://localhost:7071/api/Insert";
    private static EasyTablesClient instance;
    private string accessToken;

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

    public void Insert<T>(T instance, Action<CallbackResponse<T>> onInsertCompleted)
        where T : EasyTablesObjectBase
    {
        StartCoroutine(InsertCoroutine<T>(instance, onInsertCompleted));
    }

    private IEnumerator InsertCoroutine<T>(T instance, Action<CallbackResponse<T>> onInsertCompleted)
        where T : EasyTablesObjectBase
    {
        // Server expects a json arrary with the format:
        // [{"access_token":"value"},{"tableName":"value"},{instanceJson}]

        // TODO: will have to append function name to URL once there are multiple functions

        if (FB.IsLoggedIn)
        {
            accessToken = Facebook.Unity.AccessToken.CurrentAccessToken.TokenString;
        }
        var tableName = typeof(T).ToString();
        string instanceJson = JsonUtility.ToJson(instance);
        string jsonArray = "[{\"access_token\": \"" + accessToken + "\"}, {\"tableName\": \"" + tableName + "\"}," + instanceJson + "]";

        using (UnityWebRequest www = WebRequestUtilities.BuildWebRequest(Url, UnityWebRequest.kHttpVerbPOST, jsonArray))
        {
            yield return www.SendWebRequest();

            var response = new CallbackResponse<T>();

            if (WebRequestUtilities.IsWWWError(www))
            {
                Debug.Log("Error: " + www.error);
                WebRequestUtilities.BuildResponseObjectOnFailure(response, www);
            }
            else if (www.downloadHandler != null) // all OK.
            {
                //let's get the new object that was created
                try
                {
                    T newObject = JsonUtility.FromJson<T>(www.downloadHandler.text);
                    Debug.Log("Got this back from the server: " + newObject.ToString());
                    response.Status = CallBackResult.Success;
                    response.Result = newObject;
                }
                catch (Exception ex)
                {
                    Debug.Log("Exception!: " + ex.ToString());
                    response.Status = CallBackResult.DeserializationFailure;
                    response.Exception = ex;
                }
                onInsertCompleted(response);
            }
        }
    }

    private void Awake()
    {
        Debug.Log("Made a new instance!");
        // If in the editor, you will need to test with a debug access token.
        // This can be found here: https://developers.facebook.com/tools/accesstoken/
#if (UNITY_EDITOR)
        Debug.Log("Using debug access token. It does expire!");
         accessToken = "EAAXtZARqNfmcBAGB47qlNPCVMeEU74tBcI8Sil9bTTLW4Qmw7PA5FL3sTZBxbOVD5jy0KxVUx7I0tjEHoCB2UjK4h9JN0krwGQ4LghWC1dcWqWmtIssbbvZA0cWrkvdrLuyOXRMdBZAvCTpZCP1q3JoIGDoGDjarZBGtp3tJEEvcBEmncUa7w0nx1C8fk6AnEZD";
#endif
    }
}