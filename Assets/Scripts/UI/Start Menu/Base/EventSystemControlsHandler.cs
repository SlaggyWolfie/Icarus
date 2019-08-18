using System;
using UnityEngine;
using UnityEngine.EventSystems;
public class EventSystemControlsHandler : MonoBehaviour
{
    private GameObject _lastSelectedObject;
    private StandaloneInputModule _inputSystem;
    private EventSystem _eventSystem;
    private void Start ()
    {
        _inputSystem = gameObject.GetComponent<StandaloneInputModule>();
        _eventSystem = gameObject.GetComponent<EventSystem>();
        SettingsComponent.variableUpdatesEvent += UpdateVariables;
        UpdateVariables();
    }


    public void Update ()
    {
        if ( _eventSystem.currentSelectedGameObject == null )
        {
            _eventSystem.SetSelectedGameObject(_lastSelectedObject);
        }
        else
        {
            _lastSelectedObject = _eventSystem.currentSelectedGameObject;
        }
        //if ( Input.GetButtonDown(_inputSystem.cancelButton) )
        //{
        //    transform.root.gameObject.SetActive(false);

        //    GameObject.FindGameObjectWithTag("StartMenu").SetActive(true);
        //}
    }

    public void UpdateVariables ()
    {
        if ( SettingsComponent.gl_isUsingController )
        {
            _inputSystem.horizontalAxis = "Horizontal" + SettingsComponent.ControllerSuffix;
            _inputSystem.verticalAxis = "Vertical" + SettingsComponent.ControllerSuffix;
            _inputSystem.submitButton = "MenuSubmit" + SettingsComponent.ControllerSuffix;
            _inputSystem.cancelButton = "MenuCancel" + SettingsComponent.ControllerSuffix;
        }
        else
        {
            _inputSystem.horizontalAxis = "Horizontal";
            _inputSystem.verticalAxis = "Vertical";
            _inputSystem.submitButton = "MenuSubmit";
            _inputSystem.cancelButton = "MenuCancel";
        }
    }

}
