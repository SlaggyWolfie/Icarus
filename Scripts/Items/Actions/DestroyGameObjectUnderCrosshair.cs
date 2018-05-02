using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DestroyGameObjectUnderCrosshair : BaseAction
{
    public override void Action()
    {
        Destroy(Camera.main.GetComponent<CrosshairComponent>().
            GetRaycastHitInfo().collider.gameObject);
    }
}
