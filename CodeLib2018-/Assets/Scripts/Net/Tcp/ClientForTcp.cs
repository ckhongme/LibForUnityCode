using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using Google.Protobuf;
using System;

public class ClientForTcp : MonoBehaviour
{
    private Thread thread;
    private string serverIpStr = "192.168.50.252";
    private const int serverPort = 3333;
    protected byte[] result = new byte[1024];

    void Start()
    {
        //StartSocketClient();
    }

    protected virtual void ClientAction()
    {

    }

    #region Socket

    protected Socket socketClient;

    protected void StartSocketClient()
    {
        socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        SocketConnectServer();
    }

    /// <summary>
    /// 连接服务器
    /// </summary>
    private void SocketConnectServer()
    {
        IPAddress ipa = IPAddress.Parse(serverIpStr);
        IPEndPoint ipe = new IPEndPoint(ipa, serverPort);    
        try
        {
            socketClient.Connect(ipe);
        }
        catch
        {
            Debug.Log("连接服务器失败");
            return;
        }

        Debug.Log(string.Format("连接服务器{0}成功", socketClient.RemoteEndPoint.ToString()));

        thread = new Thread(ClientAction);
        thread.Start();
    }

    /// <summary>
    /// 单次接收
    /// </summary>
    private void SocketReceiveServerData()
    {
        try
        {
            int receiveNumber = socketClient.Receive(result);
            Debug.Log(string.Format("接收服务端端{0}消息， 长度为{1}", socketClient.RemoteEndPoint.ToString(), receiveNumber));
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            socketClient.Close();
        }
    }


    #endregion

    #region TcpClient

    protected TcpClient tcpClient;

    protected void StartTcpClient()
    {
        try
        {
            tcpClient = new TcpClient(serverIpStr, serverPort);
        }
        catch (Exception ex)
        {
            Debug.Log("连接服务器失败");
            return;
        }

        Debug.Log(string.Format("连接服务器{0}成功: {1}", tcpClient.Client.ToString(), tcpClient.Connected));
        thread = new Thread(ClientAction);
        thread.Start();
    }


    /// <summary>
    /// example - receive data
    /// </summary>
    private void TcpClientReceiveServerData()
    {
        while (tcpClient.Connected)
        {
            NetworkStream stream = tcpClient.GetStream(); //通过网络流进行数据的交换
            try
            {
                int receiveNumber = stream.Read(result, 0, 1024);
                Console.WriteLine("接收服务器{0}消息， 长度为{1}", tcpClient.Client.ToString(), receiveNumber);
            }
            catch (Exception ex)
            {
                tcpClient.Close();
                break;
            }
        }
    }

    #endregion


    private void OnDestroy()
    {
        thread.Abort();
        if(socketClient != null)
            socketClient.Close();
        if(tcpClient != null)
            tcpClient.Close();
    }
}
