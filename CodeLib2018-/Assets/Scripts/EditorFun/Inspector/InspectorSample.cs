using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 需要扩展属性页面的目标类
/// </summary>
public class InspectorSample : MonoBehaviour
{
    public float minValue;
    public float maxValue;

    [HideInInspector]
    public InspectorData data;

    [HideInInspector]
    public InspectorEditorData editorData;

    [Serializable]
    public struct InspectorData
    {
        public Transform tfm;
        public Material mat;
        public AudioClip audioClip;
        public Texture tex;

        public int index;
        public float distance;
        public string text;
        public Vector3 pos;
        public Vector2 pos2D;
        public int intRange;
        public float range;
        public InspectorEnumSample enumSample;
        public Color color;

        public int selectedIndex;
    }

    [Serializable]
    public struct InspectorEditorData
    {
        public bool isSettingData;
        public bool isSettingObj;
        public bool isEditData;

        public bool isSettingGroup;
        public bool isDisable;
        public bool isSwitchBtn;

        public float animFloat;
    }
}

public enum InspectorEnumSample
{
    A,
    B
}

