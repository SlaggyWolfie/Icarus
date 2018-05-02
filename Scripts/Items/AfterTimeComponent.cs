using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class AfterTimeComponent : MonoBehaviour
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

    [Header("When this component is enabled it will execute after the set amount of time.")]
    public bool disableOnAwake = true;
    public float afterHowManySeconds = 5;

    [SerializeField]
    [Tooltip("Strongly recommended to not set this to DoNothing")]
    private TriggerAftermath _triggerAftermath = TriggerAftermath.DestroyThisGameObject;

    public List<BaseAction> actions;

    private void Awake()
    {
        if (disableOnAwake) enabled = false;
    }

    private void OnEnable()
    {
        StartCoroutine(MyCoroutines.StartTimer(afterHowManySeconds, OnTime));
    }

    private void OnTime()
    {
        ActivateAction();
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
                    foreach (BaseAction action in actions) if (action != null && action.isActiveAndEnabled) action.enabled = false;
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
