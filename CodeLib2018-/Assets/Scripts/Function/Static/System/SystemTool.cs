using System;
using System.IO;

namespace K.system
{
    public class SystemTool
    {
        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <param name="format">格式："yyyyMMddhhmmss", "MM/dd/yyyy HH:mm:ss"</param>
        public static string GetCurrTime(string format = "")
        {
            if (!format.Equals(""))
            {
                //"yyyyMMddhhmmss", "MM/dd/yyyy HH:mm:ss"
                return DateTime.Now.ToString(format);
            }
            return DateTime.Now.ToString();
        }

        /// <summary>
        /// 调用打印机
        /// </summary>
        /// <param name="filePath">要打印文件的路径</param>
        public static void Printer(string filePath)
        {
            if (!File.Exists(filePath)) return;
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();  //系统进程

                //不显示调用程序窗口
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                process.StartInfo.UseShellExecute = true;  //采用操作系统自动识别系统
                process.StartInfo.FileName = filePath;         //要打印的文件的路径
                process.StartInfo.Verb = "print";          //打印
                process.Start();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("打印机连接有误" + ex.Message);
            }
        }
    }
}