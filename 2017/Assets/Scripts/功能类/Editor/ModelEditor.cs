using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 模型资源管理
/// </summary>
public class ModelEditor
{
    private static string[] modelSuffix = new string[] { ".fbx" };
    private static List<string> models = new List<string>();

    /// <summary>
    /// 开启Read&Write
    /// </summary>
    [MenuItem("K/ModelEditor/OpenRead&Write")]
    private static void OpenReadWrite()
    {
        SetReadWrite(true);
    }

    /// <summary>
    /// 关闭Read&Write
    /// </summary>
    [MenuItem("K/ModelEditor/CloseRead&Write")]
    private static void CloseReadWrite()
    {
        SetReadWrite(false);
    }

    private static void SetReadWrite(bool isEnable)
    {
        models = EditorTool.GetSelectionTargetPaths(false, modelSuffix);
        for (int i = 0; i < models.Count; i++)
        {
            EditorUtility.DisplayProgressBar("SetRead&Write", "设置模型的Read&Write属性", 1f * i / models.Count);
            ModelImporter m = AssetImporter.GetAtPath(models[i]) as ModelImporter;
            m.isReadable = isEnable;
            AssetDatabase.ImportAsset(models[i]);
        }
        EditorUtility.ClearProgressBar();
    }
}
