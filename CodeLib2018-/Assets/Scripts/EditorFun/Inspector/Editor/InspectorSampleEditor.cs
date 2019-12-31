using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine.Events;

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


        EditorGUI.BeginChangeCheck();
        InspectorSetting(_sample);

        //判断BeginChangeCheck 和 EndChangeCheck之间的代码是否有改变
        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(target);//保存对目标类的修改
    }

    private void InspectorSetting(InspectorSample sample)
    {
        EditorGUILayout.HelpBox("If you need more setting", MessageType.Warning);

        //Foldout
        sample.editorData.isSettingData = EditorGUILayout.Foldout(sample.editorData.isSettingData, "Data Setting");
        if (sample.editorData.isSettingData)
        {
            EditorGUI.indentLevel += 1;
            //ToggleLeft
            sample.editorData.isSettingObj = EditorGUILayout.ToggleLeft("Target Setting", sample.editorData.isSettingObj);
            if (sample.editorData.isSettingObj)
            {
                TargetSetting(sample);
            }

            //ToggleLeft
            sample.editorData.isEditData = EditorGUILayout.ToggleLeft("Edit Data", sample.editorData.isEditData);
            if(sample.editorData.isEditData)
            {
                DataSetting(sample);
            }
            EditorGUI.indentLevel -= 1;
        }
        //Foldout
        sample.editorData.isSettingGroup = EditorGUILayout.Foldout(sample.editorData.isSettingGroup, "Group Setting");
        if (sample.editorData.isSettingGroup)
        {
            EditorGUI.indentLevel += 1;
            sample.editorData.isDisable = EditorGUILayout.Toggle("Disable", sample.editorData.isDisable);
            ShowDisableGroup(sample.editorData.isDisable);
            EditorGUI.indentLevel -= 1;
        }

        //空行
        EditorGUILayout.Space();
        ShowFadeAnim(sample);
    }

    #region EditorGUI

    private void TargetSetting(InspectorSample sample)
    {
        EditorGUI.indentLevel += 2;
        //ObjectField
        sample.data.tfm = EditorGUILayout.ObjectField("Target", sample.data.tfm, typeof(Transform), true) as Transform;
        sample.data.mat = EditorGUILayout.ObjectField("Material", sample.data.mat, typeof(Material), true) as Material;
        sample.data.audioClip = EditorGUILayout.ObjectField("AudioClip", sample.data.audioClip, typeof(AudioClip), true) as AudioClip;
        //Texture
        sample.data.tex = EditorGUILayout.ObjectField("Texture", sample.data.tex, typeof(Texture), true) as Texture;

        EditorGUI.indentLevel -= 2;
    }

    private void DataSetting(InspectorSample sample)
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
        EditorGUI.indentLevel += 2;
        //水平组
        EditorGUILayout.BeginHorizontal();
        //显示在控件前面的标签
        EditorGUILayout.PrefixLabel("Horizontal Button Groups");
        ShowButtons();

        EditorGUILayout.EndHorizontal();
        EditorGUI.indentLevel -= 2;
    }

    private void ShowVerticalGroup()
    {
        EditorGUI.indentLevel += 2;
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

        EditorGUILayout.EndHorizontal();
        EditorGUI.indentLevel -= 2;
    }

    private void ShowDisableGroup(bool isDisable)
    {   
        //不可编辑组
        EditorGUI.BeginDisabledGroup(isDisable);

        ShowHorizontalGroup();
        ShowVerticalGroup();

        EditorGUI.EndDisabledGroup();
    }

    private void ShowScope()
    {
        //using(new EditorGUILayout.)
    }


    #endregion

    #region GUI

    public GUILayoutOption[] GetFixLayout(float width, float height)
    {
        return new GUILayoutOption[]{ GUILayout.Width(width), GUILayout.Height(height) };
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

    private void ShowSwitchBtn(ref bool flag, string str1, string str2)
    {
        if (GUILayout.Button(flag ? str1 : str2, GUILayout.Width(64)))
        {
            flag = !flag;
        }
    }

    private void ShowSingleChoice(InspectorSample sample)
    {
        if(sample.editorData.isSwitchBtn)
        {
            GUILayout.Label("ToolBar");
            sample.data.selectedIndex = GUILayout.Toolbar(sample.data.selectedIndex, new string[] { "1", "2", "3" });
            GUILayout.Label("SelectionGrid");
            sample.data.selectedIndex = GUILayout.SelectionGrid(sample.data.selectedIndex, new string[] { "1", "2", "3"}, 2);
            GUILayout.Label("PreferencesKeysElement");
            sample.data.selectedIndex = GUILayout.SelectionGrid(sample.data.selectedIndex, new string[] { "1", "2", "3" }, 1, "PreferencesKeysElement");
        }
    }

    #endregion

    private AnimFloat animFloat = new AnimFloat(0.01f);

    private void ShowFadeAnim(InspectorSample sample)
    {
        EditorGUI.BeginChangeCheck();

        ShowSwitchBtn(ref sample.editorData.isSwitchBtn, "Hide", "Show");

        if (EditorGUI.EndChangeCheck())
        {
            animFloat.target = sample.editorData.isSwitchBtn ? 1 : 0.01f;
            animFloat.speed = 2;

            var _event = new UnityEvent();
            _event.AddListener(() => Repaint());
            animFloat.valueChanged = _event;
        }

        EditorGUILayout.BeginFadeGroup(animFloat.value);
        ShowSingleChoice(sample);
        EditorGUILayout.EndFadeGroup();
    }

}

