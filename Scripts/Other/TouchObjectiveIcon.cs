using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TouchObjectiveIcon : MonoBehaviour
{
    [Header("LEGACY: DO NOT USE")]
    public GameObject newTarget;

    [SerializeField]
    private bool _popUp = true;

    [SerializeField]
    [Tooltip("Strongly recommended for this to be true.")]
    private bool _destroyGameObjectAfterTouch = true;

    private Collider _collider;
    private UI_ObjectiveIcon _ui_objectiveIcon;
    private GameObject _playerRef;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        if (_collider == null) Debug.LogError("WARNING: TouchObjective Component missing Collider");
        if (_collider != null && !_collider.isTrigger) Debug.Log("WARNING: TouchObjective Component Collider is not a trigger");
        
        _ui_objectiveIcon = GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_ObjectiveIcon>();
        if (_ui_objectiveIcon == null) Debug.LogError("WARNING: UI_ObjectiveIcon reference missing from TouchObjective Component.");

        _playerRef = GlobalObjectManager.Instance.player;
        if (_playerRef == null) Debug.LogError("WARNING: Player reference missing from TouchObjective Component");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _playerRef)
        {
            _ui_objectiveIcon.objectiveObject = newTarget;
            if (_popUp) _ui_objectiveIcon.PopUp();
            if (_destroyGameObjectAfterTouch) Destroy(gameObject);
        }
    }
}
