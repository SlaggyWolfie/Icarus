using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AddComponentsToObject : BaseAction
{
    public bool useThis = true;
    public GameObject target;
    public Component[] components;

    public override void Action()
    {
        GameObject go = (useThis) ? gameObject : target;
        if (go != null) foreach (Component component in components) gameObject.AddComponent(component.GetType());
    }
}
