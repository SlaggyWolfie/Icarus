using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private GameObject _player;
    [HideInInspector]
    public float rotationY
    {
        get
        {
            return _rotationY;
        }
    }

    private float xSensitivity;
    private float xClampAngle;

    private float ySensitivity;
    private float yClampAngle;

    private float zSensitivity;
    private float zClampAngle;

    private float _rotationX;
    private float _rotationY;
    private float _rotationZ;

    private MyControl _horizontalInput;
    private MyControl _verticalInput;
    private MyControl _rotateZInput;

    private Quaternion _originalRotation;
    private Quaternion _playerOriginalRotation;

    private void UpdateVariables ()
    {
        //Serialization
        xSensitivity = SettingsComponent.gl_CameraSensitivityX;
        xClampAngle = SettingsComponent.gl_CameraMaxAngleX;
        ySensitivity = SettingsComponent.gl_CameraSensitivityY;
        yClampAngle = SettingsComponent.gl_CameraMaxAngleY;
        zSensitivity = SettingsComponent.gl_CameraSensitivityZ;
        zClampAngle = SettingsComponent.gl_CameraMaxAngleZ;

        _horizontalInput = new MyControl("HorizontalLookAround" + SettingsComponent.ControllerSuffix);
        _verticalInput = new MyControl("VerticalLookAround" + SettingsComponent.ControllerSuffix);
        _rotateZInput = new MyControl("ZRotation" + SettingsComponent.ControllerSuffix);
    }

    // Use this for initialization
    void Start ()
    {
        SettingsComponent.variableUpdatesEvent += UpdateVariables;
        UpdateVariables();

        _player = GlobalObjectManager.Instance.player;

        _originalRotation = transform.localRotation;
        //_playerOriginalRotation = _player.transform.rotation;
        _playerOriginalRotation = _player.transform.localRotation;

        _rotationX = _originalRotation.eulerAngles.x;
        _rotationY = _playerOriginalRotation.eulerAngles.y;
        _rotationZ = _playerOriginalRotation.eulerAngles.z;
    }

    void Update ()
    {
        UpdateCameraRotation();
    }

    private void UpdateCameraRotation ()
    {
        _rotationY += _verticalInput.GetInvertedAxis(SettingsComponent.gl_InvertVerticalOrientation) * ySensitivity * Time.deltaTime;
        if ( _rotateZInput.GetAxisBool() )
        {
            _rotationZ += _horizontalInput.GetAxis() * zSensitivity * Time.deltaTime;
        }
        else
        {
            if ( SettingsComponent.gl_UseInvertedControlsUpsideDown )
            {

                if ( _rotationY >= SettingsComponent.gl_InvertAngle || _rotationY <= -SettingsComponent.gl_InvertAngle )
                {
                    _rotationX += -_horizontalInput.GetAxis() * xSensitivity * Time.deltaTime;
                }
                else
                {
                    _rotationX += _horizontalInput.GetAxis() * xSensitivity * Time.deltaTime;
                }
            }
            else
            {
                _rotationX += _horizontalInput.GetAxis() * xSensitivity * Time.deltaTime;
            }
        }

        if ( xClampAngle != 0 )
        {
            _rotationX = ClampAngle(_rotationX, -xClampAngle, xClampAngle);
        }
        else
        {
            ResetAngle(ref _rotationX);
        }
        if ( yClampAngle != 0 )
        {
            _rotationY = ClampAngle(_rotationY, -yClampAngle, yClampAngle);
        }
        else
        {
            ResetAngle(ref _rotationY);
        }
        if ( zClampAngle != 0 )
        {
            _rotationZ = ClampAngle(_rotationZ, -zClampAngle, zClampAngle);
        }
        else
        {
            ResetAngle(ref _rotationZ);
        }
        //Debug.Log("Rotation X " + _rotationX);
        //Debug.Log("Rotation Y " + _rotationY);
        //Debug.Log("Rotation Z " + _rotationZ);

        //Vector3 localNormalUp = (up.transform.position - player.transform.position).normalized;
        //Vector3 localNormalLeft = (left.transform.position - player.transform.position).normalized;
        //Vector3 localNormalForward = (forward.transform.position - player.transform.position).normalized;

        Quaternion xQuaternion = Quaternion.AngleAxis(_rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(_rotationY, Vector3.left);
        //Quaternion zQuaternion = Quaternion.AngleAxis(_rotationZ, Vector3.forward);

        _player.transform.rotation = _playerOriginalRotation * xQuaternion/* * zQuaternion*/; //rotates the player horizontally
        transform.localRotation = _originalRotation * yQuaternion; //rotates the camera vertically
    }

    public float ClampAngle ( float pAngle, float pMin, float pMax, out float pLeftOverAngle )
    {
        pLeftOverAngle = 0;
        if ( pAngle < pMin )
        {
            pLeftOverAngle = pAngle - pMin;
            pAngle = pMin;
        }
        else if ( pAngle > pMax )
        {
            pLeftOverAngle = pAngle - pMax;
            pAngle = pMax;
        }
        return pAngle;
    }

    public static float ClampAngle ( float pAngle, float pMin = 0, float pMax = 0, float pResetAngle = 360 )
    {
        if ( pAngle <= -pResetAngle )
        {
            pAngle += pResetAngle;
        }
        else if ( pAngle >= pResetAngle )
        {
            pAngle -= pResetAngle;
        }
        return Mathf.Clamp(pAngle, pMin, pMax);
    }
    private float ResetAngle ( ref float pAngle, float pResetAngle = 180)
    {
        if ( pAngle < -pResetAngle )
        {
            pAngle += pResetAngle * 2;
        }
        else if ( pAngle > pResetAngle )
        {
            pAngle -= pResetAngle * 2;
        }
        return pAngle;
    }
}