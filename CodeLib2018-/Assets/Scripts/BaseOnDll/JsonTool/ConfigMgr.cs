using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using LitJson;

/// <summary>
/// 配置类的基类 (每个配置类必须有唯一id)
/// </summary>
public class ConfigBase { public int id; }

/// <summary>
/// 配置管理类
/// </summary>
public class ConfigMgr
{
    /// <summary>
    /// 所有配置表的字典
    /// </summary>
	private Dictionary<Type, List<ConfigBase>> allConfigDict;

    /// <summary>
    /// 初始化
    /// </summary>
	public void Init()
	{
        allConfigDict = new Dictionary<Type, List<ConfigBase>>();
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public void DoDestroy()
	{
        allConfigDict.Clear();
        allConfigDict = null;
    }

    /// <summary>
    /// 获取T的所有配置
    /// </summary>
    /// <typeparam name="T">指定的T</typeparam>
    /// <returns>配置列表</returns>
    public List<T> GetConfigs<T>() where T : ConfigBase
	{
		Type t = typeof(T);
        if (!allConfigDict.ContainsKey(t))
            _DoParse(t);

        List<T> list = new List<T>();
        foreach (ConfigBase item in allConfigDict[t])
        {
            list.Add(item as T);
        }
        return list;
	}

    /// <summary>
    /// 通过id获取唯一T
    /// </summary>
    /// <typeparam name="T">指定的T</typeparam>
    /// <param name="id">对应的id</param>
    /// <returns>单一配置</returns>
    public T GetById<T>(int id) where T : ConfigBase
	{
        List<T> list = this.GetConfigs<T>();

        T t = list.Find(item => item.id == id);
        if (t == null)
        {
            Debug.LogErrorFormat("Check Table {0}, can't have id:{1}", typeof(T), id);
        }
        return t;
	}

	/// <summary>
	/// 解析表格：将数据添加到字典中
	/// </summary>
	/// <param name="t"></param>
	private void _DoParse(Type t) 
	{
        allConfigDict.Add(t, new List<ConfigBase>());
        //获取配置表数据
        string path     = _GetConfigPath(t);
        string jsonTxt  = File.ReadAllText(path);

        //记录数据
        Dictionary<string, object> jsonData = Json.Deserialize(jsonTxt) as Dictionary<string, object>;
        foreach (var item in jsonData)
        {
            allConfigDict[t].Add(DecodeJson(t, (Dictionary<string, object>)item.Value));
        }
	}

    /// <summary>
    /// 获取配置表的路径
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private string _GetConfigPath(Type t)
    {
        string pathFormat = string.Format("Configs/{0}.json", t.ToString());
#if UNITY_EDITOR
        return string.Format("{0}/{1}", Application.streamingAssetsPath, pathFormat);
#elif UNITY_STANDALONE_WIN
        return string.Format("{0}/{1}", Application.persistentDataPath, pathFormat);
#endif
    }

    /// <summary>
    /// 解析表格的数据类型 
    /// </summary>
    /// <param name="t">对应的类型T</param>
    /// <param name="jsonData">对应的单个数据</param>
    /// <returns></returns>
    private ConfigBase DecodeJson(Type t, Dictionary<string, object> jsonData)
    {
        //动态创建类
        ConfigBase instance = (ConfigBase)Activator.CreateInstance(t);

        FieldInfo info = null;
        foreach (var item in jsonData)
        {
            info = t.GetField(item.Key);
            if (info == null) continue;

            if (typeof(int) == info.FieldType)
            {
                info.SetValue(instance, Convert.ToInt32(item.Value));
            }
            else if (typeof(float) == info.FieldType)
            {
                info.SetValue(instance, float.Parse(item.Value.ToString()));
            }
            else if (typeof(bool) == info.FieldType)
            {
                info.SetValue(instance, Convert.ToInt32(item.Value) == 1);
            }
            else if (typeof(string) == info.FieldType)
            {
                string value = item.Value.ToString();
                if (value.Equals("0")) value = "";

                info.SetValue(instance, value);
            }
            else
            {
                List<object> list = item.Value as List<object>;
                if (list == null || list.Count == 0) continue;

                if (typeof(int[]) == info.FieldType)
                {
                    int[] arr = new int[list.Count];
                    for (int i = 0; i < arr.Length; i++)
                    {
                        arr[i] = Convert.ToInt32(list[i]);
                    }
                    info.SetValue(instance, arr);
                }
                else if (typeof(float[]) == info.FieldType)
                {
                    float[] arr = new float[list.Count];
                    for (int i = 0; i < arr.Length; i++)
                    {
                        arr[i] = Convert.ToSingle(list[i]);
                    }
                    info.SetValue(instance, arr);
                }
                else if (typeof(string[]) == info.FieldType)
                {
                    string[] arr = new string[list.Count];
                    for (int i = 0; i < arr.Length; i++)
                    {
                        arr[i] = list[i].ToString();
                    }
                    info.SetValue(instance, arr);
                }
                else if (typeof(Vector3) == info.FieldType)
                {
                    Vector3 vec = Vector3.zero;
                    if (list.Count == 3)
                    {
                        vec.x = Convert.ToSingle(list[0]);
                        vec.y = Convert.ToSingle(list[1]);
                        vec.z = Convert.ToSingle(list[2]);
                    }
                    info.SetValue(instance, vec);
                }
            }
        }
        return instance;
    }
}
