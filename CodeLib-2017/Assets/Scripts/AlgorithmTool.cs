using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmTool  
{	
	public static void BubbleSort(int[] array)
	{
		for(int i = 0; i < array.Length; i++)
		{
			bool finish = true;
			for(int j = 0; j < array.Length - i - 1; j ++)
			{
				if(array[j] > array[j+1])
				{
					var temp = array[j];
					array[j] = array[j+1];
					array[j+1] = temp;
					finish = false;
				}
			}
			if(finish) break;
		}
	}
}
