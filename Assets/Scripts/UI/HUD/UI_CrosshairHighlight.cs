using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CrosshairHighlight : MonoBehaviour
{
    private Image _crosshair;
    private RectTransform _rectTransform;
    private readonly Quaternion quaternionZero = Quaternion.Euler(Vector3.zero);
    private Quaternion _fallbackQuaternion;

    private Sprite _originalSprite;
    public Sprite outOfRangeCrosshair;
    public Sprite tooCloseCrosshair;
    private Vector3 _originalScale;
    [Tooltip("Will reduce the scale to that much.")]
    public float scale = 0.5f;
    public float scaleModifier = 1.2f;
    public float rotationSpeed = 1;

    private void Start()
    {
        _crosshair = GetComponent<Image>();
        _originalSprite = _crosshair.sprite;
        if (_crosshair == null) Debug.LogError("WARNING: Crosshair image missing");

        _rectTransform = GetComponent<RectTransform>();
        if (_rectTransform == null) Debug.LogError("WARNING: UI_CrosshairRotate not attached to UI element");

        _fallbackQuaternion = quaternionZero;
        _rectTransform.localScale *= scale;
        _originalScale = _rectTransform.localScale;
        //_currentRotation = 0;
    }

    private void LateUpdate()
    {
        //if (_crosshair.sprite == _originalSprite)
        //{
            RaycastHit hitInfo;
            if
                (Physics.Raycast(Camera.main.GetComponent<CrosshairComponent>().crosshairRay, out hitInfo, SettingsComponent.gl_MaxHookDistance)
                && hitInfo.collider.GetComponent<Rigidbody>() != null && hitInfo.collider.gameObject.GetComponent<GrapplingHook>() == null
                && hitInfo.collider.GetComponent<Rigidbody>().mass < GlobalObjectManager.Instance.player.GetComponent<Rigidbody>().mass)
            {
                _rectTransform.localRotation *= Quaternion.Euler(new Vector3(0, 0, rotationSpeed));
                _rectTransform.localScale = Vector3.Lerp(_rectTransform.localScale, _originalScale * scaleModifier, 0.1f);
            }
            else if (_rectTransform.localRotation != _fallbackQuaternion || _rectTransform.localScale != _originalScale)
            {
                _rectTransform.localRotation = Quaternion.RotateTowards(_rectTransform.localRotation, FindQuaternion(), rotationSpeed);
                _rectTransform.localScale = Vector3.Lerp(_rectTransform.localScale, _originalScale, 0.1f);

                if (Mathf.Abs(_rectTransform.localRotation.eulerAngles.z % 90) < 0.1f)
                {
                    _rectTransform.localRotation = quaternionZero;
                    _fallbackQuaternion = quaternionZero;
                }
            }
        //}
        if (!Physics.Raycast(Camera.main.GetComponent<CrosshairComponent>().crosshairRay, SettingsComponent.gl_MaxHookDistance))
        {
            //_rectTransform.localScale = Vector3.MoveTowards(_rectTransform.localScale, _originalScale, 0.1f);
            //_rectTransform.localRotation = quaternionZero;
            _crosshair.sprite = outOfRangeCrosshair;
        }
        else if (Physics.Raycast(Camera.main.GetComponent<CrosshairComponent>().crosshairRay, SettingsComponent.gl_minimumHookRange))
        {
            //_rectTransform.localRotation = quaternionZero;
            _crosshair.sprite = tooCloseCrosshair;
        }
        else
        {
            _crosshair.sprite = _originalSprite;
        }
        //else _fallbackQuaternion = quaternionZero;
    }

    //private void LateUpdate()
    //{
    //    RaycastHit hitInfo;
    //    if (Physics.Raycast(Camera.main.GetComponent<CrosshairComponent>().crosshairRay, out hitInfo, SettingsComponent.MaxHookDistance)
    //        && hitInfo.collider.GetComponent<Rigidbody>() != null && hitInfo.collider.GetComponent<Rigidbody>().mass <= GlobalObjectManager.Instance.player.GetComponent<Rigidbody>().mass / 2)
    //    {
    //        _currentRotation += rotationSpeed;
    //        _rectTransform.localRotation *= Quaternion.AngleAxis(rotationSpeed, Vector3.forward);
    //        FixAngleQuaternion();
    //    }
    //    else _rectTransform.localRotation = Quaternion.RotateTowards(_rectTransform.localRotation, quaternionZero, rotationSpeed);
    //}

    //private void FixAngleQuaternion()
    //{
    //    if (_currentRotation > 90)
    //    {
    //        _currentRotation = CameraControl.ClampAngle(_currentRotation, 0, 90, 90);
    //        _rectTransform.localRotation = Quaternion.Euler(0, 0, _currentRotation);
    //    }
    //}

    private Quaternion FindQuaternion()
    {
        if (_fallbackQuaternion == quaternionZero)
        {
            float angle;
            angle = _rectTransform.rotation.eulerAngles.z;
            float angleToRotateTo = Mathf.Sign(angle) * Mathf.Round(Mathf.Abs(angle) / 90) * 90;
            _fallbackQuaternion = Quaternion.AngleAxis(angleToRotateTo, Vector3.forward);
        }

        return _fallbackQuaternion;
    }
}
