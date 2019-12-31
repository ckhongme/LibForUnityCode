using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Net;

public class UpdClient : MonoBehaviour
{
    private const int port = 8088;
    private string IpStr = "192.168.50.252";
    private Socket socket;
    private EndPoint serverPoint;

    void Start()
    {
        IPAddress ip = IPAddress.Parse(IpStr);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        serverPoint = new IPEndPoint(ip, port);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SendMessage();
    }

    private int count = 0;
    private void SendMessage()
    {
        count++;
        string message = string.Format("这是发送的第 {0} 条信息！！！", count);
        var data = Encoding.UTF8.GetBytes(message);
        socket.SendTo(data, serverPoint);
    }
}
