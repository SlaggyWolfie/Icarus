using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateTextObjective : BaseAction
{
    [Multiline()]
    public string text = "";
    public bool popup = false;

    private UI_ObjectiveText  _ui_objectiveText;

    private void Start()
    {
        _ui_objectiveText = GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_ObjectiveText >();
        if (_ui_objectiveText == null) Debug.LogError("WARNING: UI_ObjectiveText reference missing from UpdateObjective");
    }

    public override void Action()
    {
        //Debug.Log("Every time you get called");
        _ui_objectiveText.UpdateText(text);
        if (popup) PopUp();
    }

    public void Action(string text, bool popup = true)
    {
        _ui_objectiveText.UpdateText(text);
        if (popup) _ui_objectiveText.PopUp();
    }

    public void PopUp()
    {
        _ui_objectiveText.PopUp();
    }
}
