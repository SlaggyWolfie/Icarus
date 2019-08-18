using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UI_TimerOverlay : MonoBehaviour
{
    private Text _textComponent;
    private int _timer;
    private int _referenceTimer;
    private float _time;

    private void Start()
    {
        _textComponent = GetComponent<Text>();
        if (_textComponent == null) Debug.LogError("WARNING: " + GetType().ToString() +
            " missing " + _textComponent.GetType().ToString() + " component.");
    }

    private void LateUpdate()
    {
        if (_textComponent.isActiveAndEnabled)
        {
            if (_timer > 0)
            {
                int minutes = (_timer) / 60;
                int seconds = _timer % 60;
                _textComponent.text = minutes.ToString() + ":" + ((seconds < 10) ? "0" : "") + seconds.ToString();
                if (Time.time - _time >= 1)
                {
                    _timer--;
                    _time = Time.time;
                }
            }
            else
            {
                _textComponent.enabled = false;
                _textComponent.text = "";
                _timer = 0;
            }
        }
    }

    public void SetTimer(int timer)
    {
        _timer = timer;
        _time = Time.time;
        _textComponent.enabled = true;
    }

    public void SetFont(Font font, FontStyle fontStyle = FontStyle.Normal, int fontSize = 14)
    {
        _textComponent.font = font;
        _textComponent.fontStyle = fontStyle;
        _textComponent.fontSize = fontSize;
    }

    public void StopTimer()
    {
        _timer = 0;
    }
}
