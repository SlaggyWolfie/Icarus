using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonComponent : BaseAction {


    public override void Action ()
    {
        MemoryComponent.buttonPressed = gameObject;
    }
}
