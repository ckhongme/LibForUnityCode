using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjTest : MonoBehaviour
{
    public void SaveData()
    {
        //using ScriptableObjSample to save data when some data changed;
        ScriptableObjSample.Instance.SaveData();
    }
}

