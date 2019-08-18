using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MaterialHighlightComponent : MonoBehaviour
{
    private Material _originalMaterial;
    private CrosshairComponent _crosshair;
    private Renderer _renderer;

    [Header("Tip for using: Reenable to refresh materials.")]
    [Tooltip("Uses starting material as the non-highlighted material. Else uses original provided.")]
    public bool useGivenStartingMaterial;
    public bool mouseOverHighlight;
    public Material startingMaterial;
    public Material highlightMaterial;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _originalMaterial = _renderer.material;

        _crosshair = Camera.main.GetComponent<CrosshairComponent>();
    }

    private void OnEnable()
    {
        if (useGivenStartingMaterial) _renderer.material = startingMaterial;
        else _renderer.material = _originalMaterial;
    }

    private void Update()
    {
        if (isActiveAndEnabled && mouseOverHighlight)
        {
            RaycastHit info;
            if (Physics.Raycast(_crosshair.crosshairRay, out info, SettingsComponent.gl_InteractDistance)
                && info.collider.gameObject == gameObject)
                _renderer.material= highlightMaterial;
            else
            {
                if (useGivenStartingMaterial) _renderer.material = startingMaterial;
                else _renderer.material = _originalMaterial;
            }
        }
    }

    public void Highlight ()
    {
        _renderer.material = highlightMaterial;
    }
    public void DeHighlight ()
    {
        if ( useGivenStartingMaterial )
            _renderer.material = startingMaterial;
        else
            _renderer.material = _originalMaterial;
    }
}
