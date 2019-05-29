using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

public class Test : MonoBehaviour
{
    Rect r = new Rect(30, 30, 30,90);
    double aaa;
    int[] ak;

    private void OnGUI()
    {
       
        GUIContent gc = new GUIContent("11111");
        EditorGUILayout.DropdownButton(gc, FocusType.Keyboard);

        EditorGUI.SelectableLabel(r, "2222");

        //EditorGUI.SLider
    }
}
