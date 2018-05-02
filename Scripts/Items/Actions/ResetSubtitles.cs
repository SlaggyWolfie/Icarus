using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSubtitles : BaseAction
{
    private UI_Subtitles _ui_subtitles;

    private void Start()
    {
        _ui_subtitles = GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_Subtitles>();
        if (_ui_subtitles == null) Debug.LogError("WARNING: UI_Subtitles reference missing from UpdateSubtitles");
    }

    public override void Action()
    {
        _ui_subtitles.ResetSubs();
    }
}
