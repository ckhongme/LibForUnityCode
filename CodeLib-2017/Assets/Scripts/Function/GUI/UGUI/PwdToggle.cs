using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 密码保护
/// </summary>
[RequireComponent(typeof(Toggle))]
public class PwdToggle : MonoBehaviour
{
    [Tooltip("初始状态是否为On")]
    public bool isDefaultOn = false;
    [Tooltip("密码的输入框组件")]
    public InputField password;         
    [Tooltip("密码显示的图标")]
    public Sprite showIcon;   
    [Tooltip("密码隐藏的图标")]          
    public Sprite hideIcon;
    [Tooltip("背景图（可为Null）")]
    public Sprite Bg;                   

    private Toggle toggle;
    private Image checkmark;

	void Start ()
    {
        toggle = GetComponent<Toggle>();
        toggle.isOn = isDefaultOn;

        //设置图标
        toggle.graphic = null;
        if(Bg != null)
        {
            transform.Find("Background").GetComponent<Image>().sprite = Bg;
        }
        checkmark = transform.Find("Background/Checkmark").GetComponent<Image>();
        SetContentType(isDefaultOn);
        SelectIcon(isDefaultOn);


        //监听事件
        toggle.onValueChanged.AddListener(delegate(bool isOn)
        {
            this.OnValueChange(isOn);
        });
	}
	
    /// <summary>
    /// 状态改变
    /// </summary>
    /// <param name="isOn"></param>
    void OnValueChange(bool isOn)
    {
        SetContentType(isOn);
        SelectIcon(isOn);
        EventSystem.current.SetSelectedGameObject(password.gameObject, new BaseEventData(EventSystem.current));
    }

    /// <summary>
    /// 改变图标
    /// </summary>
    /// <param name="isOn"></param>
    private void SelectIcon(bool isOn)
    {
        if (isOn && showIcon != null)
        {
            checkmark.sprite = showIcon;
        }
        else if (!isOn && hideIcon != null)
        {
            checkmark.sprite = hideIcon;
        }
    }

    /// <summary>
    /// 设置
    /// </summary>
    /// <param name="isOn"></param>
    private void SetContentType(bool isOn)
    {
        if (isOn)
        {
            password.contentType = InputField.ContentType.Standard;
        }
        else
        {
            password.contentType = InputField.ContentType.Password;
        }
    }
}
