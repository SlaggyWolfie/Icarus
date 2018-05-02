using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableHeatAction : BaseAction
{
    public override void Action()
    {
        GlobalObjectManager.Instance.gameObject.GetComponent<GameProgress>().StartHeat();
    }
}
