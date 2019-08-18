using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateObjectObjective : BaseAction
{
    [SerializeField]
    private GameObject _target;
    public bool popup = true;

    private UI_ObjectiveIcon _ui_objectiveIcon;

    private void Start()
    {
        _ui_objectiveIcon = GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_ObjectiveIcon>();
        if (_ui_objectiveIcon == null) Debug.LogError("WARNING: UI_ObjectiveIcon reference missing from UpdateObjective.");
    }

    public override void Action()
    {
        _ui_objectiveIcon.objectiveObject = _target;
        if (popup) _ui_objectiveIcon.PopUp();
    }

    public void Action(GameObject target, bool popup = true)
    {
        _ui_objectiveIcon.objectiveObject = target;
        if (popup) PopUpIcon();
    }

    //public void UpdateObjectiveIcon(GameObject target, bool popup = true)
    //{
    //    _ui_objectiveIcon.objectiveObject = target;
    //    if (popup) _ui_objectiveIcon.PopUp();
    //}

    public void PopUpIcon()
    {
        _ui_objectiveIcon.PopUp();
    }

    //public static void UpdateObject(GameObject target, bool popup = true)
    //{
    //    new QuestObjective().UpdateObjectiveIcon(target, popup);
    //}

    //public static void PopUpObject()
    //{
    //    new QuestObjective().PopUpIcon();
    //}
}
