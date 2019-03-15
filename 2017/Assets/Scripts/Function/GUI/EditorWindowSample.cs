using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 扩展窗口
/// </summary>
public class EditorWindowSample : EditorWindow
{
    [MenuItem("K/EditorWindowSample")]
    private static void ShowEditorWindow()
    {
        EditorWindow window = EditorWindow.GetWindow<EditorWindowSample>("window name", true);
        //当unity编辑视窗有所变动，自动重画窗口
        window.autoRepaintOnSceneChange = true;                                                                 
        //可以通过position设置位置和大小
        window.position = new Rect(200, 300, 400, 400);
        //设置窗口的最大值
        window.maxSize = new Vector2(1000, 1000);
    }

    /// <summary>
    /// 当选中的物件发生变化时，聚焦该窗口
    /// </summary>
    private void OnSelectionChange()
    {
        this.Focus();
    }

    /// <summary>
    /// 绘制窗体
    /// </summary>
    private void OnGUI()
    {

    }
}
