using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UguiTool : MonoBehaviour
{
    public static UguiTool Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            if (Instance == this)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// 设置输入框的
    /// </summary>
    public void SetContentType(InputField ifd, bool isPassword)
    {
        if(ifd != null)
        {
            if (isPassword)
                ifd.contentType = InputField.ContentType.Password;
            else
                ifd.contentType = InputField.ContentType.Standard;

            EventSystem.current.SetSelectedGameObject(ifd.gameObject, new BaseEventData(EventSystem.current));
        }
    }

}
