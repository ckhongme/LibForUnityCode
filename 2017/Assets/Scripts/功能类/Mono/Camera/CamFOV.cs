using UnityEngine;
using System.Collections;

/// <summary>
/// 镜头控制：鼠标右键缩放相机视野
/// </summary>
public class CamFOV : MonoBehaviour
{
    public float FOVRange = 40;     //缩放距离
    public float Speed = 5;

    private Camera cam;        
    private bool isFar = true;      //默认是远视角 
    private float minFOV, maxFOV;   
    private float curFOV;         

    void Start()
    {
        cam = this.GetComponent<Camera>();
        maxFOV = Mathf.Clamp(cam.fieldOfView, 0, 170);
        minFOV = Mathf.Clamp(cam.fieldOfView - FOVRange, 0, 100);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            _ChangeFOV();

        SetFOV();
    }

    /// <summary>
    /// 改变视角
    /// </summary>
    private void _ChangeFOV()
    {
        isFar = !isFar;
        curFOV = isFar ? minFOV : maxFOV;
    }
    
    /// <summary>
    /// 设置视角
    /// </summary>
    void SetFOV()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, curFOV, Time.deltaTime * Speed);
    }
}




