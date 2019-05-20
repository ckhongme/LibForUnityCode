using UnityEngine;
using UnityEditor;

/// <summary>
/// 属性页面扩展   （扩展类脚本需要放置在Editor文件夹下）
/// </summary>
[CustomEditor(typeof(InspectorSample))]
public class InspectorSampleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //通过target对象可以获取目标类
        InspectorSample _targetClass = target as InspectorSample;

        //TODO...修改属性页面

        //保存对目标类的修改
        EditorUtility.SetDirty(target);
    }
}

