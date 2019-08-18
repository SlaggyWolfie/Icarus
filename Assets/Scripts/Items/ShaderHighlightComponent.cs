using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ShaderHighlightComponent : MonoBehaviour
{
    private Shader _originalShader;
    private CrosshairComponent _crosshair;
    private Renderer _renderer;

    [Header("Tip for using: Reenable to refresh shaders.")]
    [Tooltip("Uses starting shader as the non-highlighted shader. Else uses original provided.")]
    public bool useGivenStartingShader;
    public Shader startingShader;
    public Shader highlightShader;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _originalShader = _renderer.material.shader;

        _crosshair = Camera.main.GetComponent<CrosshairComponent>();
    }

    private void OnEnable()
    {
        if (useGivenStartingShader) _renderer.material.shader = startingShader;
        else _renderer.material.shader = _originalShader;
    }

    private void Update()
    {
        if (isActiveAndEnabled)
        {
            RaycastHit info;
            if (Physics.Raycast(_crosshair.crosshairRay, out info, SettingsComponent.gl_InteractDistance)
                && info.collider.gameObject == gameObject)
                _renderer.material.shader = highlightShader;
            else
            {
                if (useGivenStartingShader) _renderer.material.shader = startingShader;
                else _renderer.material.shader = _originalShader;
            }
        }
    }
}
