using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[DisallowMultipleComponent]
public class OnInput : BaseAction
{
    [Header("Will execute these actions on the input provided. Input string is useless if Any Key.")]

    public bool anyKey = true;
    public string inputString = "";

    private bool _shouldCheck = false;

    public List<BaseAction> actions;

    private void Update()
    {
        if (_shouldCheck)
        {
            if ((anyKey && Input.anyKeyDown) || (!anyKey && Input.GetAxis(inputString) != 0))
            {
                ActivateAction();
                _shouldCheck = false;
                enabled = false;
            }
        }
    }

    private void ActivateAction()
    {
        foreach (BaseAction action in actions)
        {
            if (action != null && action.isActiveAndEnabled)
                action.Action();
        }
    }

    public override void Action()
    {
        _shouldCheck = true;
    }
}
