using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0472
#pragma warning disable 0162
public class UI_ObjectiveIcon : MonoBehaviour
{
    public GameObject objectiveObject;

    [Header("Fading in and out")]
    [SerializeField]
    private float _fadeInDuration = 0.1f;
    [SerializeField]
    private float _fadeOutDuration = 1;
    [SerializeField]
    private float _fadeOutAfter = 3;

    [Header("Size")]
    [SerializeField]
    private float _minimumSize = 0.3f;
    [SerializeField]
    private float _maximumSize = 1.5f;
    [SerializeField]
    private float _scale = 1;

    private GameObject _playerRef;

    private Image _image;
    private RectTransform _rectTransform;
    private Vector3 _originalScale;

    private Vector3 _targetPosition;

    private void Start()
    {
        _playerRef = GlobalObjectManager.Instance.player;
        if (_playerRef == null) Debug.LogError("Player reference missing!");

        _image = GetComponent<Image>();
        if (_image == null) Debug.LogError("WARNING: Image component missing from UI_ObjectiveIcon.");

        _rectTransform = GetComponent<RectTransform>();
        if (_rectTransform == null) Debug.LogError("WARNING: UI_ObjectiveIcon not attached to UI element.");

        if (!_image.isActiveAndEnabled) _image.enabled = true;

        _originalScale = _rectTransform.localScale;
        _image.CrossFadeAlpha(0, 0.001f, false);
    }

    private void LateUpdate()
    {
        if (objectiveObject != null)
        {
            _image.enabled = true;

            _rectTransform.localScale = _originalScale
                * Mathf.Clamp((_playerRef.transform.position - objectiveObject.transform.position).magnitude * _scale,
                _minimumSize * _scale, _maximumSize * _scale);

            Vector3 screenPoint;
            Rect rect = Camera.main.pixelRect;

            // checking if worldPoint isn't behind the camera:
            if (Vector3.Dot(Camera.main.transform.forward, objectiveObject.transform.position - Camera.main.transform.position) >= 0)
            {
                screenPoint = Camera.main.WorldToScreenPoint(objectiveObject.transform.position);
                _targetPosition = screenPoint;
            }
            else if (_targetPosition != null) screenPoint = _targetPosition;
            else screenPoint = Camera.main.WorldToScreenPoint(objectiveObject.transform.position);

            float minX = rect.xMin + _rectTransform.rect.width / 2 * _rectTransform.localScale.x;// * (_rectTransform.rect.width / Screen.currentResolution.width);
            float minY = rect.yMin + _rectTransform.rect.height / 2 * _rectTransform.localScale.y;// * (_rectTransform.rect.height / Screen.currentResolution.height);
            float maxX = rect.xMax - _rectTransform.rect.width / 2 * _rectTransform.localScale.x;// * (_rectTransform.rect.width / Screen.currentResolution.width);
            float maxY = rect.yMax - _rectTransform.rect.height / 2 * _rectTransform.localScale.y;// * (_rectTransform.rect.height / Screen.currentResolution.height);
            screenPoint.x = Mathf.Clamp(screenPoint.x, minX, maxX);
            screenPoint.y = Mathf.Clamp(screenPoint.y, minY, maxY);
            _rectTransform.position = screenPoint;
        }
        else _image.enabled = false;
    }


    public void PopUp(bool fadeout = true)
    {
        StopAllCoroutines();
        if (objectiveObject != null)
        {
            FadeIn();
            if (fadeout) StartCoroutine(MyCoroutines.StartTimer(_fadeOutAfter, FadeOut));
        }
    }

    private void FadeIn()
    {
        if (objectiveObject != null) _image.CrossFadeAlpha(1, _fadeInDuration, false);
    }

    public void FadeOut()
    {
        if (objectiveObject != null) _image.CrossFadeAlpha(0, _fadeOutDuration, false);
    }
}
