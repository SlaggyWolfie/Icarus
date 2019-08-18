using UnityEngine;

public class ResumeButtonComponent : MenuButtonBase
{
    public override void Action ()
    {
        PauseControl.Instance.TogglePauseGame();
    }
}
