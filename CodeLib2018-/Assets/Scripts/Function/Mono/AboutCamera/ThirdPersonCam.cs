using UnityEngine;
using System.Collections;

/// <summary>
/// Put the camera and player into one Empty object
/// </summary>
public class ThirdPersonCam : MonoBehaviour 
{
    public Camera thirdCamera;          //获取第三人称摄像机
    public Transform target;            //目标物体（角色）
    public float scrollSpeed = 400;     //滚动速度
    public float rotateSpeed = 250;     
    public float moveSpeed = 10;

	void Update () 
    {
        _Zoom();
        _RotateCam();
        _Movement();
	}

    /// <summary>
    /// 滑动鼠标滚轴缩放目标物体（限制缩放距离）
    /// </summary>
    void _Zoom()
    {
        float fieldOffset = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollSpeed;
        if (fieldOffset > 0.001 || fieldOffset < -0.001)
        {
            float fieldValue = thirdCamera.fieldOfView;
            //limit the fieldValue from 20 to 80
            fieldValue = Mathf.Clamp(fieldValue += fieldOffset, 20, 80);
            thirdCamera.fieldOfView = fieldValue;
        }
    }

    /// <summary>
    /// 摄像机的旋转：鼠标水平、垂直移动控制摄像机围绕目标物体旋转（垂直移动时限制旋转范围）  
    /// </summary>
    private void _RotateCam()
    {
        thirdCamera.transform.RotateAround(target.position, Vector3.up, 
            Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed);
        
        float angle = Vector3.Angle(thirdCamera.transform.forward, this.transform.up);
        float yAxis = Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed;
        if (angle < 90 && yAxis < 0  || angle> 140 && yAxis>0)
            yAxis = 0;
        thirdCamera.transform.RotateAround( target.position, thirdCamera.transform.right, yAxis );
    }

    /// <summary>
    /// 目标物体的移动（鼠标水平移动控制目标物体前进、后退的方向）
    /// </summary>
    void _Movement()
    {
        float movez = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        float movex = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        if (movez != 0)
        {
            //移动时  让目标物体的旋转角度 使用摄像机的y轴值
            Vector3 targetAngle = target.eulerAngles;
            targetAngle.y = thirdCamera.transform.eulerAngles.y;
            target.rotation = Quaternion.Lerp(target.rotation, Quaternion.Euler(targetAngle), Time.deltaTime * 10);
        }
        transform.Translate(movex, 0, movez, target);
    }
}
