using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class SocketServer : MonoBehaviour
{
    public SocketServerType server;
    public Action<byte[], IPEndPoint, int> ReceiveData;

    private const int port = 8088;
    private string IpStr = "192.168.50.252";
    private Socket serverSocket;
    private byte[] result = new byte[1024];

    private void Start()
    {
        IPAddress ipa = IPAddress.Parse(IpStr);
        IPEndPoint ipe = new IPEndPoint(ipa, port);

        if(server == SocketServerType.UdpServer)
            StartUDPServer(ipe);
        else
            StartTCPServer(ipe);
    }

    #region TCP

    private void StartTCPServer(IPEndPoint ipe)
    {
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(ipe);
        serverSocket.Listen(10);
        Debug.Log(string.Format("启动Tcp监听{0}成功 ", serverSocket.LocalEndPoint.ToString()));

        Thread thread = new Thread(TCPClientListener);
        thread.Start();
    }

    private void TCPClientListener()
    {
        while (true)
        {
            //为新的客户端连接创建一个Socket对象
            Socket clientSocket = serverSocket.Accept();

            Debug.Log(string.Format("客户端{0}成功连接", clientSocket.RemoteEndPoint.ToString()));
            //向连接的客户端发送连接成功的数据
            clientSocket.Send(new byte[2]);
            //每个客户端连接创建一个线程来接受该客户端发送的消息
            Thread thread = new Thread(TCPRecieveMessage);
            thread.Start(clientSocket);
        }
    }

    private void TCPRecieveMessage(object clientSocket)
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

    #region UDP ReceiveData

    private void StartUDPServer(IPEndPoint ipe)
    {
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        serverSocket.Bind(ipe);
        Debug.Log(string.Format("启动Udp服务器 {0}成功", serverSocket.LocalEndPoint.ToString()));
        Thread thread = new Thread(UdpReceiveInfo);
        thread.Start();
    }

    private void UdpReceiveInfo()
    {
        while (true)
        {
            EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = new byte[102400];
            int length = serverSocket.ReceiveFrom(data, ref endPoint);
            if(ReceiveData!=null)
                ReceiveData.Invoke(data, (endPoint as IPEndPoint), length);
        }
    }

    #endregion

    private void OnDestroy()
    {
        if (serverSocket.Connected)
        {
            serverSocket.Shutdown(SocketShutdown.Both);
            serverSocket.Close();
        }
    }
}

public enum SocketServerType
{
    TcpServer,
    UdpServer,
}
