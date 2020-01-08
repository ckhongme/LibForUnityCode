using System;
using System.IO;
using System.Diagnostics;

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





        #region 通过cmd调用外部应用程序

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"> 要调用的应用程序名或者要获取的文件名 带后缀 </param>
        /// <returns></returns>
        public static Process CreateProcess(string filePath)
        {
            //创建系统进程
            Process p = new Process();
            p.StartInfo.FileName = filePath;
            p.StartInfo.CreateNoWindow = true;  //是否显示调用程序的窗口
            return p;
        }


        //p.StartInfo.UseShellExecute = false;//不使用操作系统外壳程序启动线程(一定为FALSE,详细的请看MSDN)
        //p.StartInfo.RedirectStandardError = true;//把外部程序错误输出写到StandardError流中(这个一定要注意,FFMPEG的所有输出信息,都为错误输出流,用StandardOutput是捕获不到任何消息的...这是我耗费了2个多月得出来的经验...mencoder就是用standardOutput来捕获的)
        //p.ErrorDataReceived += new DataReceivedEventHandler(Output);//外部程序(这里是FFMPEG)输出流时候产生的事件,这里是把流的处理过程转移到下面的方法中,详细请查阅MSDN


        public static void StartProcess(Process p)
        {
            p.Start();//启动线程
            p.BeginErrorReadLine();//开始异步读取

            p.WaitForExit();//阻塞当前进程，知道运行的外部程序退出；
            p.Close();//关闭进程
            p.Dispose();//释放资源
        }

        /// <summary>
        /// 为外部程序添加程序退出的事件监听；
        /// </summary>
        /// <param name="p"></param>
        /// <param name="exit">当外部程序退出时调用此方法</param>
        public static void AddExitEvent(Process p, Action<object, EventArgs> exit)
        {
            p.Exited += new EventHandler(exit);
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
                Process process = new Process();  
                //不显示调用程序窗口
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.UseShellExecute = true;       //采用操作系统自动识别系统
                process.StartInfo.FileName = filePath;          //要打印的文件的路径
                process.StartInfo.Verb = "print";          //打印
                process.Start();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("打印机连接有误" + ex.Message);
            }
        }



        #endregion

    }
}