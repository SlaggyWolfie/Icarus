using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DestroyGameObject : BaseAction
{
    public GameObject[] targets;

    public override void Action()
    {
        foreach (GameObject target in targets)
            if (target != null) Destroy(target);
    }
}
