using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// 数组助手类（工具类）
/// </summary>

public class ArrayHelper
{
    /// <summary>用于查找时的比较条件委托
    /// </summary>
    /// <typeparam name="T">要比较的对象类型</typeparam>
    /// <param name="obj">要比较的对象</param>
    /// <returns>比较的结束</returns>
    public delegate bool FindHandler<T>(T obj);
    
    /// <summary>在T类型对象中，提取出TKey类型的结果
    /// </summary>
    /// <param name="obj">原对象</param>
    /// <returns>从对象中提出出来的对象</returns>
    public delegate TKey SelectHandler<T, TKey>(T obj);

    /// <summary>查找满足条件的单个对象
    /// </summary>
    /// <param name="array">源数组</param>
    /// <param name="handler">匹配条件</param>
    /// <returns>查找到的结果</returns>
    public static T Find<T>(T[] array, FindHandler<T> handler)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (handler(array[i]))
                return array[i];
        }
        return default(T);
    }

    /// <summary>查找满足条件的所有对象
    /// </summary>
    /// <param name="array">源数组</param>
    /// <param name="handler">匹配条件</param>
    /// <returns>查找到的结果</returns>
    public static T[] FindAll<T>(T[] array, FindHandler<T> handler)
    {
        List<T> list = new List<T>();
        for (int i = 0; i < array.Length; i++)
        {
            if (handler(array[i]))
                list.Add(array[i]);
        }
        return list.Count > 0 ? list.ToArray() : null;
    }

    //从数组中找出最大的元素，相当于比较条件为 p=>p 的情况
    public static T Max<T>(T[] array)
        where T : IComparable, IComparable<T>
    {
        T max = array[0];
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].CompareTo(max) > 0)
                max = array[i];
        }
        return max;
    }

    /// <summary>按照比较条件从数组中找出最大的元素
    /// </summary>
    /// <typeparam name="T">对象数据类型</typeparam>
    /// <typeparam name="TKey">比较大小的关键数据的类型</typeparam>
    /// <param name="array">源数组</param>
    /// <param name="handler">提取比较关键数据的委托</param>
    /// <returns>按比较关键字找出的最大对象</returns>
    public static T Max<T, TKey>(T[] array, SelectHandler<T, TKey> handler)
        where TKey : IComparable, IComparable<TKey>
    {
        T max = array[0];
        for (int i = 0; i < array.Length; i++)
        {
            if (handler(array[i]).CompareTo(handler(max)) > 0)
                max = array[i];
        }
        return max;
    }

    /// <summary>按照比较条件从数组中找出最小的元素
    /// </summary>
    /// <typeparam name="T">对象数据类型</typeparam>
    /// <typeparam name="TKey">比较大小的关键数据的类型</typeparam>
    /// <param name="array">源数组</param>
    /// <param name="handler">提取比较关键数据的委托</param>
    /// <returns>按比较关键字找出的最小对象</returns>
    public static T Min<T, TKey>(T[] array, SelectHandler<T, TKey> handler)
        where TKey : IComparable, IComparable<TKey>
    {
        T min = array[0];
        for (int i = 0; i < array.Length; i++)
        {
            if (handler(array[i]).CompareTo(handler(min)) < 0)
                min = array[i];
        }
        return min;
    }

    /// <summary>升序排列
    /// </summary>
    /// <param name="array">源数组</param>
    /// <param name="handler">排序依据的关键词</param>
    public static void OrderBy<T, TKey>(T[] array, SelectHandler<T, TKey> handler)
        where TKey : IComparable, IComparable<TKey>
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            for (int j = i+1; j < array.Length; j++)
            {
                if(handler(array[i]).CompareTo(handler(array[j]))>0)
                {
                    T temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }
        }
    }

    /// <summary>降序排列
    /// </summary>
    /// <param name="array">源数组</param>
    /// <param name="handler">排序依据的关键词</param>
    public static void OrderByDescending<T, TKey>(T[] array, SelectHandler<T, TKey> handler)
        where TKey : IComparable, IComparable<TKey>
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            for (int j = i + 1; j < array.Length; j++)
            {
                if (handler(array[i]).CompareTo(handler(array[j])) < 0)
                {
                    T temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }
        }
    }

    /// <summary>选择提取数组中部分信息后形成一个新的数组
    /// </summary>
    /// <param name="array">源数组</param>
    /// <param name="handler">提取算法</param>
    /// <returns>提取后的结果</returns>
    public static TKey[] Select<T, TKey>(T[] array, SelectHandler<T, TKey> handler)
    {
        TKey[] tempArr = new TKey[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            tempArr[i] = handler(array[i]);
        }
        return tempArr;
    }
}

/********************************* 关于ArrayHelper ********************************

 * 获取附近所有的敌人GameObject[] allEnemy;
 * 
 * 1. 找出距离玩家最近的敌人：
    GameObject[] target 
    = ArrayHelper.Min( allEnemy, p =>Vector3.Distance(敌人位置, 玩家位置))
   （这里的p代表敌人，所以敌人的位置为 p.transform.position）
 * 
 * 2. 找出距离玩家最远的敌人：
    GameObject[] target 
    = ArrayHelper.Max( allEnemy, p =>Vector3.Distance(敌人位置, 玩家位置))
 * 
 * 3. 找出附近"离玩家距离小于30并且HP大于0" 的所有敌人：
    GameObject[] target 
    = ArrayHelper.FindAll( allEnemy, p => p.GetComponent<状态脚本>().HP >0
                                        &&  p.GetComponent<状态脚本>().distance < 30)
 ************************************************************************************ 
 * 
 */
