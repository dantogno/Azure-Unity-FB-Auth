using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviour 
{
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
}