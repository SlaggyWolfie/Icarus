using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class RopeLineHook : MonoBehaviour
{
    public Transform target;
    public Material material;
    public float ropeWidth = 0.5f;
    public float resolution = 0.5f;
    public float ropeDrag = 0.1f;
    public float ropeMass = 0.5f;
    public int radialSegments = 6;
    public bool startRestrained = true;
    public bool endRestrained = false;
    public bool useMeshCollision = false;

    private List<Vector3> _segmentPos = new List<Vector3>();
    private List<GameObject> _joints = new List<GameObject>();
    private GameObject _tubeRenderer;
    private TubeRenderer _line;
    private int _segments = 0;
    private bool _rope = false;
    //Joint Settings
    public Vector3 swingAxis = new Vector3(0, 1, 0);
    public float lowTwistLimit = 0.0f;
    public float highTwistLimit = 0.0f;
    public float swing1Limit = 20.0f;

    private void OnDrawGizmos()
    {
        if (target)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, target.position);
            Gizmos.DrawWireSphere((transform.position + target.position) / 2, ropeWidth);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, ropeWidth);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(target.position, ropeWidth);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, ropeWidth);
        }
    }

    public void BuildRopeJoints()
    {
        int newSegments = 0;
        int allSegments = (int)(Vector3.Distance(transform.position, target.position) * resolution);

        if (_segments == 0)
        {
            _segments = newSegments = allSegments;
            if (_segmentPos.Count < 1)
            {
                _segmentPos.Add(transform.position);
                _segmentPos.Add(target.position);
            }
        }
        else
        {
            newSegments = allSegments - _segments;
            _segments = allSegments;
        }

        while (newSegments < 0)
        {
            _segmentPos.RemoveAt(1);
            Destroy(_joints[1]);
            _joints.RemoveAt(1);

            _line.vertices = new TubeVertex[_segments];
            newSegments++;
        }

        if (newSegments == 0)
        {
            if (target != null)
            {
                // Does rope exist? If so, update its position
                if (_rope)
                {
                    _line.SetPoints(_segmentPos, ropeWidth, Color.white);
                    _line.enabled = true;
                    _segmentPos[0] = transform.position;
                    for (int s = 1; s < _segments; s++)
                    {
                        _segmentPos[s] = _joints[s].transform.position;
                    }
                }
            }
            return;
        }

        if (_tubeRenderer == null)
        {
            _tubeRenderer = new GameObject("TubeRenderer_" + gameObject.name);
            _line = _tubeRenderer.AddComponent<TubeRenderer>();
            _line.useMeshCollision = useMeshCollision;

            if (material != null)
            {
                material.SetTextureScale("_MainTex", new Vector2(1, _segments + 2));
                if (material.GetTexture("_BumpMap") != null)
                    material.SetTextureScale("_BumpMap", new Vector2(1, _segments + 2));
            }

            _line.crossSegments = radialSegments;
            _line.material = material;
        }

        _line.vertices = new TubeVertex[_segments];

        _segmentPos.InsertRange(1, new Vector3[newSegments]);
        //Ensure start and end are correct
        _segmentPos[0] = transform.position; //probably unneeded
        _segmentPos[_segmentPos.Count - 1] = target.position;
        //joints something
        AddJointPhysics(0); //Add Joint to object this script is attached to. In this case probably the hook gun

        int segs = _segments - 1;
        Vector3 seperation = (target.position - transform.position) / segs;
        for (int n = 0; n < newSegments; n++)
        {
            _segmentPos[n + 1] = (seperation * (n + 1)) + transform.position;
            AddJointPhysics(n + 1);
        }

        //for (int i = 0; i < _segments; i++)
        //{
        //    //Set and update position of segments and update joints

        //}

        CharacterJoint end = target.gameObject.GetComponent<CharacterJoint>();
        if (end == null)
        {
            end = target.gameObject.AddComponent<CharacterJoint>();
            end.connectedBody = _joints[_joints.Count - 1].transform.GetComponent<Rigidbody>();
            end.connectedBody.useGravity = false;
            end.swingAxis = swingAxis;

            end.lowTwistLimit = SetSoftJointLimitLimit(end.lowTwistLimit, lowTwistLimit);
            end.highTwistLimit = SetSoftJointLimitLimit(end.highTwistLimit, highTwistLimit);
            end.swing1Limit = SetSoftJointLimitLimit(end.swing1Limit, swing1Limit);
        }

        //target.parent = transform;
        if (endRestrained) end.GetComponent<Rigidbody>().isKinematic = true;
        if (startRestrained) transform.GetComponent<Rigidbody>().isKinematic = true;
        // Rope = true, The rope now exists in the scene!
        _rope = true;
    }

    private void AddJointPhysics(int n)
    {
        //_joints[n] = new GameObject("Joint_" + n);
        _joints.Insert(n, new GameObject("Joint_" + n));
        _joints[n].transform.parent = transform;
        Rigidbody rigid = _joints[n].AddComponent<Rigidbody>();
        if (!useMeshCollision)
        {
            SphereCollider col = _joints[n].AddComponent<SphereCollider>();
            col.radius = ropeWidth;
        }
        CharacterJoint ph = _joints[n].AddComponent<CharacterJoint>();
        ph.swingAxis = swingAxis;
        ph.lowTwistLimit = SetSoftJointLimitLimit(ph.lowTwistLimit, lowTwistLimit);
        ph.highTwistLimit = SetSoftJointLimitLimit(ph.highTwistLimit, highTwistLimit);
        ph.swing1Limit = SetSoftJointLimitLimit(ph.swing1Limit, swing1Limit);

        //ph.breakForce = ropeBreakForce; <--------------- TODO
        _joints[n].transform.position = _segmentPos[n];
        rigid.drag = ropeDrag;
        rigid.mass = ropeMass;

        if (n == 0) ph.connectedBody = transform.GetComponent<Rigidbody>();
        else ph.connectedBody = _joints[n - 1].GetComponent<Rigidbody>();
    }

    private SoftJointLimit SetSoftJointLimitLimit(SoftJointLimit sjl, float limit)
    {
        SoftJointLimit softJointLimit = sjl;
        softJointLimit.limit = limit;
        return softJointLimit;
    }
}