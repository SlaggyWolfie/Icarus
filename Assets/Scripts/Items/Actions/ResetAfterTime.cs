using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[DisallowMultipleComponent]
public class ResetAfterTime : BaseAction
{
    public GameObject target;

    public override void Action()
    {
        //StopAllCoroutines();
        foreach (AfterTimeAction ata in target.GetComponents<AfterTimeAction>()) ata.StopAllCoroutines();
    }
}
