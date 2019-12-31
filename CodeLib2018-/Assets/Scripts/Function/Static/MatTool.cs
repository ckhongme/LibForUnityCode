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
        public static void ChangeShader(Material mat, string shaderName)
        {
            if (mat != null)
                mat.shader = Shader.Find(shaderName);
        }

        /// <summary>
        /// 改变物体材质球的Shader
        /// </summary>
        /// <param name="go"></param>
        /// <param name="shaderName"></param>
        public static void ChangeShader(GameObject go, string shaderName)
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
                for (int i = 0; i < images.Length; i++)
                {
                    if (images[i].material == null || images[i].material != mat)
                    {
                        images[i].material = mat;
                    }
                }

                if (isCoverText)
                {
                    Text[] texts = go.GetComponentsInChildren<Text>(true);
                    for (int i = 0; i < texts.Length; i++)
                    {
                        if (texts[i].material == null || texts[i].material != mat)
                        {
                            texts[i].material = mat;
                        }
                    }
                }
            }
        }
    }
}