using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptableObjSample))]
public class ScriptableObjEditor : Editor
{
    private bool isEnableEdit = false;

    protected override void OnHeaderGUI()
    {
        base.OnHeaderGUI();
        //add a button to edit the data
        if (GUI.Button(new Rect(45, 22, 50, 18), isEnableEdit ? "Apply" : "Editor"))
        {
            isEnableEdit = !isEnableEdit;
            ScriptableObjSample.Instance.SaveData();
        }
    }

    /// <summary>
    /// Implement this function to make a custom inspector.
    /// </summary>
    public override void OnInspectorGUI()
    {
        GUI.enabled = isEnableEdit;
        base.OnInspectorGUI();
    }
}
