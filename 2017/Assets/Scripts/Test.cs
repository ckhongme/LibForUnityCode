using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Test : MonoBehaviour
{
    Rect r = new Rect(30, 30, 30,90);
    double aaa;

    private void OnGUI()
    {

        //GUI.Label(r, "gui btn");
        //GUILayout.Label("guiLayout btn");


        //GUI.SelectionGrid(r, 1, new string[] { "1", "2", "3" }, 2);
        //GUILayout.Fl
        //EditorGUI.Windo

        //EditorGUILayout.ColorField(Color.red);
        //EditorGUILayout.RectField(r);
        //aaa = EditorGUILayout.DoubleField(22);

        //EditorGUILayout.HelpBox("1111", MessageType.Info);

        //GUI.En

        GUIContent gc = new GUIContent("11111");
        EditorGUILayout.DropdownButton(gc, FocusType.Keyboard);

        EditorGUI.SelectableLabel(r, "2222");

        //EditorGUI.SLider
    }
}
