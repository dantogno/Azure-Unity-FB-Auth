using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviour 
{
    public void ClickedInsertButton()
    {
        EasyTablesClient.Instance.Insert<string>("garbageData");
    }
}