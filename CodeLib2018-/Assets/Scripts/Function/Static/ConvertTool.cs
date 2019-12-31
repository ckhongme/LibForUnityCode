using System;
using UnityEngine;

namespace K
{
    public class ConvertTool
    {
        /// <summary>
        /// 通过色值获取颜色
        /// </summary>
        public static Color GetColor(string rgba)
        {
            int count = rgba.Length;
            if (count != 6 && count != 8)
                return Color.white;

            int r = Convert.ToInt32(rgba.Substring(0, 2), 16);
            int g = Convert.ToInt32(rgba.Substring(2, 2), 16);
            int b = Convert.ToInt32(rgba.Substring(4, 2), 16);

            int a;
            if (count == 8)
                a = Convert.ToInt32(rgba.Substring(6, 2), 16);
            else
                a = 255;
            return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        }
        
        /// <summary>
        /// 字节数组转Tex
        /// </summary>
        /// <param name="binary"></param>
        /// <returns></returns>
        public static Texture2D Byte2Tex(byte[] binary)
        {
            if (binary == null) return null;

            Texture2D tex2D = new Texture2D(0, 0);
            //根据字节数组加载图片
            tex2D.LoadImage(binary);
            return tex2D;
        }

        /// <summary>
        /// 字节数组转Sprite
        /// </summary>
        /// <param name="binary">字节数组</param>
        /// <returns></returns>
        public static Sprite Byte2Sprite(byte[] binary)
        {
            if (binary == null) return null;

            return Texture2Sprite(Byte2Tex(binary));
        }

        /// <summary>
        /// 将Texture转成Sprite
        /// </summary>
        /// <param name="tex">纹理对象</param>
        /// <returns></returns>
        public static Sprite Texture2Sprite(Texture tex)
        {
            if (tex == null) return null;
            return Sprite.Create(tex as Texture2D, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }


    }
}