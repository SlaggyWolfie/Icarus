using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[DisallowMultipleComponent]
public class AfterTimeAction : BaseAction
{
    public float afterHowManySeconds = 5;
    public List<BaseAction> actions;

    public override void Action()
    {
        //StopAllCoroutines();
        StartCoroutine(MyCoroutines.StartTimer(afterHowManySeconds, ActivateAction));
    }

    private void ActivateAction()
    {
        foreach (BaseAction action in actions)
        {
            if (action != null/* && action.isActiveAndEnabled*/)
                action.Action();
        }
        //StopAllCoroutines();
    }

    public void ResetAfterTime()
    {
        StopAllCoroutines();
    }
}
