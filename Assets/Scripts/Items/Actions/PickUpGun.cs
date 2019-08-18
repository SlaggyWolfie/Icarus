using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGun : BaseAction
{
    private PlayerCollisions _pc;

    public override void Action()
    {
        _pc.hasGrapplingGun = true;
    }

    // Use this for initialization
    void Start()
    {
        _pc = GlobalObjectManager.Instance.player.GetComponent<PlayerCollisions>();
        if (_pc == null) Debug.LogError("WARNING: " + GetType().ToString() +
            " missing " + _pc.GetType().ToString() + " reference.");
    }
}
