using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace K
{
    /// <summary>
    /// 材质工具
    /// </summary>
    public class MatTool
    {
        /// <summary>
        /// 改变物体的材质
        /// </summary>
        /// <param name="go"></param>
        /// <param name="shaderName"></param>
        public static void ChangeMat(GameObject go, string shaderName)
        {
            if (go != null)
            {
                Renderer[] array = go.GetComponentsInChildren<MeshRenderer>(true);
                foreach (Renderer renderer in array)
                {
                    renderer.material.shader = Shader.Find(shaderName);
                }
            }
        }

        /// <summary>
        /// 改UI的材质
        /// </summary>
        public static void ChangeUiMat(GameObject go, Material mat, bool isCoverText = true)
        {
            if (go != null)
            {
                Image[] images = go.GetComponentsInChildren<Image>(true);
                int count = images.Length;
                for (int i = 0; i < count; i++)
                {
                    if (images[i].material == null || images[i].material != mat)
                    {
                        images[i].material = mat;
                    }
                }

                if (isCoverText)
                {
                    Text[] texts = go.GetComponentsInChildren<Text>(true);
                    int count2 = texts.Length;
                    for (int i = 0; i < count2; i++)
                    {
                        if (texts[i].material == null || texts[i].material != mat)
                        {
                            texts[i].material = mat;
                        }
                    }
                }
            }
        }

        public static void ChangeMat(Material mat, string shaderName)
        {
            if (mat != null)
                mat.shader = Shader.Find(shaderName);
        }
    }
}