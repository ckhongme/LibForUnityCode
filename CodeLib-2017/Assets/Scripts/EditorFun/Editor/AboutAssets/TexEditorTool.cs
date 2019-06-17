using System.Collections.Generic;
using UnityEditor;

namespace K
{
    /// <summary>
    /// 贴图资源管理
    /// </summary>
    public class TexEditorTool
    {
        private static string[] texSuffix = new string[] { ".tga" };
        private static List<string> texs = new List<string>();

        /// <summary>
        /// 开启MipMaps
        /// </summary>
        [MenuItem("K/TexEditor/OpenMipMaps")]
        private static void OpenMipMaps()
        {
            SetMipMaps(true);
        }

        /// <summary>
        /// 关闭MipMaps
        /// </summary>
        [MenuItem("K/TexEditor/CloseMipMaps")]
        private static void CloseMipMaps()
        {
            SetMipMaps(false);
        }

        /// <summary>
        /// 设置MipMaps
        /// </summary>
        private static void SetMipMaps(bool isOpen)
        {
            texs = SystemTool.GetTargetPaths(EditorTool.GetSelectionPath(), false, texSuffix);
            for (int i = 0; i < texs.Count; i++)
            {
                EditorUtility.DisplayProgressBar("SetMipMaps", "设置贴图的MipMaps属性", 1f * i / texs.Count);
                TextureImporter t = AssetImporter.GetAtPath(texs[i]) as TextureImporter;
                t.mipmapEnabled = isOpen;

                TextureImporterSettings tis = new TextureImporterSettings();
                t.ReadTextureSettings(tis);
                tis.ApplyTextureType(tis.textureType);
                t.SetTextureSettings(tis);
                AssetDatabase.ImportAsset(texs[i]);
            }
            EditorUtility.ClearProgressBar();
        }
    }
}