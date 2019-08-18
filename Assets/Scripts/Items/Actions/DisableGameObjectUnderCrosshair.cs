using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DisableGameObjectUnderCrosshair : BaseAction
{
    public override void Action()
    {
        Camera.main.GetComponent<CrosshairComponent>().
            GetRaycastHitInfo().collider.gameObject.SetActive(false);
    }
}
