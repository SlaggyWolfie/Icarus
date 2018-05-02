using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LiquidMachine : ActivateComponent
{
    [Header("Do this on activate and +1 the counter.")]
    public List<BaseAction> actionsOnActivate = null;

    [SerializeField]
    private int _hits = 0;

    [Header("Do this on necessary amount of hits.")]
    public List<BaseAction> actionsOnSuccess = null;
    

}
