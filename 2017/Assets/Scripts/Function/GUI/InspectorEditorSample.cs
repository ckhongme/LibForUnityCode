using UnityEngine;
using UnityEditor;

/// <summary>
/// 属性页面扩展   （扩展类脚本需要放置在Editor文件夹下）
/// </summary>
[CustomEditor(typeof(TargetMonoClass))]
public class InspectorEditorSample : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //通过target对象可以获取目标类
        TargetMonoClass _targetClass = target as TargetMonoClass;

        //TODO...修改属性页面

        //保存对目标类的修改
        EditorUtility.SetDirty(target);
    }
}

/// <summary>
/// 需要扩展属性页面的目标类
/// </summary>
public class TargetMonoClass : MonoBehaviour
{

}