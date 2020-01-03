using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DataReceiver : MonoBehaviour
{
    public SocketServer server;
    public RawImage image;
    private byte[] temp = new byte[102400];
    private bool isGotData = false;

    void Start()
    {
        server.ReceiveData += GotPic;
    }

    private void ReceiveData(byte[] data, IPEndPoint endPoint, int length)
    {
        string message = Encoding.UTF8.GetString(data, 0, length);
        Debug.Log("从客户端" + endPoint.ToString() + " 接收到了数据：" + message);
    }


    private void GotPic(byte[] data, IPEndPoint endPoint, int length)
    {
        temp = data;
        isGotData = true;
    }

    private void Update()
    {
        if (isGotData)
        {
            var texture = new Texture2D(10, 10);
            texture.LoadImage(temp);
            image.texture = texture;
            isGotData = false;
        }
    }
}
