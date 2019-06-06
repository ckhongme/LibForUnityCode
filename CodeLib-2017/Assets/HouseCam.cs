using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class HouseCam : MonoBehaviour
{
    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.aspect = 1;

        GameObject go = GameObject.Find("DrawingBoard");
        if (go)
        {
            Transform parent = go.transform.FindChild("UI");
            transform.SetParent(parent);
            transform.localPosition = new Vector3(-0.5f, 0, -1);
            transform.localEulerAngles = Vector3.zero;
        }
    }

    void Start()
    {
        VrNotice.addNoticeListener(KEvent.HOUSEMAP_PIC, _GetPic);
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        VrNotice.removeNoticeListener(KEvent.HOUSEMAP_PIC, _GetPic);
    }

    private void _GetPic(string noticeType, object data)
    {
        gameObject.SetActive(true);
        Rect rect = new Rect(0, 0, cam.pixelWidth, cam.pixelHeight);
        Texture2D tex = kMethod.ScreenShot(cam, rect);
        DataMgr.house.SaveHousePic(tex.EncodeToJPG());
        gameObject.SetActive(false);
    }
}
