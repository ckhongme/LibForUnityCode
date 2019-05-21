using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

/// <summary>
/// 属性页面扩展   （扩展类脚本需要放置在Editor文件夹下）
/// </summary>
[CustomEditor(typeof(InspectorSample))]
public class InspectorSampleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //设置GUI.enable可以管理UI的可编辑性
        GUI.enabled = false;
        base.OnInspectorGUI();
        GUI.enabled = true;

        //通过target对象获取目标类
        InspectorSample _sample = target as InspectorSample;
        InspectorSetting(_sample);
        //保存对目标类的修改
        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }

    private void InspectorSetting(InspectorSample sample)
    {
        EditorGUILayout.HelpBox("If you need more setting", MessageType.Warning);
        //Foldout
        sample.editorData.isFoldout = EditorGUILayout.Foldout(sample.editorData.isFoldout, "Data Setting");
        if (sample.editorData.isFoldout)
        {
            EditorGUI.indentLevel += 1;
            //Toggle
            sample.editorData.Toggle = EditorGUILayout.Toggle("Target Setting", sample.editorData.Toggle);
            if (sample.editorData.Toggle)
            {
                EditorGUI.indentLevel += 2;
                //ObjectField
                sample.data.Target = (Transform)EditorGUILayout.ObjectField("Target", sample.data.Target, typeof(Transform), true);
                EditorGUI.indentLevel -= 2;
            }

            sample.editorData.ToggleLeft = EditorGUILayout.ToggleLeft("Edit Data", sample.editorData.ToggleLeft);
            if(sample.editorData.ToggleLeft)
            {
                //判断BeginChangeCheck 和 EndChangeCheck之间的代码是否有改变
                EditorGUI.BeginChangeCheck();
                SettingData(sample);
                if (EditorGUI.EndChangeCheck())
                {
                    Debug.Log("Data has changed");
                }
            }
            EditorGUI.indentLevel -= 1;
        }
        //空行
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Group Setting");



        ShowHorizontalGroup();
        ShowVerticalGroup();
    }

    private void SettingData(InspectorSample sample)
    {
        EditorGUI.indentLevel += 2;

        sample.data.index = EditorGUILayout.IntField("Index", sample.data.index);
        sample.data.distance = EditorGUILayout.FloatField("Distance", sample.data.distance);
        sample.data.text = EditorGUILayout.TextField("TextContent", sample.data.text);
        sample.data.pos = EditorGUILayout.Vector3Field("Position", sample.data.pos);
        sample.data.pos2D = EditorGUILayout.Vector2Field("Position2D", sample.data.pos2D);
        sample.data.intRange = EditorGUILayout.IntSlider("IntRange", sample.data.intRange, 0, 10);
        sample.data.range = EditorGUILayout.Slider("Range", sample.data.range, 0, 1);
        EditorGUILayout.MinMaxSlider("MinMaxValue", ref sample.minValue, ref sample.maxValue, 0, 1);
        sample.data.enumSample = (InspectorEnumSample)EditorGUILayout.EnumPopup("Enum Value", sample.data.enumSample);

        EditorGUI.indentLevel -= 2;
    }

    private void ShowHorizontalGroup()
    {
        EditorGUI.indentLevel += 1;
        //水平组
        EditorGUILayout.BeginHorizontal();
        //显示在控件前面的标签
        EditorGUILayout.PrefixLabel("Horizontal Button Groups");
        ShowButtons();

        EditorGUILayout.EndHorizontal();
        EditorGUI.indentLevel -= 1;
    }

    private void ShowVerticalGroup()
    {
        EditorGUI.indentLevel += 1;
        EditorGUILayout.BeginHorizontal();

        //垂直组1
        EditorGUILayout.BeginVertical();
        //显示在控件前面的标签
        EditorGUILayout.PrefixLabel("Vertical Button Groups");
        EditorGUILayout.EndVertical();
        
        //垂直组2
        EditorGUILayout.BeginVertical();
        ShowButtons();
        EditorGUILayout.EndVertical();

        EditorGUI.indentLevel -= 1;
    }

    private void ShowDisableGroup(bool isDisable)
    {   
        

        //不可编辑组
        EditorGUI.BeginDisabledGroup(isDisable);



        EditorGUI.EndDisabledGroup();
    }

    private void ShowButtons()
    {
        if (GUILayout.Button("Button1"))
        {
        }
        else if (GUILayout.Button("Button2"))
        {
        }
        else if (GUILayout.Button("Button3"))
        {
        }
    }

    private void SwitchButton(bool isSwitch)
    {
        if(GUILayout.Button(isSwitch ? "Enable":"Disable"))        
        {
            
        }
    }

    private AnimFloat animFloat = new AnimFloat(0.01f);






}

