using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public struct InspectorData
    {
        public Transform Target;
        public int index;
        public float distance;
        public string text;
        public Vector3 pos;
        public Vector2 pos2D;
        public int intRange;
        public float range;
        public InspectorEnumSample enumSample;
        public Color color;
    }

    public struct InspectorEditorData
    {
        public bool isFoldout;
        public bool Toggle;
        public bool ToggleLeft;
        public bool isDisable;
    }

}

public enum InspectorEnumSample
{
    A,
    B
}

