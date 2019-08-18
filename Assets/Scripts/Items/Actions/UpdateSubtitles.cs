using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Subtitles
{
    public PlaySound playSoundAction;

    [Multiline]
    public string text;
    public float duration;
}

public class UpdateSubtitles : BaseAction
{
    [SerializeField]
    private Subtitles[] _subtitles;
    private UI_Subtitles _ui_subtitles;

    private void Start()
    {
        _ui_subtitles = GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_Subtitles>();
        if (_ui_subtitles == null) Debug.LogError("WARNING: UI_Subtitles reference missing from UpdateSubtitles");
    }

    public override void Action()
    {
        _ui_subtitles.ResetSubs();
        _ui_subtitles.SetUpSubs(_subtitles);
    }
}
