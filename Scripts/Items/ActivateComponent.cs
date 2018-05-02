using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ActivateComponent : MonoBehaviour
{
    public BaseAction[] actions;

    public void ActivateAction ()
    {
        foreach ( BaseAction action in actions )
        {
            if (action != null && enabled/* && _canWork*/)
            {
                if (action is WaitForPuzzleEnd)
                {
                    WaitForPuzzleEnd w = action as WaitForPuzzleEnd;
                    w.Action();
                    if (!w.alreadyTriggered) break;
                }
                action.Action();
            }
        }
    }
}
