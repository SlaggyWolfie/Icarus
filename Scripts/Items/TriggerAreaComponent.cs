using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class TriggerAreaComponent : MonoBehaviour
{
    public enum TriggerAftermath
    {
        DoNothing,
        DestroyThisComponent, DisableThisComponent,
        DestroyThisGameObject, DisableThisGameObject,
        RemoveActionsFromList,
        DisableThisComponentAndRemoveAllActions,
        DisableThisComponentAndAllActions,
        DestroyThisComponentAndAllActions

    };

    [SerializeField]
    [Tooltip("Strongly recommended to not set this to DoNothing")]
    private TriggerAftermath _triggerAftermath = TriggerAftermath.DestroyThisGameObject;

    //[SerializeField]
    //[Tooltip("Strongly recommended for this to be true.")]
    //private bool _destroyGameObjectAfterTouch = true;

    private Collider _collider;
    private GameObject _playerRef;
    public List<BaseAction> actions;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        if (_collider == null) Debug.LogError("WARNING: TriggerArea Component missing Collider");
        if (_collider != null && !_collider.isTrigger) Debug.Log("WARNING: TriggerArea Component Collider is not a trigger");


        _playerRef = GlobalObjectManager.Instance.player;
        if (_playerRef == null) Debug.LogError("WARNING: Player reference missing from TriggerArea Component");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _playerRef)
        {
            ActivateAction();
            //enabled = false;
            //if (_destroyGameObjectAfterTouch) Destroy(gameObject);
            switch (_triggerAftermath)
            {
                case TriggerAftermath.DestroyThisComponent: Destroy(this); break;
                case TriggerAftermath.DisableThisComponent: enabled = false; break;
                case TriggerAftermath.DestroyThisGameObject: Destroy(gameObject); break;
                case TriggerAftermath.DisableThisGameObject: gameObject.SetActive(false); break;
                case TriggerAftermath.RemoveActionsFromList: actions.Clear(); break;

                case TriggerAftermath.DisableThisComponentAndRemoveAllActions:
                    {
                        actions.Clear();
                        enabled = false;
                        break;
                    }
                case TriggerAftermath.DisableThisComponentAndAllActions:
                    {
                        foreach (BaseAction action in actions) if (action.isActiveAndEnabled) action.enabled = false;
                        enabled = false;
                        StartCoroutine(MyCoroutines.StartTimer(0.1f, Disable)); //ensure it's disabled
                        break;
                    }
                case TriggerAftermath.DestroyThisComponentAndAllActions:
                    {
                        foreach (BaseAction action in actions) Destroy(action);
                        Destroy(this);
                        break;
                    }
            }
        }
    }

    private void ActivateAction()
    {
        foreach (BaseAction action in actions)
        {
            if (action != null && action.isActiveAndEnabled)
                action.Action();
        }
    }

    private void Disable()
    {
        enabled = false;
    }
}
