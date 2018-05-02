using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TouchObjectiveItem : MonoBehaviour
{
    [Header("LEGACY: DO NOT USE")]
    [SerializeField]
    private string _newText = "";

    [SerializeField]
    private bool _popUp = true;

    [SerializeField]
    [Tooltip("Strongly recommended for this to be true.")]
    private bool _destroyGameObjectAfterTouch = true;

    private Collider _collider;
    private UI_ObjectiveText _ui_objectiveText;
    private GameObject _playerRef;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        if (_collider == null) Debug.LogError("WARNING: TouchObjective Component missing Collider");
        if (_collider != null && !_collider.isTrigger) Debug.Log("WARNING: TouchObjective Component Collider is not a trigger");

        _playerRef = GlobalObjectManager.Instance.player;
        if (_playerRef == null) Debug.LogError("WARNING: Player reference missing from TouchObjective Component");

        _ui_objectiveText = GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_ObjectiveText>();
        if (_ui_objectiveText == null) Debug.LogError("WARNING: UI_ObjectiveText  reference missing from UpdateObjective");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _playerRef)
        {
            _ui_objectiveText.UpdateText(_newText);
            if (_popUp) _ui_objectiveText.PopUp();
            if (_destroyGameObjectAfterTouch) Destroy(gameObject);
        }
    }
}
