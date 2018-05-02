using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActivateComponent))]
public class WaitForPuzzleEnd : BaseAction
{
    [HideInInspector]
    public bool alreadyTriggered = false;

    public override void Action()
    {
        if (!alreadyTriggered && MemoryComponent.puzzleCompleted) alreadyTriggered = true;
    }
}
