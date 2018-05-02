using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class HoldComponent : MonoBehaviour
{
    private readonly float snapDistance = 0.05f;
    private readonly float slowDownModifier = 0.95f;
    private readonly float moveForce = 10;

    private bool _physics;
    private float _distanceFromPlayer = 1;
    private readonly float speedToTransition = 0.1f;

    private GameObject _holder;
    private Rigidbody _rigidbody;
    private CrosshairComponent _crosshair;

    [HideInInspector]
    public bool held = false;
    [HideInInspector]
    public bool shouldHold = false;

    //Used in Hold.cs (Eh)
    [HideInInspector]
    public Vector3 targetPosition;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _crosshair = Camera.main.GetComponent<CrosshairComponent>();

        //_holder = Camera.main.transform.parent.gameObject;

        //Basically a type check
        _holder = Camera.main.GetComponentInParent<PlayerControls>().gameObject;
    }

    private void Update()
    {
        if (shouldHold) Hold(_distanceFromPlayer, _physics);
    }

    public void Hold(float distanceFromPlayer = 1, bool physics = true)
    {
        //_rb.isKinematic = true;
        //if (_distanceFromPlayer != distanceFromPlayer)
        _distanceFromPlayer = distanceFromPlayer;
        _physics = physics;

        Ray ray = _crosshair.crosshairRay;
        targetPosition = _holder.transform.position + ray.direction.normalized * _distanceFromPlayer;
        Vector3 delta = (targetPosition - gameObject.transform.position);
        if (_physics)
        {
            //Vector3 delta = (targetPosition - gameObject.transform.position);
            _rigidbody.AddForce(delta * moveForce, ForceMode.Acceleration);
            _rigidbody.velocity *= slowDownModifier;
        }
        else
        {
            _rigidbody.isKinematic = true;
            gameObject.transform.position = Vector3.MoveTowards
                (gameObject.transform.position, targetPosition, speedToTransition);
            _rigidbody.isKinematic = false;
        }

        delta = targetPosition - gameObject.transform.position;
        if (delta.magnitude < snapDistance / 2)
        {
            gameObject.transform.position = targetPosition;
            held = true;
            return;
        }
        held = false;
        //_rb.isKinematic = false;
    }

    private void Comments()
    {

        //Debug.Log((targetPosition - holder.transform.position).magnitude);
        //Debug.DrawLine(holder.transform.position, targetPosition, Color.red, 20);
        //targetPosition += holder.transform.rotation.eulerAngles;
        // - _crosshair.transform.rotation.eulerAngles;
        //Debug.Log(ray.direction.normalized);
        //Debug.Log(ray.direction.normalized.magnitude);
        //Vector3 nextPosition = Vector3.Lerp(gameObject.transform.position, targetPosition, 0.2f);
        //gameObject.transform.Translate(nextPosition, Space.World);
        //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, speedToTransition);
        //_rb.MovePosition(Vector3.MoveTowards(gameObject.transform.position, targetPosition, speedToTransition));


        //string test = (_holder.transform.position.Equals(targetPosition)).ToString();
        //Debug.Log(test);
        ////Debug.Log("Direction: " + ray.direction.normalized);
        ////Debug.Log("Holder: " + _holder.transform.position);
        ////Debug.Log("TargetPos: " + targetPosition);
        //Debug.DrawLine(_holder.transform.position, targetPosition, Color.red, 20);
    }
}
