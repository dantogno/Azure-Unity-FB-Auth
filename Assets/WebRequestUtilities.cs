using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class WebRequestUtilities 
{
    public static bool IsWWWError(UnityWebRequest www)
    {
        return www.isNetworkError || (www.responseCode >= 400L && www.responseCode <= 511L);
    }


    /// <summary>
    /// Builds and returns a UnityWebRequest object
    /// </summary>
    /// <param name="url">Url to hit</param>
    /// <param name="method">POST,GET, etc.</param>
    /// <param name="json">Any JSON to send</param>
    /// <returns>A UnityWebRequest object</returns>
    public static UnityWebRequest BuildWebRequest(string url, string method, string json)
    {
        var www = new UnityWebRequest(url, method);

        www.SetRequestHeader(Constants.Accept, Constants.ApplicationJson);
        www.SetRequestHeader(Constants.Content_Type, Constants.ApplicationJson);
        www.SetRequestHeader(Constants.ZumoString, Constants.ZumoVersion);

        if (!string.IsNullOrEmpty(json))
        {
            byte[] payload = Encoding.UTF8.GetBytes(json);
            var handler = new UploadHandlerRaw(payload);
            handler.contentType = Constants.ApplicationJson;
            www.uploadHandler = handler;
        }

        www.downloadHandler = new DownloadHandlerBuffer();

        return www;
    }

    public static void BuildResponseObjectOnFailure(CallbackResponse response, UnityWebRequest www)
    {
        if (www.responseCode == 404L)
            response.Status = CallBackResult.NotFound;
        else if (www.responseCode == 409L)
            response.Status = CallBackResult.ResourceExists;
        else
            response.Status = CallBackResult.Failure;

        string errorMessage = www.error;
        if (errorMessage == null && www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.text))
            errorMessage = www.downloadHandler.text;
        else
            errorMessage = Constants.ErrorOccurred;

        Exception ex = new Exception(errorMessage);
        response.Exception = ex;
    }
}