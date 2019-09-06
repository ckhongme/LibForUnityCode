using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UguiTool : MonoBehaviour
{
    private static UguiTool instance;
    public static UguiTool Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = new GameObject("UguiTool");
                GameObject.DontDestroyOnLoad(obj);
                instance = obj.AddComponent<UguiTool>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (Instance == null)
            instance = this;
        else
        {
            if (Instance == this)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// 设置输入框的类型
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
