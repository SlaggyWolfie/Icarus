using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController3D : MonoBehaviour
{

    private Transform _hookGun;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private LineRenderer _lineRender;

    ////Rope properties
    //public float resolution = 0.5f;
    //public float ropeMass = 0.1f;
    //public float ropeColliderRadius = 0.5f;
    ////Calculation properties
    //private Vector3[] segmentPositions;
    //private GameObject[] joints;
    //private int segments = 0;
    ////Joints' properties
    //public Vector3 jointSwingAxis = new Vector3(1, 1, 1);
    //public float lowTwistLimit = -100f;
    //public float highTwistLimit = 100f;
    //public float swingLimit = 20f;

    // Use this for initialization
    void Start ()
    {
        _hookGun = Camera.main.GetComponentInChildren<HookController>().transform;
        _startPosition = _hookGun.position;
        _endPosition = transform.position;
        BuildRope();
    }
    //UNFINISHED
    private void BuildRope ()
    {
        _lineRender = GetComponent<LineRenderer>();
        //Non physics
        _lineRender.positionCount = 2;
        _lineRender.startWidth = 0.5f;
        _lineRender.endWidth = 0.5f;

        //Physics attempt
        //segments = (int)(Vector3.Distance(_startPosition, _endPosition) * resolution);
        //_lineRender.positionCount = segments;
        //segmentPositions = new Vector3[segments];
        //joints = new GameObject[segments];
        //segmentPositions[0] = _startPosition;
        //segmentPositions[segments - 1] = _endPosition;

        //Vector3 distanceBetweenSegs = (_endPosition - _startPosition) / (segments - 1);

        //for ( int currentSegment = 0; currentSegment < segments; currentSegment++ )
        //{
        //    Vector3 jointPos = (distanceBetweenSegs * currentSegment) + _startPosition;
        //    segmentPositions[currentSegment] = jointPos;
        //    AddJointPhysics(currentSegment);
        //}

        //CharacterJoint endJoint = _hookGun.gameObject.AddComponent<CharacterJoint>();
        //endJoint.connectedBody = joints[joints.Length - 1].transform.GetComponent<Rigidbody>();
        //endJoint.swingAxis = jointSwingAxis;
        //SetLimits(endJoint);
        //transform.parent = _hookGun;

    }

    //Rope physics
    //private void AddJointPhysics (int pCurrentSegment)
    //{
    //    joints[pCurrentSegment] = new GameObject("Joint_" + pCurrentSegment);
    //    joints[pCurrentSegment].transform.parent = _hookGun;
    //    Rigidbody jointRigidbody = joints[pCurrentSegment].AddComponent<Rigidbody>();
    //    SphereCollider jointCollider = joints[pCurrentSegment].AddComponent<SphereCollider>();
    //    CharacterJoint jointRagdoll = joints[pCurrentSegment].AddComponent<CharacterJoint>();
    //    jointRagdoll.swingAxis = jointSwingAxis;
    //    //Sets limits for the ragdoll
    //    SetLimits(jointRagdoll);

    //    joints[pCurrentSegment].transform.position = segmentPositions[pCurrentSegment];

    //    jointRigidbody.mass = ropeMass;
    //    jointCollider.radius = ropeColliderRadius;

    //    if ( pCurrentSegment == 1 )
    //    {
    //        jointRagdoll.connectedBody = transform.GetComponent<Rigidbody>();
    //    }
    //    else
    //    {
    //        jointRagdoll.connectedBody = joints[pCurrentSegment - 1].GetComponent<Rigidbody>();
    //    }

    //}

    //private void SetLimits (CharacterJoint pJoint)
    //{
    //    SoftJointLimit limit_setter = pJoint.lowTwistLimit;
    //    limit_setter.limit = lowTwistLimit;
    //    pJoint.lowTwistLimit = limit_setter;
    //    limit_setter = pJoint.highTwistLimit;
    //    limit_setter.limit = highTwistLimit;
    //    pJoint.highTwistLimit = limit_setter;
    //    limit_setter = pJoint.swing1Limit;
    //    pJoint.swing1Limit = limit_setter;
    //}

    // Update is called once per frame
    void FixedUpdate ()
    {
        UpdatePosition();
    }

    private void UpdatePosition ()
    {
        _startPosition = _hookGun.position;
        _endPosition = transform.position;
        _lineRender.SetPosition(0, _startPosition);
        _lineRender.SetPosition(_lineRender.positionCount - 1, _endPosition);
    }
}
