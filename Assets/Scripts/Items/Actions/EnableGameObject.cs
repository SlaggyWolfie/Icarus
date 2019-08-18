using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EnableGameObject : BaseAction
{
    public GameObject[] targets;

    public override void Action()
    {
        foreach (GameObject target in targets)
            if (target != null && !target.activeSelf) target.SetActive(true);
    }
}
