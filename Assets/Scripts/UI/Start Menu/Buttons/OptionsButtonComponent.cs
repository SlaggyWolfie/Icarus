using System;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0414
public class OptionsButtonComponent : MenuButtonBase {

    [SerializeField]
    private GameObject _background;
    [SerializeField]
    private Sprite _optionsBackgroundSprite;
    private Sprite _originalBackgroundSprite;
    private static bool _showingOptions = false;

    private void Start ()
    {
        _originalBackgroundSprite = _background.GetComponent<Image>().sprite;
    }

    public override void Action ()
    {

    }
}
