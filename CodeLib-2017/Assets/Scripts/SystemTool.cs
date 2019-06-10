using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

public class SystemTool
{
    #region SystemIO

    /// <summary>
    /// 获取文件的名称
    /// </summary>
    /// <param name="path">文件的路径</param>
    /// <param name="filter">是否保留后缀</param>
    /// <returns></returns>
    public static string GetFileName(string path, bool filter = false)
    {
        FileInfo file = new FileInfo(path);
        string name = file.Name;
        if (filter == false)
        {
            string ext = Path.GetExtension(path);
            name = name.Substring(0, name.Length - ext.Length);
        }
        return name;
    }

    /// <summary>
    /// 获取满足条件的目标对象的路径
    /// </summary>
    /// <param name="fullPath">资源文件夹的路径（包括磁盘目录）</param>
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

    /// <summary>
    /// 删除文件
    /// </summary>
    public static void DeleteFile(string path)
    {
        if (File.Exists(path))
            File.Delete(path);
    }

    public static void CreateTxt(string savePath, string fileName, string content)
    {
        string filePath = string.Format("{0}/{1}", savePath, fileName);
        FileInfo file = new FileInfo(filePath);
        StreamWriter sw = file.CreateText();
        sw.WriteLine(content);
        sw.Close();
        sw.Dispose();
    }

    public static string ReadTextFile(string filePath, string filter, System.Text.Encoding encoding)
    {
        var path = string.Format("{0}/{1}", filePath, filter);
        if (File.Exists(path))
        {
            return File.ReadAllText(path, encoding);
        }
        else
        {
            Debug.Log("--------------------> 要读取的文件不存在");
            return null;
        }
    }

    /// <summary>
    /// 文件流转字节
    /// </summary>
    /// <param name="filePath"></param>
    public static byte[] GetByteFromFile(string filePath)
    {
        if (!File.Exists(filePath)) return null;

        FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        fileStream.Seek(0, SeekOrigin.Begin);
        byte[] binary = new byte[fileStream.Length];            //创建文件长度的buffer   
        fileStream.Read(binary, 0, (int)fileStream.Length);
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        return binary;
    }

    #endregion

    /// <summary>
    /// 获取当前时间
    /// </summary>
    /// <param name="format">格式："yyyyMMddhhmmss", "MM/dd/yyyy HH:mm:ss"</param>
    public static string GetCurrTime(string format = "")
    {
        if (!format.Equals(""))
        {
            //"yyyyMMddhhmmss", "MM/dd/yyyy HH:mm:ss"
            return System.DateTime.Now.ToString(format);
        }
        return System.DateTime.Now.ToString();
    }

    /// <summary>
    /// 调用打印机
    /// </summary>
    /// <param name="filePath">要打印文件的路径</param>
    public static void Printer(string filePath)
    {
        if (!File.Exists(filePath)) return;
        try
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();  //系统进程

            //不显示调用程序窗口
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            process.StartInfo.UseShellExecute = true;  //采用操作系统自动识别系统
            process.StartInfo.FileName = filePath;         //要打印的文件的路径
            process.StartInfo.Verb = "print";          //打印
            process.Start();
        }
        catch (Exception ex)
        {
            Debug.Log("打印机连接有误" + ex.Message);
        }
    }

}
