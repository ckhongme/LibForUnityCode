using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public class JsonTest : MonoBehaviour
{
    #region u3d的JSON写法

    string sample = @"{""name""  : ""Bill"", 
                      ""age""   : 32,
                      ""awake"" : true,
                      ""n""     : 1994.0226,
                      ""note""  : [ ""life"", ""is"", ""but"", ""a"", ""dream"" ] }";

    #endregion

    void Start()
    {
        Test();
    }

    void Test()
    {
        a a = new a();
        b b = new b();
        c c = new c();
        d d = new d();
        d.strD = "11";
        string data = K.JsonTool.ToJson(b);
        Debug.Log(data);
        b temp = K.JsonTool.ToObject<b>(data);
    }
}


class a
{
    //私有的不会被发现
    public bool boolA;
    public char charA;
    public string strA;
    public int intA;
    public double doubleA;
    public float floatA;                 //浮点数 转不了
    public Vector3 vA;                   //向量 转不了

    public bool[] boolArrayA;
    public char[] charArrayA;
    public int[] intArrayA;         //字符串数组，int数组可以转换
    public string[] strArrayA;
    public float[] floatArrayA;

    public List<int> intlistA;
    public List<string> strlistA;                               //字符串列表、int列表可以转换 
    public List<Vector3> vListA;
    public Dictionary<string, string> dicStrA;                  //字典的话，键必须是 字符串类型

    public a()
    {
        boolA = false;
        charA = '1';
        strA = "strG";
        intA = 1;
        doubleA = 4.2;
        floatA = 1.5f;
        vA = new Vector3(1, 3, 4);

        boolArrayA = new bool[1] { false };
        charArrayA = new char[1] { 'a' };
        intArrayA = new int[3] { 11, 23, 4 };

        strArrayA = new string[1] { "aaaa" };
        floatArrayA = new float[1] { 11.33f };

        intlistA = new List<int>();
        intlistA.Add(1);
        intlistA.Add(2);

        strlistA = new List<string>();
        strlistA.Add("11");
        strlistA.Add("22");

        vListA = new List<Vector3>();
        vListA.Add(new Vector3(1, 2, 3));

        dicStrA = new Dictionary<string, string>();
        dicStrA.Add("11", "12");
        dicStrA.Add("22", "21");
    }
}

class b
{
    public TestEnum e;
    public string[] strs = new string[1] { "bbbb" };
    public c[] cs;
    public List<c> cl;
    public Dictionary<string, int> dicD;                  

    public b()
    {
        cs = new c[1];
        cs[0] = new c();

        cl = new List<c>();
        cl.Add(new c());

        dicD = new Dictionary<string, int>();
        dicD.Add("11", 12);
        dicD.Add("22", 21);
    }
}

class c
{
    public List<int> list = new List<int>();
    public int[] array = new int[1];
    public Dictionary<string, int> dic;

    public c()
    {
        list.Add(1);
        array[0] = 1;
        dic = new Dictionary<string, int>();
        dic.Add("11", 12);
        dic.Add("123", 22);
    }
}

public enum TestEnum
{
    A,
    B,
}

public struct d
{
    public string strD;
}