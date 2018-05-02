using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Interact
{
    private CrosshairComponent _crosshair;

    public Interact()
    {
        _crosshair = Camera.main.GetComponent<CrosshairComponent>();
        if (_crosshair == null) Debug.LogError("WARNING: CROSSHAIR COMPONENT MISSING FROM INTERACT");
    }

    //public void Activate()
    //{
    //    //need to do a test anyway
    //    RaycastHit info;
    //    if (Physics.Raycast(_crosshair.crosshairRay, out info, 1))
    //    {
    //        ActivateComponent activate;
    //        activate = info.collider.gameObject.GetComponent<ActivateComponent>();
    //        if (activate != null && activate.isActiveAndEnabled) activate.ActivateAction();
    //    }
    //}

    public void Activate(float interactDistance)
    {
        //need to do a test anyway
        RaycastHit info;
        if (Physics.Raycast(_crosshair.crosshairRay, out info, interactDistance))
        {
            ActivateComponent activate;
            activate = info.collider.gameObject.GetComponent<ActivateComponent>();
            if (activate != null && activate.isActiveAndEnabled) activate.ActivateAction();
        }
    }
}
