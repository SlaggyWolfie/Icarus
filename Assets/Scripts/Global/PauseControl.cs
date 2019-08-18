using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseControl : MonoBehaviour
{
    public GameObject pauseMenu;
    private GameObject _hud;
    private GameObject _tempMenu;
    private MyControl _pauseControl;
    private float previousTimeScale;
    private bool _paused;

    private void Start ()
    {
        //if (SceneManager.GetActiveScene().buildIndex != SceneManager.GetSceneAt(0).buildIndex)
            _hud = GlobalObjectManager.Instance.HUDCanvas;
        SettingsComponent.variableUpdatesEvent += UpdateVariables;
        UpdateVariables();

    }
    private void UpdateVariables ()
    {
        _pauseControl = new MyControl("Pause" + SettingsComponent.ControllerSuffix);
    }

    // Update is called once per frame
    private void Update ()
    {
        //Debug.Log(SceneManager.GetActiveScene().buildIndex);
        if (SceneManager.GetActiveScene().name != "Start Menu") // SceneManager.GetSceneByName().name//SceneManager.GetSceneAt(0).buildIndex)
        {
            if ( _pauseControl.GetAxisOnce() )
            {
                TogglePauseGame();
            }

            if ( _tempMenu == null && _paused )
            {
                _tempMenu = Instantiate(pauseMenu);
                _hud.SetActive(false);
            }
            else if ( _tempMenu != null && !_paused )
            {
                Destroy(_tempMenu);
                _hud.SetActive(true);
            }
        }

    }
    public void TogglePauseGame ()
    {

        if ( !_paused )
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
            _paused = true;
        }
        else
        {
            Time.timeScale = previousTimeScale;
            _paused = false;
        }
    }

    public static PauseControl Instance
    {
        get;
        private set;
    }

    private void OnEnable ()
    {
        //Do not use with Awake (they do the same sort of)
        Instance = this;
    }
}
