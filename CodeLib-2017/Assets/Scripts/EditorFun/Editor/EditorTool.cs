using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// 编辑器助手
/// </summary>
public class EditorTool
{
    /* 完整目录 fullPath 包括磁盘目录
     * 项目目录 assetPath 不包括磁盘目录，从Assets目录开始*/

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

    public static void SelectSceneObj(GameObject go)
    {
        EditorGUIUtility.PingObject(go);
        Selection.activeGameObject = go;
    }




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

    public static T Trans2Object<T>(FileInfo fileInfo) where T: Object
    {
        string assetPath = SystemIOTool.GetFileInfoAssetPath(fileInfo);
        return LoadAsset<T>(assetPath);
    }

    public static T LoadAsset<T>(string assetPath) where T : Object
    {
        return AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object)) as T;
    }

    #endregion
}
