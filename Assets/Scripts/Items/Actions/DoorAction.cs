using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAction : BaseAction
{
    public Animator animator;
    public PlaySound doorSound;
    //private bool open = false;
    private string conditionaName;

    private PlayerCollisions _pc;

    private void Start()
    {
        conditionaName = animator.parameters[0].name;
        //open = animator.GetBool(conditionaName);

        _pc = GlobalObjectManager.Instance.player.GetComponent<PlayerCollisions>();
        if (_pc == null) Debug.LogError("WARNING: " + GetType().ToString() +
            " missing " + _pc.GetType().ToString() + " reference.");
    }

    public override void Action()
    {
        if (_pc.hasKeycard)
        {
            //open = !open;
            animator.SetBool(conditionaName, !animator.GetBool(conditionaName));
            doorSound.audioSourceToUse.PlayOneShot(doorSound.audioClipToUse);
        }
    }
}
