using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class ServerForTcp : MonoBehaviour
{
    private const int port = 3333;
    private string IpStr = "192.168.50.252";
    private Thread listenThread;

    void Start()
    {
        //StartSocketServer();
        //StartTcpListener();
    }

    /// <summary>
    /// 服务端是接受数据，还是发送数据
    /// </summary>
    protected virtual void ServerAction(object socketObj, object threadObj)
    {

    }

    #region Socket

    private Socket serverSocket;

    protected void StartSocketServer()
    {
        IPAddress ipa = IPAddress.Parse(IpStr);
        IPEndPoint ipe = new IPEndPoint(ipa, port);

        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(ipe);
        serverSocket.Listen(10);
        Debug.Log(string.Format("启动Tcp监听{0}成功 ", serverSocket.LocalEndPoint.ToString()));

        listenThread = new Thread(SocketClientListener);
        listenThread.Start();
    }

    /// <summary>
    /// 监听 客户端 的连接情况
    /// </summary>
    private void SocketClientListener()
    {
        while (true)
        {
            //为新的客户端连接创建一个Socket对象
            Socket clientSocket = serverSocket.Accept();
            Debug.Log(string.Format("客户端{0}成功连接", clientSocket.RemoteEndPoint.ToString()));
            Thread thread = null;
            thread = new Thread(() => ServerAction(clientSocket, thread));
            thread.Start();
        }
    }

    /// <summary>
    /// 接受 客户端 的数据
    /// </summary>
    protected void ReceiveSocketClientData(object socketObj, object threadObj)
    {
        var thread = (Thread)threadObj;
        var client = (Socket)socketObj;

        byte[] data = new byte[1024];

        while (client.Connected)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                break;
            }
        }

        if (!client.Connected)
        {
            thread.Abort();
            client.Close();
        }
    }

    /// <summary>
    /// Example
    /// </summary>
    private void SendSocketClientData(object socketObj, object threadObj)
    {
        var thread = (Thread)threadObj;
        var client = (Socket)socketObj;
            
        while (client.Connected)
        {
            try
            {
                byte[] data = null;  //TODO 数据处理
                client.Send(data);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                break;
            }
        }

        if (!client.Connected)
        {
            thread.Abort();
            client.Close();
        }
    }

    #endregion

    #region TcpListener TcpClient 

    private TcpListener tcpListener;

    protected void StartTcpListener()
    {
        tcpListener = new TcpListener(IPAddress.Parse(IpStr), port);  //创建socket对象
        tcpListener.Start(); //开始进行监听
        Debug.Log(string.Format("启动Tcp监听{0}成功 ", tcpListener.LocalEndpoint.ToString()));

        listenThread = new Thread(TcpClientListener);
        listenThread.Start();
    }

    /// <summary>
    /// 监听 客户端 的连接情况
    /// </summary>
    private void TcpClientListener()
    {
        while (true)
        {
            //为新的客户端连接创建一个Socket对象
            TcpClient tcpClient = tcpListener.AcceptTcpClient();
            Debug.Log(string.Format("客户端{0}成功连接", tcpClient.Client.RemoteEndPoint.ToString()));
            Thread thread = null;
            thread = new Thread(() => { ServerAction(tcpClient, thread); });
            thread.Start();
        }
    }

    /// <summary>
    /// Example 接受 客户端 的数据 
    /// </summary>
    private void ReceiveTcpClientData(object clientObj, object threadObj)
    {
        var thread = (Thread)threadObj;
        var client = (TcpClient)clientObj;
        NetworkStream stream = client.GetStream();

        byte[] data = new byte[1024];
        while (client.Connected)
        {
            try
            {
                int length = stream.Read(data, 0, 1024);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                break;
            }
        }

        if (!client.Connected)
        {
            thread.Abort();
            client.Close();
        }
    }

    /// <summary>
    /// Example 发送数据给 客户端
    /// </summary>
    private void SendTcpClientData(object clientObj, object threadObj)
    {
        var thread = (Thread)threadObj;
        var client = (TcpClient)clientObj;
        NetworkStream stream = client.GetStream();

        while (client.Connected)
        {
            try
            {
                byte[] data = null;
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                break;
            }
        }

        if (!client.Connected)
        {
            thread.Abort();
            client.Close();
        }
    }

    #endregion


    private void OnDestroy()
    {
        if (listenThread != null)
            listenThread.Abort();

        if (tcpListener != null)
            tcpListener.Stop();

        if (serverSocket != null)
            serverSocket.Close();
    }

}