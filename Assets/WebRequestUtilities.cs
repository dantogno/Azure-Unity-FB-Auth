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

    public static UnityWebRequest BuildWebRequest(string url, string method, string json)
    {
        UnityWebRequest www = new UnityWebRequest(url, method);

        www.SetRequestHeader(Constants.Accept, Constants.ApplicationJson);
        www.SetRequestHeader(Constants.Content_Type, Constants.ApplicationJson);
        www.SetRequestHeader(Constants.ZumoString, Constants.ZumoVersion);

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