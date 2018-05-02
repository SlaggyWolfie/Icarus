using System;
using UnityEngine;

public class PopUpObjective : BaseAction
{
    [SerializeField]
    private bool _popUpText = true;
    [SerializeField]
    private bool _popUpIcon = true;

    private UI_ObjectiveText _ui_objectiveText;
    private UI_ObjectiveIcon _ui_objectiveIcon;

    private void Start()
    {
        _ui_objectiveText = GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_ObjectiveText>();
        if (_ui_objectiveText == null) Debug.LogError("WARNING: UI_ObjectiveText reference missing from PopUpObjective");

        _ui_objectiveIcon = GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_ObjectiveIcon>();
        if (_ui_objectiveIcon == null) Debug.LogError("WARNING: UI_ObjectiveIcon reference missing from PopUpObjective");
    }

    public void PopUpText()
    {
        _ui_objectiveText.PopUp();
    }
    public void PopUpIcon()
    {
        _ui_objectiveIcon.PopUp();
    }

    public override void Action()
    {
        if (_popUpText) PopUpText();
        if (_popUpIcon) PopUpIcon();
    }
}
