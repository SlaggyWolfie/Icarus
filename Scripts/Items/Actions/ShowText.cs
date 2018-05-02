using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowText : BaseAction
{
    [Header("Only displays text on the Console for now.")]
    [Multiline()]
    public string text = "";

    public override void Action()
    {
        PrintText();
    }

    public void PrintText()
    {
        //Just Testing
        Debug.Log(text);
    }
    public void PrintError ()
    {
        Debug.LogError(text);
    }
}
