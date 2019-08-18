using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Other
{
    /// <summary>
    /// Returns true if not null and prints this data to the console if it is null.
    /// </summary>
    /// <param name="toBeChecked"></param>
    /// <param name="objectRef"></param>
    /// <returns>False if null, true if not. </returns>
    public static bool NullCheck(UnityEngine.Object toBeChecked, UnityEngine.Object objectRef)
    {
        bool _bool = (toBeChecked != null);
        if (!_bool) Debug.LogError("WARNING: " + toBeChecked.GetType().ToString() +
            " missing " + objectRef.GetType().ToString() + " reference.");
        return _bool;

    }
}

public class MyCoroutines
{
    public static IEnumerator StartTimer(float seconds = 1)
    {
        yield return new WaitForSeconds(seconds);
    }

    public static IEnumerator StartTimer(float seconds, System.Action action)
    {
        yield return new WaitForSeconds(seconds);
        action.Invoke();
    }
}

public class ComponentStatus
{
    public static void SetComponentEnabled(Component component, bool value = true)
    {
        if (component == null) return;
        if (component is Renderer)
        {
            (component as Renderer).enabled = value;
        }
        else if (component is Collider)
        {
            (component as Collider).enabled = value;
        }
        else if (component is Animation)
        {
            (component as Animation).enabled = value;
        }
        else if (component is Animator)
        {
            (component as Animator).enabled = value;
        }
        else if (component is AudioSource)
        {
            (component as AudioSource).enabled = value;
        }
        else if (component is Behaviour)
        {
            (component as Behaviour).enabled = value;
        }
        else
        {
            Debug.Log("Don't know how to enable " + component.GetType().Name);
        }
    }

    public static bool IsEnabled(Component component)
    {
        bool value = false;
        if (component == null)
        {
            Debug.Log("Component is null.");
            return value;
        }
        if (component is Renderer)
        {
            value = (component as Renderer).enabled;
        }
        else if (component is Collider)
        {
            value = (component as Collider).enabled;
        }
        else if (component is Animation)
        {
            value = (component as Animation).enabled;
        }
        else if (component is Animator)
        {
            value = (component as Animator).enabled;
        }
        else if (component is AudioSource)
        {
            value = (component as AudioSource).enabled;
        }
        else if (component is Behaviour)
        {
            value = (component as Behaviour).enabled;
        }
        else
        {
            Debug.Log("Don't know if enabled " + component.GetType().Name);
        }

        return value;
    }
}

