using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 使用Tab和Shift+Tab切换UGUI的InputField  （将脚本挂在对应的InputField上）
/// </summary>
public class InputNavigator : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private EventSystem _system;
    private bool _isSelect = false;

    void Start()
    {
        _system = EventSystem.current;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && _isSelect)
        {
            Selectable next = null;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                next = _system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            }
            else
            {
                next = _system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            }
            if (next != null)
            {
                InputField inputfield = next.GetComponent<InputField>();
                _system.SetSelectedGameObject(next.gameObject, new BaseEventData(_system));
            }
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        _isSelect = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _isSelect = false;
    }
}