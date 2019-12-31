using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class kMethod
{

    //---------------------------- 静态方法 -------------------------------//

   
    /// <summary>
    /// 获取 单行Item 居中分布的位置
    /// </summary>
    /// <param name="count">item总数</param>
    /// <param name="itemX">item的宽度</param>
    /// <param name="interval">item之间的距离</param>
    /// <returns></returns>
    public static float[] GetOneLineItemPosX(int count, float itemX, float interval)
    {
        float side = ((float)count / 2) - 0.5f;
        float posX = -side * (interval + itemX);

        List<float> list = new List<float>();
        for (int i = 0; i < count; i++)
        {
            list.Add(posX);
            posX += (interval + itemX);
        }

        return list.ToArray();
    }
    

    #region Unity 内部

    /// <summary>
    /// 截屏 全屏 保存为png
    /// </summary>
    /// <param name="filePath">png文件保存路径</param>
    public static void ScreenShot(string filePath)
    {
        ScreenCapture.CaptureScreenshot(filePath, 0);
    }

    /// <summary>
    /// 截屏 3D场景 指定范围Rect
    /// </summary>
    /// <param name="camera">选定的摄像机</param>
    /// <param name="rect">选定的屏幕区域</param>
    /// <param name="savePath">保存路径</param>
    /// <param name="mipmap">是否打开mipmap</param>
    /// <returns></returns>
    public static Texture2D ScreenShot(Camera camera, Rect rect, string savePath = null, bool mipmap = false)
    {
        if (camera == null) return null;

        //设置位图深度（0，16，24）, 深度不够，会出现部分模型不显示
        int depth = 24;
        //创建RenderTexture，临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, depth);
        camera.targetTexture = rt;
        camera.Render();

        //激活rt, 并读取像素存储为纹理数据
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)rt.width, (int)rt.height, TextureFormat.RGB24, mipmap);
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();
        //重置相关参数，以使用camera继续在屏幕上显示  
        camera.targetTexture = null;
        RenderTexture.active = null;
        rt.Release();
        GameObject.Destroy(rt);

        if (savePath != null)
        {
            byte[] bytes = screenShot.EncodeToPNG();
            System.IO.File.WriteAllBytes(savePath, bytes);
        }

        return screenShot;
    }

    /// <summary>
    /// 截屏 3D场景 保存为纹理数据
    /// </summary>
    /// <param name="camera">指定的摄像机</param>
    /// <param name="width">宽</param>
    /// <param name="height">高</param>
    /// <returns>贴图</returns>
    public static Texture2D ScreenShot(Camera camera, float width, float height, string savePath = null)
    {
        Rect rect = new Rect(0, 0, width, height);
        return ScreenShot(camera, rect, savePath);
    }

    #endregion

    #region string 字符串

    /// <summary>
    /// 将 列表或数组 转换成 字符串 (用于数据比较大的时候）
    /// </summary>
    /// <param name="obj">列表或数组</param>
    /// <param name="blank">间隔符号</param>
    /// <returns></returns>
    public static string List2String<T>(IEnumerable<T> obj, string blank = ",")
    {
        if (obj == null) return null;

        System.Text.StringBuilder str = new System.Text.StringBuilder();

        foreach (T item in obj)
        {
            if (str.Length <= 0)
            {
                str.Append(item);
            }
            else
            {
                str.Append(blank);
                str.Append(item);
            }
        }
        return str.ToString();
    }

    /// <summary>
    /// 将 字符串以指定的间隔符 分割到 列表 中
    /// </summary>
    /// <param name="str">要分割的字符串</param>
    /// <param name="blank">间隔符</param>
    /// <returns>字符串列表</returns>
    public static List<string> String2List(string str, string blank = ",")
    {
        if (str == null) return null;

        List<string> list = new List<string>();
        char[] blanks = blank.ToCharArray();
        string[] strs = str.Split(blanks);

        foreach (var item in strs)
        {
            list.Add(item);
        }
        return list;
    }

    public static string Dic2String<T1, T2>(IEnumerable<KeyValuePair<T1, T2>> dic, string blank = ",", string interval = "-")
    {
        if (dic == null) return string.Empty;
        System.Text.StringBuilder str = new System.Text.StringBuilder();
        foreach (var item in dic)
        {
            if (str.Length > 0)
            {
                str.Append(blank);
            }
            str.Append(item.Key);
            str.Append(interval);
            str.Append(item.Value);
        }
        return str.ToString();
    }

    public static Dictionary<int, int> String2Dic(string data, char blank = ',', char interval = '-')
    {
        Dictionary<int, int> dic = new Dictionary<int, int>();
        string[] strs = data.Split(blank);
        foreach (var item in strs)
        {
            string[] ss = item.Split(interval);
            if(ss.Length == 2)
                dic.Add( Convert.ToInt32(ss[0]),Convert.ToInt32(ss[1]));
        }
        return dic;
    }

    #endregion

}

public enum kCodeForm
{
    Default,
    UTF8,
    ASCII,
}