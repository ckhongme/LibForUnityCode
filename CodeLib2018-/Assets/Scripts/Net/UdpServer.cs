using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class UdpServer : MonoBehaviour
{
    private const int port = 8088;
    private static string IpStr = "192.168.50.252";
    private static Socket serverSocket;
    private static byte[] result = new byte[1024];

    static void Main(string[] args)
    {
        IPAddress ipa = IPAddress.Parse(IpStr);
        IPEndPoint ipe = new IPEndPoint(ipa, port);

        StartUDPServer(ipe);
        //StartTCPServer(ipe);
    }

    #region TCP

    private static void StartTCPServer(IPEndPoint ipe)
    {
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        serverSocket.Bind(ipe);
        serverSocket.Listen(10);
        Console.WriteLine("启动监听{0}成功", serverSocket.LocalEndPoint.ToString());

        Thread thread = new Thread(TCPClientListener);
        thread.Start();
    }

    private static void TCPClientListener()
    {
        while (true)
        {
            //为新的客户端连接创建一个Socket对象
            Socket clientSocket = serverSocket.Accept();

            Console.WriteLine("客户端{0}成功连接", clientSocket.RemoteEndPoint.ToString());
            //向连接的客户端发送连接成功的数据
            clientSocket.Send(new byte[2]);
            //每个客户端连接创建一个线程来接受该客户端发送的消息
            Thread thread = new Thread(TCPRecieveMessage);
            thread.Start(clientSocket);
        }
    }

    private static void TCPRecieveMessage(object clientSocket)
    {
        Socket mClientSocket = (Socket)clientSocket;
        while (true)
        {
            try
            {
                int receiveNumber = mClientSocket.Receive(result);

                //Console.WriteLine("接收客户端{0}消息， 长度为{1}", mClientSocket.RemoteEndPoint.ToString(), receiveNumber);
                //Console.WriteLine("数据内容：{0}", result[0]);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                mClientSocket.Shutdown(SocketShutdown.Both);
                mClientSocket.Close();
                break;
            }
        }
    }

    #endregion

    #region UDP

    private static void StartUDPServer(IPEndPoint ipe)
    {
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        serverSocket.Bind(ipe);
        Console.WriteLine("启动监听{0}成功", serverSocket.LocalEndPoint.ToString());
        Thread thread = new Thread(ReceiveInfo);
        thread.Start();
    }

    private static void ReceiveInfo()
    {
        while (true)
        {
            EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = new byte[1024];
            int length = serverSocket.ReceiveFrom(data, ref remoteEndPoint);
            string message = Encoding.UTF8.GetString(data, 0, length);
            Console.WriteLine("从IP：" + (remoteEndPoint as IPEndPoint).Address.ToString() + " " + (remoteEndPoint as IPEndPoint).Port + "收到了数据：" + message);
        }
    }
}
