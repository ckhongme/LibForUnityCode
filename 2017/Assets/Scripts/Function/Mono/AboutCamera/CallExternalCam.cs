using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Call the external camera
/// </summary
public class CallExternalCam : MonoBehaviour
{
    public string deviceName;           
    private WebCamTexture cameTexture;         
    private Image img;
    
    void Start()
    {
        img = GetComponent<Image>();    
        StartCoroutine(CallCamera());
    }
    
    IEnumerator CallCamera()
    {
        //获取授权  
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            if(cameTexture != null)
            {
                cameTexture.Stop();
            }

            WebCamDevice[] devices = WebCamTexture.devices;
            deviceName = devices[0].name; 
            
            //设置摄像机摄像的区域
            cameTexture = new WebCamTexture(deviceName);

            if(img != null)
                img.canvasRenderer.SetTexture(cameTexture);

            if(GetComponent<Renderer>())
                GetComponent<Renderer>().material.mainTexture = cameTexture;

            cameTexture.Play();//播放
        }
    }
}