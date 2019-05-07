using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

/// <summary>
/// 编辑器助手
/// </summary>
public class EditorTool
{
    #region 获取路径

    /* 完整目录 fullPath 包括磁盘目录
     * 项目目录 assetPaht 不包括磁盘目录，从Assets目录开始*/

    /// <summary>
    /// 获取选择的路径
    /// </summary>
    /// <param name="isFullPath">是否返回完整路径</param>
    /// <returns></returns>
    public static string GetSelectionPath(bool isFullPath = false)
    {
        Object obj = Selection.activeObject;
        if (obj != null)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            if (isFullPath)
                path = string.Format("{0}/{1}", Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')), path);
            return path;
        }
        return string.Empty;
    }

    /// <summary>
    /// 获取满足条件的目标对象的路径
    /// </summary>
    /// <param name="fullPath">资源文件夹的路径（fullPath）</param>
    /// <param name="predicate">需要满足的条件</param>
    /// <param name="isFullPath">是否返回完整路径</param>
    public static List<string> GetTargetPaths(string fullPath, System.Func<string, bool> predicate, bool isFullPath = false)
    {
        List<string> paths = new List<string>();
        if (string.IsNullOrEmpty(fullPath)) return paths;

        var assetPaths = Directory.GetFiles(fullPath, "*", SearchOption.AllDirectories).Where(predicate);
        if (assetPaths != null)
        {
            foreach (var item in assetPaths)
            {
                string path = item.Replace(@"\", "/");
                if (!isFullPath)
                    path = path.Substring(path.IndexOf("Assets"));

                if (!paths.Contains(path))
                    paths.Add(path);
            }
        }
        return paths;
    }

    /// <summary>
    /// 获取目标对象的路径
    /// </summary>
    /// <param name="fullPath">资源文件夹的路径（fullPath）</param>
    /// <param name="isFullPath">是否返回完整路径</param>
    /// <param name="suffixs">指定的后缀</param>
    public static List<string> GetTargetPaths(string fullPath, bool isFullPath = false, string[] suffixs = null)
    {
        List<string> paths = new List<string>();
        if (string.IsNullOrEmpty(fullPath)) return paths;

        if (suffixs == null)
            suffixs = new string[] { string.Empty };

        foreach (var suffix in suffixs)
        {
            var assetPaths = Directory.GetFiles(fullPath, string.Format("*{0}", suffix), SearchOption.AllDirectories);
            if (assetPaths != null)
            {
                foreach (var item in assetPaths)
                {
                    string path = item.Replace(@"\", "/");
                    if (!isFullPath)
                        path = path.Substring(path.IndexOf("Assets"));

                    if (!paths.Contains(path))
                        paths.Add(path);
                }
            }
        }
        return paths;
    }

    public static List<string> GetSelectionTargetPaths(bool isFullPath = false, string[] suffix = null)
    {
        string fullPath = GetSelectionPath();
        return GetTargetPaths(fullPath, isFullPath, suffix);
    }

    #endregion

    #region 获取对象列表

    public static void GetFileInfoList(List<FileInfo> list, string[] suffixs)
    {
        string fullPath = GetSelectionPath(true);
        if (string.IsNullOrEmpty(fullPath))
        {
            Debug.Log("请选择Project的一个文件夹");
            return;
        }

        if (Directory.Exists(fullPath))
        {
            DirectoryInfo dir = new DirectoryInfo(fullPath);
            list.Clear();
            foreach (var suffix in suffixs)
            {
                var temps = dir.GetFiles(string.Format("*{0}", suffix), SearchOption.AllDirectories);
                foreach (var temp in temps)
                {
                    if (!list.Contains(temp))
                        list.Add(temp);
                }
            }
        }
    }

    #endregion

    #region AssetBundle

    /*************************** AB名字设置 *******************************/

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
        string folderPath = GetSelectionPath(true);
        List<string> paths = GetTargetPaths(folderPath);
        int count = paths.Count;
        for (int i = 0; i < paths.Count; i++)
        {
            EditorUtility.DisplayProgressBar("EditorHelper", "Clearing AssetBundle Names", 1f * i / count);
            SetABName(paths[i], null);
        }
        AssetDatabase.RemoveUnusedAssetBundleNames();
        EditorUtility.ClearProgressBar();
    }

    /*************************** 创建AB ***********************************/

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
        List<string> paths = GetTargetPaths(assetPath, false, suffixs);
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
        string path = GetSelectionPath();
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
        int mapCount = assetPath.Count();
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

    #region 格式转换

    /// <summary>
    /// 将图片的格式转成Sprite
    /// </summary>
    /// <param name="path">目标图片的路径</param>
    public static void Trans2Spt(string path)
    {
        TextureImporter texI = AssetImporter.GetAtPath(path) as TextureImporter;
        if (texI == null) return;
        if (texI.textureType != TextureImporterType.Sprite)
        {
            texI.textureType = TextureImporterType.Sprite;
            //导入指定路径的资源
            AssetDatabase.ImportAsset(path);
        }
    }

    /// <summary>
    /// FileInfo转成Object
    /// </summary>
    public static Object Trans2Object(FileInfo fileInfo)
    {
        string allPath = fileInfo.FullName;
        string assetPath = allPath.Substring(allPath.IndexOf("Assets"));
        Object obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));
        return obj;
    }

    #endregion
}
