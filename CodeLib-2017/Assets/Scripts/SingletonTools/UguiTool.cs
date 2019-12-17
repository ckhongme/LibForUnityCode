using System.Collections;
using System.Collections.Generic;
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
                obj.AddComponent<UguiTool>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) DestroyImmediate(gameObject);
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

    /// <summary>
    /// 设置下拉菜单内容
    /// </summary>
    /// <param name="list">选项列表</param>
    /// <param name="defaultIndex">默认选项</param>
    public void SetDropdown(Dropdown dropdowm, List<string> list, int defaultIndex = 0)
    {
        dropdowm.ClearOptions();
        foreach (var item in list)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item;
            dropdowm.options.Add(option);
        }
        dropdowm.value = defaultIndex;
        dropdowm.captionText.text = dropdowm.options[defaultIndex].text;
    }

}
