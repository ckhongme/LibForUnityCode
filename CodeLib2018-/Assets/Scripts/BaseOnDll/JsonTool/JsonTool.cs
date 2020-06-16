/*
 * @Author: HCK
 * @Date: 2018-11-13 12:01:01
 * @Description: Provide a tool to transform json and data class (not inherits MonoBehaivour and with a not-parameter constructor). 
 */

using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.IO;

namespace K
{
    public class JsonTool
    {
        public static string ToJson(object obj)
        {
            JsonWriter wr = new JsonWriter();
            _WriteClassJson(wr, obj);
            return wr.ToString();
        }

        public static void ToJson(object obj, string path, string fileName, System.Text.Encoding encoding)
        {
            WriteJson(path, fileName, ToJson(obj), encoding);
        }

        public static T ToObject<T>(string json)
        {
            JsonData data = JsonMapper.ToObject(json);
            Type t = typeof(T);
            T instance = (T)Activator.CreateInstance(t);
            return (T)_ConvertClass(data, t, instance);
        }

        public static T ToObject<T>(string filePath, string fileName)
        {
            string json = ReadJson(filePath, fileName);
            return ToObject<T>(json);
        }

        public static void WriteJson(string path, string fileName, string content, System.Text.Encoding encoding)
        {
            if (Directory.Exists(path))
            {
                string fullPath = string.Format("{0}/{1}.json", path, fileName);
                File.WriteAllText(fullPath, content, encoding);
            }
        }

        public static string ReadJson(string path, string fileName)
        {
            path = string.Format("{0}/{1}.json", path, fileName);

            Debug.Log(File.Exists(path));
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return null;
        }

        #region ToJson

        private static void _WriteClassJson(JsonWriter wr, object obj)
        {
            wr.WriteObjectStart();
            if (obj != null)
            {
                FieldInfo[] fields = obj.GetType().GetFields();
                foreach (FieldInfo info in fields)
                {
                    wr.WritePropertyName(info.Name);
                    Type t = info.FieldType;
                    object v = info.GetValue(obj);
                    _Write(t, v, wr);
                }
            }
            wr.WriteObjectEnd();
        }

        private static void _Write(Type t, object o, JsonWriter wr)
        {
            #region Variable
            if (t == typeof(bool)) wr.Write((bool)o);
            else if (t == typeof(Vector3)) wr.Write(_Vector2Str((Vector3)o));
            else if (t == typeof(Vector2)) wr.Write(_Vector2Str((Vector2)o));
            else if (t == typeof(Quaternion)) wr.Write(_Quaternion2Str((Quaternion)o));
            else if (t == typeof(Color)) wr.Write(_Color2Str((Color)o));
            else if (t.IsValueType || t == typeof(string))  //valueType or String
            {
                if (o == null)
                    wr.Write(null);
                else
                    wr.Write(Convert.ToString(o));
            }
            #endregion
            else
            {
                IDictionary dic = o as IDictionary;
                if (dic != null)
                {
                    _WriteDic(dic, wr);
                }
                else
                {
                    ICollection list = o as ICollection;
                    if (list != null)
                    {
                        _WriteArray(o as ICollection, wr);
                    }
                    else
                    {
                        _WriteClassJson(wr, o);
                    }
                }
            }
        }

        #region write Json

        private static string _Vector2Str(Vector3 v)
        {
            return string.Format("({0},{1},{2})", v.x, v.y, v.z);
        }

        private static string _Vector2Str(Vector2 v)
        {
            return string.Format("({0},{1})", v.x, v.y);
        }

        private static string _Quaternion2Str(Quaternion q)
        {
            return string.Format("({0}, {1}, {2}, {3})", q.x, q.y, q.z, q.w);
        }

        private static string _Color2Str(Color color)
        {
            return string.Format("{0},{1},{2},{3}", color.r, color.g, color.b, color.a);
        }

        private static void _WriteArray(ICollection list, JsonWriter wr)
        {
            wr.WriteArrayStart();
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    Type t = item.GetType();
                    _Write(t, item, wr);
                }
            }
            wr.WriteArrayEnd();
        }

        /// <summary>
        /// 将字典写入Json （只支持键可转成string的类型）
        /// </summary>
        private static void _WriteDic(IDictionary dic, JsonWriter wr)
        {
            wr.WriteArrayStart();
            if (dic != null && dic.Count > 0)
            {
                foreach (var key in dic.Keys)
                {
                    wr.WriteArrayStart();
                    _Write(key.GetType(), key, wr);
                    _Write(dic[key].GetType(), dic[key], wr);
                    wr.WriteArrayEnd();
                }
            }
            wr.WriteArrayEnd();
        }

        #endregion

        #endregion

        #region ToObject

        private static object _ConvertClass(JsonData json, Type classType, object instance)
        {
            if (json == null) return instance;

            FieldInfo[] fields = classType.GetFields();
            foreach (var info in fields)
            {
                info.SetValue(instance, _Convert(info.FieldType, json[info.Name]));
            }
            return instance;
        }

        private static object _Convert(Type t, JsonData jd)
        {
            if (jd.IsArray)
            {
                if (t.IsArray)
                {
                    return _Str2Array(jd, t.GetElementType());
                }
                else if (t.IsGenericType)
                {
                    Type[] ts = t.GetGenericArguments();

                    if (ts.Length == 1)
                    {
                        return _Str2List(jd, ts[0]);
                    }
                    else if (ts.Length == 2)
                    {
                        return _Str2Dic(jd, ts[0], ts[1]);
                    }
                }
            }
            else //variable && define class
            {
                string str = jd.ToString();
                if (typeof(bool) == t) return Convert.ToBoolean(str);
                else if (typeof(char) == t) return Convert.ToChar(str);
                else if (typeof(string) == t) return Convert.ToString(str);
                else if (typeof(int) == t) return Convert.ToInt32(str);
                else if (typeof(float) == t) return Convert.ToSingle(str);
                else if (typeof(double) == t) return Convert.ToDouble(str);
                else if (typeof(Vector3) == t) return _Str2Vector3(str);//三维向量
                else if (typeof(Vector2) == t) return _Str2Vector2(str);//二维向量
                else if (typeof(Quaternion) == t) return _Str2Quaternion(str);
                else if (typeof(Color) == t) return _Str2Color(str);//颜色
                else if (t.IsEnum) return Enum.Parse(t, str);//枚举值
                else
                {
                    object o = Activator.CreateInstance(t);
                    return _ConvertClass(jd, t, o);
                }
            }
            return null;
        }

        #region read Json

        private static Vector3 _Str2Vector3(string str)
        {
            Vector3 v = new Vector3();
            str = str.Substring(1, str.Length - 2);
            string[] strs = str.Split(',');
            if (strs.Length == 3)
            {
                v.x = float.Parse(strs[0]);
                v.y = float.Parse(strs[1]);
                v.z = float.Parse(strs[2]);
            }
            return v;
        }

        private static Vector2 _Str2Vector2(string str)
        {
            Vector2 v = new Vector2();
            str = str.Substring(1, str.Length - 2);
            string[] strs = str.Split(',');
            if (strs.Length == 2)
            {
                v.x = float.Parse(strs[0]);
                v.y = float.Parse(strs[1]);
            }
            return v;
        }

        private static Quaternion _Str2Quaternion(string str)
        {
            Quaternion q = new Quaternion();
            str = str.Substring(1, str.Length - 2);
            string[] strs = str.Split(',');
            if (strs.Length == 4)
            {
                q.x = float.Parse(strs[0]);
                q.y = float.Parse(strs[1]);
                q.z = float.Parse(strs[2]);
                q.w = float.Parse(strs[3]);
            }
            return q;
        }

        private static Color _Str2Color(string str)
        {
            Color color = Color.white;
            string[] strs = str.Split(',');
            if (strs.Length == 4)
            {
                color = new Color(float.Parse(strs[0]), float.Parse(strs[2]), float.Parse(strs[2]), float.Parse(strs[3]));
            }
            return color;
        }

        private static object _Str2List(JsonData jd, Type elementType)
        {
            var genericType = typeof(List<>).MakeGenericType(elementType);     //构建出 itemType 类型的 List
            var genericList = Activator.CreateInstance(genericType);
            var addMethod = genericType.GetMethod("Add");

            if (jd != null)
            {
                foreach (JsonData item in jd)
                {
                    addMethod.Invoke(genericList, new object[] { _Convert(elementType, item) });
                }
            }
            return genericList;
        }

        private static object _Str2Array(JsonData jd, Type elementType)
        {
            int count = jd.Count;
            int index = 0;
            var array = Array.CreateInstance(elementType, count);
            foreach (JsonData item in jd)
            {
                array.SetValue(_Convert(elementType, item), index++);
            }
            return array;
        }

        private static object _Str2Dic(JsonData jd, Type elementType, Type elementType2)
        {
            Type[] typeArgs = { elementType, elementType2 };
            var genericType = typeof(Dictionary<,>).MakeGenericType(typeArgs);     //构建出 itemType 类型的 List
            var genericList = Activator.CreateInstance(genericType);
            var addMethod = genericType.GetMethod("Add");

            if (jd != null)
            {
                foreach (JsonData item in jd)
                {
                    addMethod.Invoke(genericList, new object[] { _Convert(elementType, item[0]), _Convert(elementType2, item[1]) });
                }
            }
            return genericList;
        }
        #endregion

        #endregion
    }
}
