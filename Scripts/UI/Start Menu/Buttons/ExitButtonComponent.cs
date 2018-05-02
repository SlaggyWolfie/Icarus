using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class ExitButtonComponent : MenuButtonBase {

    public override void Action ()
    {
        //Debug.Log("Stopping application...");
    #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
    #endif
        Application.Quit();
    }
}
