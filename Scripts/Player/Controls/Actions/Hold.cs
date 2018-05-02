using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Hold
{
    private CrosshairComponent _crosshair;
    public GameObject heldObject;
    //private float _radiusCheck = 0.5f;

    public Hold()
    {
        _crosshair = Camera.main.GetComponent<CrosshairComponent>();
        if (_crosshair == null) Debug.LogError("WARNING: CROSSHAIR COMPONENT MISSING FROM INTERACT");
    }

    public void HoldItem(float interactDistance, float holdDistance, float radiusCheck, bool usePhysics)
    {
        if (heldObject != null)
        {
            HoldComponent hold = heldObject.GetComponent<HoldComponent>();
            if (hold == null) Debug.LogError("HoldComponent for object missing mid holding!");
            if (!hold.isActiveAndEnabled) Debug.Log("HoldComponent for object disabled mid holding!");

            //Constantly checking in radius
            Collider[] colliders = Physics.OverlapSphere(hold.targetPosition, radiusCheck);
            if (colliders.Contains(heldObject.GetComponent<Collider>()))
            {
                hold.Hold(holdDistance, usePhysics);
                return;
            }
            else
            {
                hold.held = false;
                heldObject = null;
            }
        }

        RaycastHit info;
        if (Physics.Raycast(_crosshair.crosshairRay, out info, interactDistance))
        {
            HoldComponent hold;
            hold = info.collider.gameObject.GetComponent<HoldComponent>();
            if (hold != null && hold.isActiveAndEnabled)
            {
                hold.Hold(holdDistance, usePhysics);
                if (hold.held) heldObject = hold.gameObject;
                else heldObject = null;
            }
        }
    }

    public void ClickHoldItem(float interactDistance, float holdDistance, bool usePhysics)
    {
        if (heldObject != null)
        {
            HoldComponent hold = heldObject.GetComponent<HoldComponent>();
            if (hold == null) Debug.LogError("HoldComponent for object missing mid holding!");
            if (!hold.isActiveAndEnabled) Debug.LogError("HoldComponent for object disabled mid holding!");

            hold.held = false;
            hold.shouldHold = false;
            hold = null;
            heldObject = null;

            return;
        }

        RaycastHit info;
        if (Physics.Raycast(_crosshair.crosshairRay, out info, interactDistance))
        {
            HoldComponent hold;
            hold = info.collider.gameObject.GetComponent<HoldComponent>();
            if (hold != null && hold.isActiveAndEnabled)
            {
                hold.Hold(holdDistance, usePhysics);
                heldObject = hold.gameObject;
                hold.shouldHold = true;
            }
        }
    }
}
