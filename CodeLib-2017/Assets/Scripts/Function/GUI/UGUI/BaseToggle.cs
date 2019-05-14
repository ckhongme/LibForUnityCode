using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseToggle : MonoBehaviour
{
    public bool isDefaultOn;
    private Toggle toggle;

	private void Start ()
    {
        toggle = GetComponent<Toggle>();
        toggle.isOn = isDefaultOn;

        toggle.onValueChanged.AddListener(delegate (bool isOn)
        {
            this.OnValueChange(isOn);
        });
	}

    private void OnValueChange(bool isOn)
    {
        Debug.Log("Toggle.isOn is " + isOn + "  (BaseToggle.cs)");
    }
}
