using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlaySound : BaseAction
{
    public AudioSource audioSourceToUse;
    public AudioClip audioClipToUse;

    public override void Action()
    {
        audioSourceToUse.PlayOneShot(audioClipToUse);
    }
}