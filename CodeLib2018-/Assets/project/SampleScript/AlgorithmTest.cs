using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using K;
using System;

public class AlgorithmTest : MonoBehaviour
{
    private int[] ARRAY = new int[10] { 6, 1, 2, 7, 4, 3, 9, 5, 10, 8 };

    void Start()
    {
        _TestQuickSort();
    }

    #region Test BubbleSort

    private void _TestBubbleSort()
    {

    }


    #endregion

    #region Test QuickSort

    private void _TestQuickSort()
    {
        var array = new int[10];
        var time = 0f;

        time = Time.realtimeSinceStartup;
        ARRAY.CopyTo(array, 0);
        AlgorithmTool.QuickSort(array, 0, array.Length - 1);
        DebugArray("QuickSort", array, time);

        time = Time.realtimeSinceStartup;
        ARRAY.CopyTo(array, 0);
        AlgorithmTool.QuickSort2(array, 0, array.Length - 1);
        DebugArray("QuickSort2", array, time);

    }

    private void DebugArray(string funName, int[] array, float time)
    {
        time = Time.realtimeSinceStartup - time;
        StringBuilder sb = new StringBuilder();
        foreach (var item in array)
        {
            sb.Append(item);
            sb.Append(",");
        }
        Debug.Log(funName + ": " + sb.ToString() + " - usingTime: " + time);
    }

    #endregion
}


