using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 需要扩展属性页面的目标类
/// </summary>
public class InspectorSample : MonoBehaviour
{
    [HideInInspector]
    public bool isFoldout = true;
    [HideInInspector]
    public bool Toggle = false;
    [HideInInspector]
    public bool ToggleLeft = false;

    public float minValue;
    public float maxValue;

    [HideInInspector]
    public InspectorData data;

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

}

public enum InspectorEnumSample
{
    A,
    B
}

