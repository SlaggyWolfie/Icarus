using UnityEngine.SceneManagement;
using UnityEngine;
public class StartButtonComponent : MenuButtonBase
{
    public GameObject settings;
    public override void Action ()
    {
        //DontDestroyOnLoad(settings);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
