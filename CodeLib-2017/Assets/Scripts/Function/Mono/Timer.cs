using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float maxTime = 180;
    public bool isAutoStart = false;
    public bool isDesc = false;  

    public Text[] texts;

    private int _step = 1;
    private bool isTiming;
    private float _curTime;
    private float _tempTime;

    private void Start()
    {
        isTiming = isAutoStart;
        if(PlayerPrefs.HasKey("Timer"))
            maxTime = PlayerPrefs.GetInt("Timer");

        _ShowTime();
    }

    private void FixedUpdate()
    {
        if (isTiming)
        {
            if (Time.realtimeSinceStartup >= _tempTime)
            {
                _curTime++;
                if (_curTime > maxTime)
                {
                    _Over();
                    return;
                }
                _ShowTime();
                _tempTime = Time.realtimeSinceStartup + _step;
            }
        }
    }

    public void SetTimer(float maxTime, int step = 1)
    {
        this.maxTime = maxTime;
        _curTime = 0;
        _tempTime = 0;
        _step = step;
    }

    public void Begin()
    {
        isTiming = true;
    }

    public int Stop()
    {
        isTiming = false;
        return (int)(isDesc ? maxTime - _curTime : _curTime);
    }

    public void Resume()
    {
        isTiming = true;
        _tempTime = Time.realtimeSinceStartup - _step;
    }

    public void ReSet()
    {
        _curTime = 0;
    }


    private void _Over()
    {
        isTiming = false;
        _curTime = maxTime;
    }

    private void _ShowTime()
    {
        if (texts.Length <= 0) return;

        int temp = (int)(isDesc ? maxTime - _curTime : _curTime);
        int min = temp / 60;
        int sec = temp % 60;

        foreach (var item in texts)
        {
            item.text = string.Format("{0:D2}:{1:D2}", min, sec);
        }
    }
}
