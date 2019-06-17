using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace K
{
    /// <summary>
    /// 材质编辑器 （添加贴图&移除贴图）
    /// </summary>
    public class MatEditorTool
    {
        //此处添加需要命名的资源后缀名,注意大小写。
        private static string[] matSuffix = new string[] { ".mat" };
        private static string[] texSuffix = new string[] { ".tga", ".png" };

        private static List<FileInfo> mats = new List<FileInfo>();
        private static List<FileInfo> texs = new List<FileInfo>();

        /// <summary>
        /// 给材质添加贴图
        /// </summary>
        [MenuItem("K/MatEditor/AddTex")]
        private static void AddTex()
        {
            SetTex(true);
        }

        /// <summary>
        /// 给材质移除贴图
        /// </summary>
        [MenuItem("K/MatEditor/RemoveTex")]
        private static void RemoveTex()
        {
            SetTex(false);
        }

        #region 添加&移除贴图

        private static void SetTex(bool isAdd)
        {
            string path = EditorTool.GetSelectionPath(true);
            SystemTool.GetFileInfoList(path, mats, matSuffix);

            if (isAdd)
                SystemTool.GetFileInfoList(path, texs, texSuffix);

            for (int i = 0; i < mats.Count; i++)
            {
                FileInfo fileInfo = mats[i];

                if (isAdd)
                {
                    EditorUtility.DisplayProgressBar("AddTex", "给材质添加贴图ing", 1f * i / mats.Count);
                    _AddTex(fileInfo, texs);
                }
                else
                {
                    EditorUtility.DisplayProgressBar("RemoveTex", "给材质移除贴图ing", 1f * i / mats.Count);
                    _RemoveTex(fileInfo);
                }
                AssetDatabase.RemoveUnusedAssetBundleNames();
            }
            EditorUtility.ClearProgressBar();
        }

        private static void _AddTex(FileInfo fileInfo, List<FileInfo> texs)
        {
            Material mat = EditorTool.Trans2Object<Material>(fileInfo);
            string matName = NameSetting(fileInfo.Name);

            if (mat != null)
            {
                foreach (var tex in texs)
                {
                    if (NameCompare(matName, tex.Name))
                    {
                        Texture t = EditorTool.Trans2Object<Texture>(tex);
                        if (t != null)
                        {
                            SelectTex(mat, t);
                        }
                    }
                }
                MatSetting(mat);
            }
        }

        /// <summary>
        /// 设置对应的贴图
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="tex"></param>
        private static void SelectTex(Material mat, Texture tex)
        {
            string name = tex.name.ToLower();
            string texName = string.Empty;

            if (name.Contains("albedo"))
            {
                texName = Albedo;
            }
            else if (name.Contains("ao"))
            {
                texName = AO;
            }
            else if (name.Contains("metallic"))
            {
                texName = Metallic;
            }
            else if (name.Contains("normal"))
            {
                texName = Normal;
                //Clicking “Fix Now” has the same effect as selecting Texture Type: Normal Map in the texture inspector settings
                TextureImporter t = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(tex)) as TextureImporter;
                t.textureType = TextureImporterType.NormalMap;
            }
            else if (name.Contains("emission"))
            {
                texName = Emission;
            }

            if (texName != string.Empty)
            {
                mat.SetTexture(texName, tex);
            }
        }

        private static void _RemoveTex(FileInfo fileInfo)
        {
            Material mat = EditorTool.Trans2Object<Material>(fileInfo);
            if (mat != null)
            {
                mat.SetTexture(Albedo, null);
                mat.SetTexture(AO, null);
                mat.SetTexture(Metallic, null);
                mat.SetTexture(Normal, null);
                mat.SetTexture(Emission, null);
            }
        }

        #endregion

        #region 材质球属性
        private const string Albedo = "_MainTex";
        private const string AO = "_OcclusionMap";
        private const string Metallic = "_MetallicGlossMap";
        private const string Normal = "_BumpMap";
        private const string Emission = "_EmissionMap";
        #endregion

        #region 规则

        /// <summary>
        /// 命名规则
        /// </summary>
        private static string NameSetting(string name)
        {
            string lowerName = name.ToLower();                          //转成小写
            name = name.Substring(0, name.Length - 4);                  //去除后缀 “.mat”

            Debug.Log(name);
            return name;
        }

        /// <summary>
        /// 名字的比较规则 （for循环执行）
        /// </summary>
        private static bool NameCompare(string matName, string texName)
        {
            texName = texName.ToLower();                    //转成小写
            if (texName.Contains(matName))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 材质的特殊处理
        /// </summary>
        /// <param name="mat"></param>
        private static void MatSetting(Material mat)
        {

        }

        #endregion
    }
}
