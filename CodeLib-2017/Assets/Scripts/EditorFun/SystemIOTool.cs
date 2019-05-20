using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class SystemIOTool
{
    /// <summary>
    /// 获取满足条件的目标对象的路径
    /// </summary>
    /// <param name="fullPath">资源文件夹的路径（fullPath）</param>
    /// <param name="predicate">需要满足的条件</param>
    /// <param name="isFullPath">是否返回完整路径</param>
    public static List<string> GetTargetPaths(string fullPath, System.Func<string, bool> predicate, 
        bool isFullPath = false)
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
    public static List<string> GetTargetPaths(string fullPath, bool isFullPath = false, 
        string[] suffixs = null)
    {
        List<string> paths = new List<string>();
        if (string.IsNullOrEmpty(fullPath)) return paths;

        if (suffixs == null)
            suffixs = new string[] { string.Empty };

        foreach (var suffix in suffixs)
        {
            var assetPaths = Directory.GetFiles(fullPath, string.Format("*{0}", suffix), 
                SearchOption.AllDirectories);
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

    public static void GetFileInfoList(string fullPath, List<FileInfo> list, string[] suffixs)
    {
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

    public static string GetFileInfoAssetPath(FileInfo fileInfo)
    {
        string allPath = fileInfo.FullName;
        return  allPath.Substring(allPath.IndexOf("Assets"));
    }
}
