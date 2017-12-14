using Facebook.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EasyTablesClient : MonoBehaviour 
{
     private const string url = "http://localhost:7071/api/";
    //private const string url = "https://testmobilesdk.azurewebsites.net/api/";
    private static EasyTablesClient instance;

    public static EasyTablesClient Instance
    {
        get
        {
            if (instance == null)
            {
                var newGameObject = new GameObject(typeof(EasyTablesClient).ToString());
                instance = newGameObject.AddComponent<EasyTablesClient>();
                DontDestroyOnLoad(newGameObject);
            }

            return instance;
        }
    }

    public void Insert<T>(T instance, Action<CallbackResponse<T>> onInsertCompleted)
        where T : EasyTablesObjectBase
    {
        StartCoroutine(InsertCoroutine<T>(instance, onInsertCompleted));
    }

    public void GetAllEntries<T>(Action<CallbackResponse<List<T>>> onGetAllEntriesCompleted)
        where T : EasyTablesObjectBase
    {
        StartCoroutine(GetAllEntriesCoroutine(onGetAllEntriesCompleted));
    }

    private IEnumerator GetAllEntriesCoroutine<T>(Action<CallbackResponse<List<T>>> onGetAllEntriesCompleted)
        where T : EasyTablesObjectBase
    {
        string functionUrl = url + "GetAllEntries";

        // Server expects a json arrary with the format:
        // [{"access_token":"value"},{"tableName":"value"}]
        string jsonArray = string.Format("[{0}, {1}]", GetAccessTokenJson(), GetTableNameJson<T>());

        using (UnityWebRequest www = WebRequestUtilities.BuildWebRequest(functionUrl, UnityWebRequest.kHttpVerbPOST, jsonArray))
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
                    Debug.Log(www.downloadHandler.text);
                    // T newObject = JsonUtility.FromJson<T>(www.downloadHandler.text);
                    //Debug.Log("Got this back from the server: " + newObject.ToString());
                    // response.Status = CallBackResult.Success;
                    //response.Result = newObject;
                }
                catch (Exception ex)
                {
                    Debug.Log("Exception!: " + ex.ToString());
                    response.Status = CallBackResult.DeserializationFailure;
                    response.Exception = ex;
                }
                //onGetAllEntriesCompleted(response);
            }
        }
    }

    private IEnumerator InsertCoroutine<T>(T instance, Action<CallbackResponse<T>> onInsertCompleted)
        where T : EasyTablesObjectBase
    {
        string functionUrl = url + "Insert";

        // Server expects a json arrary with the format:
        // [{"access_token":"value"},{"tableName":"value"},{instanceJson}]
        string instanceJson = JsonUtility.ToJson(instance);
        string jsonArray = string.Format("[{0}, {1}, {2}]", GetAccessTokenJson(), GetTableNameJson<T>(), instanceJson);

        using (UnityWebRequest www = WebRequestUtilities.BuildWebRequest(functionUrl, UnityWebRequest.kHttpVerbPOST, jsonArray))
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
                    Debug.Log(www.downloadHandler.text);
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

    private string GetTableNameJson<T>()
    {
        return "{\"tableName\": \"" + typeof(T).ToString() + "\"}";
    }

    private string GetAccessTokenJson()
    {
        return "{\"access_token\": \"" + FacebookLogin.Instance.AccessToken + "\"}";
    }
}