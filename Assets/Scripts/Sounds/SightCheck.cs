using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SightCheck : MonoBehaviour
{
    //public static int cutoffDistance = 10000;
    public static int cutoffDistance = 100;

    private static GameObject _player = null;
    private static Type _reference;

    private void Start()
    {
        _reference = GetType();
        if (_player != null) Debug.Log("WARNING: " + _reference + " script attached to two game objects.");
        _player = gameObject;
    }

    public static bool PlayerInSightLine(Vector3 point, float offset = 0)
    {
        if (_player == null)
        {
            Debug.Log("WARNING: " + _reference + " script not attached to a player.");
            return false;
        }

        Vector3 direction = _player.transform.position - point;

        return Physics.Linecast(point + direction.normalized * offset, direction.normalized * cutoffDistance);
    }

    public static bool PlayerInSightRay(Vector3 point, float offset = 0)
    {
        if (_player == null)
        {
            Debug.Log("WARNING: " + _reference + " script not attached to a player.");
            return false;
        }

        Vector3 direction = _player.transform.position - point;
        Ray lineOfSight = new Ray(point + direction.normalized * offset, direction);

        RaycastHit hitInfo;
        //if (Physics.Raycast(lineOfSight, out hitInfo) && hitInfo.collider.gameObject == _player)
        //    return true;
        //return false;
        if (Physics.Raycast(lineOfSight, out hitInfo, cutoffDistance) && 
            (hitInfo.collider.GetType() == typeof(BoxCollider) 
            || hitInfo.collider.GetType() == (typeof(TerrainCollider))))
            return false;
        return true;
    }
}
