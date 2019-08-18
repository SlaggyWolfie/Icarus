using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DestroyComponents : BaseAction
{
    public Component[] targets;

    public override void Action()
    {
        foreach (Component target in targets)
            if (target != null) Destroy(target);
    }
}
