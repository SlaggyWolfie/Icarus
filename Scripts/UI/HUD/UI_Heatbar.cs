using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Heatbar : MonoBehaviour
{
    public float fillUp = 0.001f;
    public GameObject bar;
    public GameObject barBackground;
    private Image _barImage;
    private Image _barBackgroundImage;

    private GameProgress _gameProgress;

    // Use this for initialization
    private void Start()
    {
        _barImage = bar.GetComponent<Image>();
        if (_barImage == null) Debug.LogError("WARNING: Bar Image missing in UI_Heatbar");
        else
        {
            _barImage.type = Image.Type.Filled;
            _barImage.fillMethod = Image.FillMethod.Horizontal;
            _barImage.fillOrigin = 0;
            _barImage.fillAmount = 0;
        }

        _barBackgroundImage = barBackground.GetComponent<Image>();
        if (_barBackgroundImage == null) Debug.LogError("WARNING: Bar Image Background missing in UI_Heatbar");

        _gameProgress = GlobalObjectManager.Instance.gameObject.GetComponent<GameProgress>();
        if (_gameProgress == null) Debug.LogError("WARNING: UI_Heatbar missing GameProgress reference");
    }

    private void LateUpdate()
    {
        //Debug.Log(_gameProgress.totalProgress);
        //_barImage.fillAmount = Mathf.Lerp(_barImage.fillAmount, _gameProgress.totalProgress, fillUp);
        //_barImage.fillAmount = _gameProgress.totalProgress;
        _barImage.fillAmount = Mathf.MoveTowards(_barImage.fillAmount, _gameProgress.totalProgress, fillUp);
    }
}
