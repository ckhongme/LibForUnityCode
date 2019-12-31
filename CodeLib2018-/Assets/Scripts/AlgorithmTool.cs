using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace K
{
    public class AlgorithmTool
    {
        #region 冒泡排序 

        /// <summary>
        /// 冒泡排序（从小到大排序）
        /// </summary>
        public static void BubbleSort(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                bool finish = true;
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        var temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                        finish = false;
                    }
                }
                if (finish) break;
            }
        }

        #endregion

        #region 快速排序

        public static void QuickSort(int[] array, int left, int right)
        {
            if (left < right)
            {
                int pivot = Partition(array, left, right);
                QuickSort(array, left, pivot - 1);
                QuickSort(array, pivot + 1, right);
            }
        }

        private static int Partition(int[] array, int left, int right)
        {
            int pivot = left;
            while (left < right)
            {
                while (left < right && array[right] >= array[pivot])
                    right--;
                while (left < right && array[left] <= array[pivot])
                    left++;
                Swap(array, left, right);
            }
            Swap(array, pivot, left);
            return left;
        }

        private static void Swap(int[] array, int i, int j)
        {
            if (array.Length > j & array.Length > i)
            {
                var temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }



        public static void QuickSort2(int[] array, int left, int right)
        {
            if (left < right)
            {
                int pivot = Partition2(array, left, right);
                QuickSort2(array, left, pivot - 1);
                QuickSort2(array, pivot + 1, right);
            }
        }

        private static int Partition2(int[] array, int left, int right)
        {
            int pivot = array[left];
            while (left < right)
            {
                while (left < right && array[right] >= pivot)
                    right--;
                array[left] = array[right];

                while (left < right && array[left] <= pivot)
                    left++;
                array[right] = array[left];
            }
            array[left] = pivot;
            return left;
        }

        #endregion

    }
}