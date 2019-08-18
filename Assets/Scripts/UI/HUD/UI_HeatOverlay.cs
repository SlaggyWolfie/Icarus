using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dict
{
    public float alpha;
    public float duration;
}

[RequireComponent(typeof(Image))]
public class UI_HeatOverlay : MonoBehaviour
{
    [SerializeField]
    private Dict[] dict;

    private Image _image;
    private bool _heatTrigger;
    private float _startTime;

    [SerializeField]
    private float _immediateTimeChange = 0.001f;

    //[SerializeField]
    //private float _normalTimeChange = 0.1f;

    //[SerializeField]
    //private float _maxAlpha = 0.5f;

    private float _duration;

    // Use this for initialization
    private void Start()
    {
        _image = GetComponent<Image>();
        if (_image == null) Debug.LogError("WARNING: Image component missing from UI_HeatOverlay");
        else
        {
            _image.enabled = true;
            _image.CrossFadeAlpha(0, _immediateTimeChange, false);
        }
    }

    private void LateUpdate()
    {
        if (_heatTrigger)
        {
            _duration = 0;
            for (int i = 0; i < dict.Length; i++)
            {
                _duration += dict[i].duration;
                if (Time.time - _startTime <= _duration)
                {
                    _image.CrossFadeAlpha(dict[i].alpha, dict[i].duration, false);
                    break;
                }
            }
        }
        else
        {
            _image.CrossFadeAlpha(0, _duration, false);
        }
    }

    public void StartHeat()
    {
        _heatTrigger = true;
        _startTime = Time.time;
    }

    public void EndHeat()
    {
        _heatTrigger = false;
    }

    //public void OnHeatTrigger(bool trigger)
    //{
    //    _heatTrigger = trigger;

    //    float targetAlpha = trigger ? _maxAlpha : 0;
    //    _image.CrossFadeAlpha(targetAlpha, _normalTimeChange, false);
    //}

    //public void OnHeatTrigger(bool trigger, float alpha)
    //{
    //    float targetAlpha = trigger ? alpha : 0;
    //    _image.CrossFadeAlpha(targetAlpha, _normalTimeChange, false);
    //}
}
