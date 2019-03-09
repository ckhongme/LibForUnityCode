using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiTool : MonoBehaviour
{
    public static UiTool Instance;

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
