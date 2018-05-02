using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TriggerGameObjectActivation : BaseAction
{
    public GameObject[] targets;

    public override void Action ()
    {
        foreach ( GameObject target in targets )
        {
            if ( target != null )
                target.SetActive(!target.activeSelf);
        }
    }
    public void Activate ()
    {
        if ( gameObject != null )
        {
            gameObject.SetActive(true);
        }
    }
    public void Deactivate ()
    {
        if ( gameObject != null )
        {
            gameObject.SetActive(false);
        }
    }
}