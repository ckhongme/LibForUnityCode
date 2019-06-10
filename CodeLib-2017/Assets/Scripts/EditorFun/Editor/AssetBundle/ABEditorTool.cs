using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ABEditorTool : MonoBehaviour
{
    #region 设置名字

    /// <summary>
    /// 设置资源的assetbundle名字
    /// </summary>
    /// <param name="assetPath">资源的项目路径</param>
    /// <param name="abName">指定的ab名字</param>
    /// <param name="exception">不做修改的ab资源</param>
    public static void SetABName(string assetPath, string abName, string exception = null)
    {
        AssetImporter asset = AssetImporter.GetAtPath(assetPath);
        SetABName(asset, abName, exception);
    }

    /// <summary>
    /// 设置资源的assetbundle名字
    /// </summary>
    /// <param name="path">资源的路径</param>
    /// <param name="abName">指定的ab名字</param>
    public static void SetABName(AssetImporter asset, string abName, string exception = null)
    {
        if (asset != null)
        {
            if (exception != null && asset.assetBundleName == exception)
                return;
            asset.assetBundleName = abName;
        }
    }

    /// <summary>
    /// 清空资源的AssetBundle名字
    /// </summary>
    public static void ClearABName()
    {
        string folderPath = EditorTool.GetSelectionPath(true);
        List<string> paths = SystemTool.GetTargetPaths(folderPath);
        int count = paths.Count;
        for (int i = 0; i < paths.Count; i++)
        {
            EditorUtility.DisplayProgressBar("EditorHelper", "Clearing AssetBundle Names", 1f * i / count);
            SetABName(paths[i], null);
        }
        AssetDatabase.RemoveUnusedAssetBundleNames();
        EditorUtility.ClearProgressBar();
    }

    #endregion

    #region 创建AB

    /// <summary>
    /// 创建所有对象的assetbundle
    /// </summary>
    /// <param name="outputPath">输出路径（fullPath）</param>
    public static void BuildAllABs(string outputPath)
    {
        BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
    }

    /// <summary>
    /// 创建指定文件夹中的assetbundle
    /// </summary>
    /// <param name="assetPath">资源的项目路径</param>
    /// <param name="outputPath">输出路径（fullPath）</param>
    /// <param name="suffixs">要求的后缀</param>
    public static void BuildPartABs(string assetPath, string outputPath, string[] suffixs = null)
    {
        List<string> paths = SystemTool.GetTargetPaths(assetPath, false, suffixs);
        BuildTargetAB(paths, outputPath);
        EditorUtility.ClearProgressBar();
    }

    /// <summary>
    /// 创建选中对象的assetbundle
    /// </summary>
    /// <param name="path">对象的路径</param>
    /// <param name="outputPath">输出路径</param>
    public static void BuildSelectionAB(string outputPath)
    {
        string path = EditorTool.GetSelectionPath();
        List<string> paths = new List<string>() { path };
        BuildTargetAB(paths, outputPath);
        EditorUtility.ClearProgressBar();
    }

    /// <summary>
    /// 创建assetbundle
    /// </summary>
    /// <param name="assetPath">资源的项目路径</param>
    /// <param name="outputPath"></param>
    private static void BuildTargetAB(List<string> assetPath, string outputPath)
    {
        int mapCount = assetPath.Count;
        AssetBundleBuild[] buildMap = new AssetBundleBuild[mapCount];
        for (int i = 0; i < mapCount; i++)
        {
            AssetImporter ai = AssetImporter.GetAtPath(assetPath[i]);
            //.meta文件也可以被获取到路径，但获取不到对象，需要排除
            if (ai != null)
            {
                buildMap[i].assetBundleName = ai.assetBundleName;
                string[] subAssets = new string[] { assetPath[i] };
                buildMap[i].assetNames = subAssets;
            }
        }
        BuildPipeline.BuildAssetBundles(outputPath, buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }

    #endregion
}
