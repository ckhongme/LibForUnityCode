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
    /// Width of the window
    /// </summary>
    private float windowWidth;

    /// <summary>
    /// Offset value of the window
    /// </summary>
    private float windowOffset = 15;

    /// <summary>
    /// Scorll position of the window
    /// </summary>
    private Vector2 scorllPosition;


    private List<EditorWindowData> datas;

    private class EditorWindowData
    {

    }

    /// <summary>
    /// List of window scroll positions
    /// </summary>
    /// <typeparam name="EditorWindowData">test data</typeparam>
    /// <typeparam name="Vector2">Scroll position</typeparam>
    /// <returns></returns>
    private Dictionary<EditorWindowData, Vector2> scrollPositions = new Dictionary<EditorWindowData, Vector2>();

    /// <summary>
    /// If gesture data of hand is visible on the window
    /// </summary>
    /// <typeparam name="GestureHandData">Gesture data of hand</typeparam>
    /// <typeparam name="bool">If ture, data is visible on the window</typeparam>
    /// <returns></returns>
    private Dictionary<EditorWindowData, bool> dataVisible = new Dictionary<EditorWindowData, bool>();


    /// <summary>
    /// 绘制窗体
    /// </summary>
    private void OnGUI()
    {
        windowWidth = this.position.width;
        //DrawData();
    }

    //#region Draw GestureData

    ///// <summary>
    ///// Draw gesture data on the window
    ///// </summary>
    //private void DrawData()
    //{
    //    using (var scope = new EditorGUILayout.ScrollViewScope(scorllPosition))
    //    {
    //        scorllPosition = scope.scrollPosition;
    //        foreach (var data in datas)
    //        {
    //            using (var vscope = new EditorGUILayout.VerticalScope())
    //            {
    //                GUI.Box(vscope.rect, new GUIContent());
    //                DrawDataHeader(data);
    //                if (IsDrawFingerData(data))
    //                    DrawFingerData(data);
    //            }
    //        }
    //        if (delayDeleteData != null)
    //        {
    //            DexmoDatabase.Instance.GestureDatas.Gestures.Remove(delayDeleteData);
    //            delayDeleteData = null;
    //        }
    //        if (delayResetData != null)
    //        {
    //            if (previewDatas.ContainsKey(delayResetData))
    //                previewDatas[delayResetData].gestureData.Copy(new GestureHandData());
    //            delayResetData.Copy(new GestureHandData());
    //            delayResetData = null;
    //        }
    //    }
    //}

    ///// <summary>
    ///// Draw a header plane on the hand gesture data area 
    ///// </summary>
    ///// <param name="data"></param>
    //private void DrawDataHeader(EditorWindowData data)
    //{
    //    using (var scope = new EditorGUILayout.HorizontalScope())
    //    {
    //        GUI.backgroundColor = Color.gray;
    //        GUI.Box(scope.rect, "");
    //        GUI.backgroundColor = Color.white;

    //        EditorGUILayout.LabelField("TestData", GUILayout.Width(80));

    //        GUI.backgroundColor = Color.red;

    //        if (GUILayout.Button("-", GUILayout.Width(30)))
    //        {
    //            delayDeleteData = data;
    //        }
    //        GUI.backgroundColor = Color.white;
    //    }
    //}

    ///// <summary>
    ///// Returns if 
    ///// </summary>
    ///// <param name="data">Hand gesture data</param>
    ///// <returns></returns>
    //private bool IsDrawFingerData(GestureHandData data)
    //{
    //    using (var scope = new EditorGUILayout.HorizontalScope())
    //    {
    //        GUI.Box(scope.rect, "");
    //        if (!dataVisible.ContainsKey(data))
    //        {
    //            dataVisible.Add(data, false);
    //        }
    //        dataVisible[data] = EditorGUILayout.Toggle("FingerData", dataVisible[data]);
    //        return dataVisible[data];
    //    }
    //}

    ///// <summary>
    ///// Draw finger gesture data
    ///// </summary>
    ///// <param name="data">Hand gesture data</param>
    //private void DrawFingerData(GestureHandData data)
    //{
    //    using (var scope = new EditorGUILayout.HorizontalScope())
    //    {
    //        GUI.Box(scope.rect, "");
    //        if (!scrollPositions.ContainsKey(data))
    //            scrollPositions.Add(data, Vector2.zero);

    //        if (!fingerGestureVisible.ContainsKey(data))
    //            fingerGestureVisible.Add(data, new bool[5]);

    //        using (var scope2 = new EditorGUILayout.ScrollViewScope(scrollPositions[data], GUILayout.Height(EditorGUIUtility.singleLineHeight * 15)))
    //        {
    //            scrollPositions[data] = scope2.scrollPosition;
    //            SetGestureData(data);
    //            PreviewHandGesture(data);
    //        }
    //    }
    //}

    ///// <summary>
    ///// Set hand gesture data equals to data
    ///// </summary>
    ///// <param name="data">The hand gesture data which will be set</param>
    //private void SetGestureData(GestureHandData data)
    //{
    //    var showFinger = fingerGestureVisible[data];
    //    foreach (FingerType item in Enum.GetValues(typeof(FingerType)))
    //    {
    //        int index = (int)item;

    //        EditorGUILayout.BeginHorizontal();
    //        showFinger[index] = EditorGUILayout.Toggle(string.Format("              {0}", item.ToString()), showFinger[index]);
    //        if (showFinger[index])
    //        {
    //            data[item].EnableSplitAxis = EditorGUILayout.Toggle("               Split", data[item].EnableSplitAxis);
    //        }
    //        EditorGUILayout.EndHorizontal();

    //        if (showFinger[index])
    //        {
    //            var bend = data[item].Bend[JointType.MCP];

    //            EditorGUILayout.BeginHorizontal();
    //            EditorGUILayout.LabelField("                            MinBendValue");
    //            bend.ValueMin = EditorGUILayout.Slider(bend.ValueMin, 0, 1);
    //            EditorGUILayout.EndHorizontal();

    //            EditorGUILayout.BeginHorizontal();
    //            EditorGUILayout.LabelField("                            MaxBendValue");
    //            bend.ValueMax = EditorGUILayout.Slider(bend.ValueMax, 0, 1);
    //            EditorGUILayout.EndHorizontal();

    //            EditorGUILayout.Space();

    //            if (data[item].EnableSplitAxis)
    //            {
    //                var split = data[item].Split;
    //                EditorGUILayout.BeginHorizontal();
    //                EditorGUILayout.LabelField("                            MinSplitValue");
    //                split.ValueMin = EditorGUILayout.Slider(split.ValueMin, 0, 1);
    //                EditorGUILayout.EndHorizontal();

    //                EditorGUILayout.BeginHorizontal();
    //                EditorGUILayout.LabelField("                            MaxSplitValue");
    //                split.ValueMax = EditorGUILayout.Slider(split.ValueMax, 0, 1);
    //                EditorGUILayout.EndHorizontal();
    //            }
    //        }
    //    }
    //}

    //#endregion

}
