using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSelectionImages : MonoBehaviour {
    [SerializeField]
    private Sprite _PCImage;
    [SerializeField]
    private Sprite _PCImageHighlight;
    
    [SerializeField]
    private Sprite _XBOXImage;
    [SerializeField]
    private Sprite _XBOXImageHighlight;

    [SerializeField]
    private Sprite _PSImage;
    [SerializeField]
    private Sprite _PSImageHighlight;

    private void Start()
    {
        SettingsComponent.variableUpdatesEvent += UpdateVariables;
    }

    // Update is called once per frame
    private void UpdateVariables () {
		if (!SettingsComponent.gl_isUsingController)
        {
            foreach (Image image in GetComponentsInChildren<Image>()) image.sprite = _PCImage;
            foreach (Button button in GetComponentsInChildren<Button>())
                button.spriteState = SetHighlightedImage(GetComponentInChildren<Button>().spriteState, _PCImageHighlight);
        }
        else
        {
            if (SettingsComponent.gl_ControllerType == SettingsComponent.ControllerTypes.XBOX)
            {
                foreach (Image image in GetComponentsInChildren<Image>()) image.sprite = _XBOXImage;
                foreach (Button button in GetComponentsInChildren<Button>())
                    button.spriteState = SetHighlightedImage(GetComponentInChildren<Button>().spriteState, _XBOXImageHighlight);
            }
            else if (SettingsComponent.gl_ControllerType == SettingsComponent.ControllerTypes.PS4)
            {
                foreach (Image image in GetComponentsInChildren<Image>()) image.sprite = _PSImage;
                foreach (Button button in GetComponentsInChildren<Button>())
                    button.spriteState = SetHighlightedImage(GetComponentInChildren<Button>().spriteState, _PSImageHighlight);
            }
        }
	}

    private SpriteState SetHighlightedImage(SpriteState spr_st, Sprite sprite)
    {
        SpriteState spr = spr_st;
        spr.highlightedSprite = sprite;
        return spr;
    }
}
