using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRigidbodiesToAllChildren : MonoBehaviour
{
    private Transform[] _childrenTransforms;

    // Use this for initialization
    private void Start()
    {
        _childrenTransforms = GetComponentsInChildren<Transform>();
        foreach (Transform transform in _childrenTransforms)
        {
            if (transform == gameObject.transform || transform.gameObject.GetComponent<Rigidbody>() != null) continue;
            transform.gameObject.AddComponent<Rigidbody>().useGravity = false;
        }

        Destroy(this);
    }
}
