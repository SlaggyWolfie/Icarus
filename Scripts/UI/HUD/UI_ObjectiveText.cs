using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ObjectiveText : MonoBehaviour
{
    private Text _textComponent;

    [SerializeField]
    private float _fadeInDuration = 0.1f;
    [SerializeField]
    private float _fadeOutDuration = 5;
    [SerializeField]
    private float _fadeOutAfter = 10;

    private void Awake()
    {
        _textComponent = GetComponent<Text>();
        if (_textComponent == null) Debug.LogError("WARNING: " + GetType().ToString() +
            " missing " + _textComponent.GetType().ToString() + " component.");
        if (!_textComponent.isActiveAndEnabled) _textComponent.enabled = true;
        _textComponent.CrossFadeAlpha(0, 0, false);
    }

    public void UpdateText(string text = "")
    {
        if (text.Contains("[Controls]"))
        {
            string controlType = "";

            if (!SettingsComponent.gl_isUsingController) controlType = "Use the mouse";
            else controlType = "Use the right thumbstick";

            text = text.Replace("[Controls]", controlType);
        }

        _textComponent.text = text;
    }

    public void PopUp()
    {
        StopAllCoroutines();
        FadeIn();
        StartCoroutine(MyCoroutines.StartTimer(_fadeOutAfter, FadeOut));
    }

    public void FadeIn()
    {
        _textComponent.CrossFadeAlpha(1, _fadeInDuration, false);
    }

    public void FadeOut()
    {
        _textComponent.CrossFadeAlpha(0, _fadeOutDuration, false);
    }
}
