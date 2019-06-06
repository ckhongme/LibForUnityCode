using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 截屏 功能模块
/// </summary>
public class ScreenShot
{
    private MonoBehaviour mono;
    private GameObject vrCam;               // vr摄像机

    private Transform countDown;            // 倒计时对象
    private SpriteRenderer sprite;          // 倒计时2D精灵
    private string fileName;                // 保存的截图文件名

    private int num = 3;                    // 倒计时的数量
    private int index = 0;                  // 显示的图像序号

    public void Init(MonoBehaviour mono)
    {
        this.mono = mono;

        //创建倒计时对象
        GameObject temp = Resources.Load<GameObject>("Home/ScreenShot/CountDown");
        if (temp != null)
        {
            countDown = GameObject.Instantiate(temp).transform;
            sprite = countDown.GetComponent<SpriteRenderer>();
            countDown.gameObject.SetActive(false);
            countDown.AddParent(mono.transform);
        }

        VrNotice.addNoticeListener(KEvent.SCREEN_SHOT, _ScreenShot);
    }

    public void OnDestroy()
    {
        VrNotice.removeNoticeListener(KEvent.SCREEN_SHOT, _ScreenShot);
    }

    /// <summary>
    /// 截屏
    /// </summary>
    /// <param name="info"></param>
    /// <param name="data"></param>
    private void _ScreenShot(string info, object data)
    {
        if (countDown.parent == mono.transform)
        {
            if (vrCam == null)
                vrCam = GameObject.FindGameObjectWithTag("MainCamera");

            countDown.AddParent(vrCam.transform);
            countDown.localPosition = new Vector3(0, 0, 0.5f);
        }

        index = num;
        //隐藏主菜单
        VrNotice.sendNotice(KEvent.HIDE_HOMEPAGE);
        VRPlayer.Instance.ToScenePos();
        mono.StartCoroutine(_SetCountDown());
    }

    /// <summary>
    /// 开始倒计时
    /// </summary>
    /// <returns></returns>
    private IEnumerator _SetCountDown()
    {
        if (countDown == null) yield return null;
        if (index == 3)
        {
            yield return new WaitForSeconds(0.5f);
        }

        sprite.sprite = Resources.Load<Sprite>(string.Format("Home/ScreenShot/{0}", index));
        countDown.gameObject.SetActive(true);

        //透明隐藏
        sprite.DOFade(0, 0.5f).OnComplete(() =>
        {
            index--;
            countDown.gameObject.SetActive(false);
            sprite.DOFade(1, 0f);
        });

        yield return new WaitForSeconds(1);

        if (index > 0)
        {
            mono.StartCoroutine(_SetCountDown());
        }
        else
        {
            _DoScreenShot();

            yield return new WaitForSeconds(3f);

            VrNotice.sendNotice(KEvent.SHOW_HOMEPAGE);
            countDown.AddParent(mono.transform);

            //显示保存路径
            VrNotice.sendNotice(KEvent.SHOW_HOMEPAGE_TIP, string.Format("截图保存路径：{0}", fileName));
        }
    }

    /// <summary>
    /// 进行截屏
    /// </summary>
    private void _DoScreenShot()
    {
        string time = System.DateTime.Now.ToOADate().ToString();
        fileName = string.Format("{0}/ScreenShot/{1}_screenshot.png", Application.streamingAssetsPath, time);

        //截屏(主相机视角)
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        //获取截屏图像
        Texture2D tex2d = kMethod.ScreenShot(Camera.main, rect, fileName);

        GameObject screenshot = GameObject.Instantiate(Resources.Load<GameObject>("Home/ScreenShot/ScreenShot"));
        screenshot.GetComponent<SpriteRenderer>().sprite = kMethod.Texture2Sprite(tex2d);

        if (vrCam == null)
            vrCam = GameObject.FindGameObjectWithTag("MainCamera");

        screenshot.transform.AddParent(vrCam.transform);
        screenshot.transform.localPosition = new Vector3(0, 0, 1f);

        //销毁省略图
        GameObject.Destroy(screenshot, 2.5f);
    }
}
