using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestHUD : MonoBehaviour
{
    private PlayerHealth _ph;
    private RectTransform _rt;
    private float _originalWidth;
    private float _originalHeight;
    private float _idealPercentage;
    private float _currentPercentage;
    private float _lerpModifier = 0.01f;

    // Use this for initialization
    void Start()
    {
        _rt = gameObject.GetComponent<RectTransform>();
        _originalWidth = _rt.rect.width;
        _originalHeight = _rt.rect.height;
        //GetComponent<Image>().sprite
        //Image image = GetComponent<Image>();
        //image.Cross
        _ph = GlobalObjectManager.Instance.player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        _idealPercentage = 1 - _ph.currentHealth / SettingsComponent.gl_MaxHealth;
        float width = _originalWidth * _idealPercentage;
        float height = _originalHeight * _idealPercentage;
        //float width = _originalWidth * _currentPercentage;
        //float height = _originalHeight * _currentPercentage;
        //float width = Mathf.Max(_rt.rect.width - 1, 0);
        //float height = Mathf.Max(_rt.rect.height - 1, 0);

        _rt.sizeDelta = new Vector2(Mathf.Lerp(_rt.rect.width, width, _lerpModifier), Mathf.Lerp(_rt.rect.height, height, _lerpModifier));
        //_currentPercentage = width / _originalWidth;
    }
}
