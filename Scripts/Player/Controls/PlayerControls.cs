using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0414
[RequireComponent(typeof(Rigidbody))]
public class PlayerControls : MonoBehaviour
{
    //[Header("JUMP NEEDS ARTISTIC FLAIR ONLY")]
    //public bool useController = true; //false for keyboard - not yet (obsolete - using global variables)

    private float _jumpRadius;
    private float _jumpBoostMax;
    private float _wallGlideSpeed;
    private float _interactDistance;
    private float _holdDistance;
    private float _holdRadiusCheck;
    private bool _physicsBasedHolding;
    private bool _buttonHold;
    private bool _useInvertedControlsUpsideDown;

    private MyControl m_interactControl;
    private MyControl m_modifierLeftButton;
    private MyControl m_jumpControl;
    private MyControl m_playerHorizontalInput;
    private MyControl m_playerVerticalInput;
    private MyControl m_objectiveHintControl;
    //private MyControl m_menuControl;

    private Jump _jump;
    private Interact _interact;
    private Hold _hold;
    private WallMovement _wallMovement;
    private UI_ObjectiveText _ui_objectiveText;
    private UI_ObjectiveIcon _ui_objectiveIcon;

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, SettingsComponent.gl_JumpRadius);
        //Gizmos.color = Color.red;
        //if (_hold.heldObject != null) Gizmos.DrawWireSphere(_hold.heldObject.transform.position, SettingsComponent.HoldRadiusCheck);
    }

    private void UpdateVariables()
    {
        _jumpRadius = SettingsComponent.gl_JumpRadius;
        _jumpBoostMax = SettingsComponent.gl_JumpBoostMax;
        _wallGlideSpeed = SettingsComponent.gl_WallGlideSpeed;
        _interactDistance = SettingsComponent.gl_InteractDistance;
        _holdDistance = SettingsComponent.gl_HoldDistance;
        _physicsBasedHolding = SettingsComponent.gl_PhysicsBasedHolding;
        _buttonHold = SettingsComponent.gl_HoldControlToHold;
        _holdRadiusCheck = SettingsComponent.gl_HoldRadiusCheck;
        _useInvertedControlsUpsideDown = SettingsComponent.gl_UseInvertedControlsUpsideDown;

        //Controls
        m_playerHorizontalInput = new MyControl("Horizontal" + SettingsComponent.ControllerSuffix);
        m_playerVerticalInput = new MyControl("Vertical" + SettingsComponent.ControllerSuffix);
        m_jumpControl = new MyControl("Jump" + SettingsComponent.ControllerSuffix);
        m_interactControl = new MyControl("Interact" + SettingsComponent.ControllerSuffix);
        m_modifierLeftButton = new MyControl("Modifier" + SettingsComponent.ControllerSuffix);
        m_objectiveHintControl = new MyControl("ObjectiveHint" + SettingsComponent.ControllerSuffix);
        //m_menuControl = new MyControl("Menu"+SettingsComponent.ControllerSuffix);
    }

    // Use this for initialization
    void Start()
    {
        SettingsComponent.variableUpdatesEvent += UpdateVariables;
        UpdateVariables();
        _jump = new Jump();
        _interact = new Interact();
        _hold = new Hold();
        _wallMovement = new WallMovement();

        _ui_objectiveText = GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_ObjectiveText>();
        if (_ui_objectiveText == null) Debug.LogError("WARNING: UI_ObjectiveText  reference missing from PlayerControls");

        _ui_objectiveIcon = GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_ObjectiveIcon>();
        if (_ui_objectiveIcon == null) Debug.LogError("WARNING: UI_ObjectiveIcon reference missing from PlayerControls");
    }

    // Update is called once per frame
    void Update()
    {
        //Interact
        if (m_interactControl.GetAxisOnce())
        {
            _interact.Activate(_interactDistance);
        }

        if (_buttonHold && m_interactControl.GetAxisBool())
        {
            _hold.HoldItem(_interactDistance, _holdDistance, _holdRadiusCheck, _physicsBasedHolding);
        }
        if (!_buttonHold && m_interactControl.GetAxisOnce())
        {
            _hold.ClickHoldItem(_interactDistance, _holdDistance, _physicsBasedHolding);
        }

        if (m_modifierLeftButton.GetAxisBool() && _hold.heldObject != null)
        {
            //Rotate Object
        }

        //Jump
        float jumpInput;
        float verticalJumpInput;
        //if ( _useInvertedControlsUpsideDown )
        //{
            if ( Camera.main.GetComponent<CameraControl>().rotationY >= SettingsComponent.gl_InvertAngle || Camera.main.GetComponent<CameraControl>().rotationY <= -SettingsComponent.gl_InvertAngle )
            {
                jumpInput = -m_jumpControl.GetAxis();
                verticalJumpInput = -m_playerVerticalInput.GetAxis();
            }
            else
            {
                jumpInput = m_jumpControl.GetAxis();
                verticalJumpInput = m_playerVerticalInput.GetAxis();
            }
        //}
        //else
        //{
        //    jumpInput = m_jumpControl.GetAxis();
        //    verticalJumpInput = m_playerVerticalInput.GetAxis();
        //}

        float jumpBoost = _jump.HoldBoostF(jumpInput, gameObject, _jumpRadius, _jumpBoostMax);
        if (jumpBoost != 0 && jumpInput == 0)
            _jump.BoostJump(gameObject, m_playerHorizontalInput.GetAxis(), verticalJumpInput);

        //Wall Movement
        _wallMovement.WallMove(verticalJumpInput, m_playerHorizontalInput.GetAxis(), gameObject, _jumpRadius, _wallGlideSpeed);

        //ObjectiveHint
        if (m_objectiveHintControl.GetAxisBool())
        {
            _ui_objectiveText.PopUp();
            _ui_objectiveIcon.PopUp();
        }
    }
}

public class MyControl
{
    private string _unityStringAxis;

    public MyControl(string inputAxis)
    {
        _unityStringAxis = inputAxis;
    }

    #region Axis Raw
    public float GetAxisRaw()
    {
        return Input.GetAxisRaw(_unityStringAxis);
    }


    public bool GetAxisRawBool()
    {
        return (Input.GetAxisRaw(_unityStringAxis) != 0);
    }

    public bool GetAxisRawOnce()
    {
        return Input.GetButtonDown(_unityStringAxis);
    }
    #endregion
    #region Axis
    public float GetAxis()
    {
        return Input.GetAxis(_unityStringAxis);
    }

    public bool GetAxisBool()
    {
        return (Input.GetAxis(_unityStringAxis) != 0);
    }

    public bool GetAxisOnce()
    {
        return Input.GetButtonDown(_unityStringAxis);
    }

    public float GetInvertedAxis ( bool pInvert )
    {
        return Input.GetAxis(_unityStringAxis) * (pInvert ? -1 : 1);
    }

    //private void Update()
    //{
    //    if (_axisInUse)
    //    {
    //        if (Time.time > _lastControlTime + _controlDelay)
    //            _axisInUse = false;
    //    }
    //}

    //public bool GetAxisOnce()
    //{
    //    if (!_axisInUse)
    //    {
    //        if (GetAxisBool())
    //        {
    //            _axisInUse = true;
    //            _lastControlTime = Time.time; //initiate cooldown
    //            return true;
    //        }
    //        else return false;
    //    }
    //    else return false;
    //}

    //public bool GetAxisOnce()
    //{
    //    if (GetAxis() != 0)
    //    {
    //        if (!_axisInUse)
    //        {
    //            _axisInUse = true;
    //            return true;
    //        }
    //        return false;
    //    }
    //    else
    //    {
    //        _axisInUse = false;
    //        return false;
    //    }
    //}
    #endregion
}
