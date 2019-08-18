using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ChangeTextColor : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler, IDeselectHandler
//kek need to import like this
{
    private Text _textComponent;
    private Color _ogColor;
    private Color _invertedColor;
    // Use this for initialization
    void Start()
    {
        _textComponent = transform.parent.GetComponentInChildren<Text>();
        if (_textComponent == null) Debug.LogError("WARNING: " + GetType().ToString() +
            " missing " + _textComponent.GetType().ToString() + " component.");

        _ogColor = _textComponent.color;

        float maxRGB = 1;
        _invertedColor = new Color(maxRGB - _ogColor.r, maxRGB - _ogColor.g, maxRGB - _ogColor.b);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _textComponent.color = _invertedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.selectedObject != gameObject) _textComponent.color = _ogColor;
    }

    public void OnSelect(BaseEventData eventData)
    {
        _textComponent.color = _invertedColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //if (eventData.currentInputModule.IsPointerOverGameObject(0))
            _textComponent.color = _ogColor;
    }
}
