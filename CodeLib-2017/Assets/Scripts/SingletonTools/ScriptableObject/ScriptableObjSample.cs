using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScriptableObjSample : ScriptableObject
{
    /// <summary>
    /// Singleton instance of Dexmo Database
    /// </summary>
    private static ScriptableObjSample instance;

    /// <summary>
    /// Returns singleton instance of Dexmo Database
    /// </summary>
    /// <value></value>
    public static ScriptableObjSample Instance
    {
        get
        {
            if (instance == null)
            {
                CreateScriptableObject();
            }
            return instance;
        }
    }


#if UNITY_EDITOR
    [UnityEditor.MenuItem("K/CreateScriptableObject")]
#endif
    public static void CreateScriptableObject()
    {
        instance = Resources.Load<ScriptableObjSample>("ScriptableObjSample");
        if (instance == null)
        {
            Debug.Log("Create ScriptableObjSample.asset");
            instance = ScriptableObject.CreateInstance<ScriptableObjSample>();

#if UNITY_EDITOR
            // Only in the editor should we save it to disk
            string path = System.IO.Path.Combine(UnityEngine.Application.dataPath, "project/Resources");
            if (!System.IO.Directory.Exists(path))
            {
                UnityEditor.AssetDatabase.CreateFolder("Assets/project", "Resources");
            }

            string fullPath = System.IO.Path.Combine("Assets/project/Resources",
                "ScriptableObjSample.asset");
            UnityEditor.AssetDatabase.CreateAsset(instance, fullPath);
#endif
        }
    }

    /// <summary>
    /// If properties on the inspector had changed, save dexmo database please
    /// </summary>
    public void SaveData()
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }


    [SerializeField]
    private ScriptableData testData;
}


[Serializable]
public class ScriptableData
{
    public string dataName;
    public float value;
    public Vector3 vector;
}
