using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ui自适应  (UGUI)
/// </summary>
public class UiSelfAdapt : MonoBehaviour
{
    public float maxWidth = 1920;       //ui的初始宽度
    public float maxHeigh = 1080;       //ui的初始高度

    public bool isChildUI = false;      //是否子物体     

    private float screenWidth = 0f;     //屏幕宽度
    private float screenHeight = 0f;    //屏幕高度

    private RectTransform rectTfm;
    private Vector2 initScale;          //ui的初始大小

    private void Awake()
    {
        rectTfm = transform.GetComponent<RectTransform>();
        initScale = new Vector2(rectTfm.localScale.x, rectTfm.localScale.y);
    }

    private void Start()
    {
        //_SelfAdapt();
    }

    private void Update()
    {
        if (screenWidth != Screen.width || screenHeight != Screen.height)
        {
            screenWidth = Screen.width;
            screenHeight = Screen.height;
            _SelfAdapt();
        }
    }

    /// <summary>
    /// 改变ui大小 实现 自适应
    /// </summary>
    private void _SelfAdapt()
    {
        if (isChildUI)
        {
            //设置长和宽
            Vector2 sizeDelta = Vector2.zero;
            sizeDelta.x = rectTfm.sizeDelta.x * Screen.width / maxWidth;
            sizeDelta.y = rectTfm.sizeDelta.y * Screen.height / maxHeigh;
            rectTfm.sizeDelta = sizeDelta;

            //设置位置
            Vector3 pos = Vector3.zero;
            pos.x = rectTfm.anchoredPosition3D.x * Screen.width / maxWidth;
            pos.y = rectTfm.anchoredPosition3D.y * Screen.height / maxHeigh;
            rectTfm.anchoredPosition3D = pos;
        }
        else
        {
            Vector3 scale = Vector3.zero;
            scale.x = initScale.x * Screen.width / maxWidth;
            scale.y = initScale.y * Screen.height / maxHeigh;
            rectTfm.localScale = scale;
        }
    }
}
