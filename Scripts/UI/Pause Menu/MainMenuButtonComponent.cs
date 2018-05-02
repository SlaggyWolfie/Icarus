using UnityEngine.SceneManagement;

public class MainMenuButtonComponent : MenuButtonBase
{

    public override void Action ()
    {
        SceneManager.LoadScene(0);
    }
}
