using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

#pragma warning disable 0649
public class AddGameObject : BaseAction
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private bool simpleInstatiation = false;

    [Header("Advanced")]
    [SerializeField]
    private Transform parent;
    [SerializeField]
    private bool instantiateInWorldSpace = true;

    [SerializeField]
    private bool usePosition;

    [SerializeField]
    private bool useRotation;

    [SerializeField]
    private bool useScale;

    [SerializeField]
    private Vector3 position;

    [SerializeField]
    private Vector3 rotation;

    [SerializeField]
    private Vector3 scale;


    public override void Action()
    {
        if (simpleInstatiation)
        {
            Instantiate(target);
            return;
        }
        GameObject go = Instantiate(target, parent, instantiateInWorldSpace);
        if (usePosition) go.transform.position = position;
        if (useRotation) go.transform.rotation = Quaternion.Euler(rotation);
        if (useScale) go.transform.localScale = scale;
    }
}
