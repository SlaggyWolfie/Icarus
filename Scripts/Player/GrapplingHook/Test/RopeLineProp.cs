using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RopeLineProp : MonoBehaviour
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
    // Private Variables (Only change if you know what your doing)
    private Vector3[] _segmentPos;
    private GameObject[] _joints;
    private GameObject _tubeRenderer;
    private TubeRenderer _line;
    private int _segments = 4;
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

    private void Awake()
    {
        if (target != null) BuildRope();
        else Debug.LogError("You must have a gameobject attached to target: " + this.name, this);
    }

    private void LateUpdate()
    {
        if (target)
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
    }

    private void BuildRope()
    {
        _tubeRenderer = new GameObject("TubeRenderer_" + gameObject.name);
        _line = _tubeRenderer.AddComponent<TubeRenderer>();
        _line.useMeshCollision = useMeshCollision;
        // Find the amount of segments based on the distance and resolution
        // Example: [resolution of 1.0f = 1 joint per unit of distance]
        _segments = (int)(Vector3.Distance(transform.position, target.position) * resolution);
        if (material != null)
        {
            material.SetTextureScale("_MainTex", new Vector2(1, _segments + 2));
            if (material.GetTexture("_BumpMap") != null)
                material.SetTextureScale("_BumpMap", new Vector2(1, _segments + 2));
        }
        _line.vertices = new TubeVertex[_segments];
        _line.crossSegments = radialSegments;
        _line.material = material;
        _segmentPos = new Vector3[_segments];
        _joints = new GameObject[_segments];
        _segmentPos[0] = transform.position;
        _segmentPos[_segments - 1] = target.position;
        // Find the distance between each segment
        int segs = _segments - 1;
        Vector3 seperation = (target.position - transform.position) / segs;
        for (int s = 0; s < _segments; s++)
        {
            // Find the each segments position using the slope from above
            Vector3 vector = (seperation * s) + transform.position;
            _segmentPos[s] = vector;
            if (s == 0) _segmentPos[s] = transform.position;
            if (s == segs) _segmentPos[s] = target.position;
            //Add Physics to the segments
            AddJointPhysics(s);
        }
        // Attach the joints to the target object and parent it to this object
        CharacterJoint end = target.gameObject.AddComponent<CharacterJoint>();
        end.connectedBody = _joints[_joints.Length - 1].transform.GetComponent<Rigidbody>();
        end.connectedBody.useGravity = false;
        end.swingAxis = swingAxis;
        //end.lowTwistLimit.limit = lowTwistLimit;
        //end.highTwistLimit.limit = highTwistLimit;
        //end.swing1Limit.limit = swing1Limit;

        end.lowTwistLimit = SetSoftJointLimitLimit(end.lowTwistLimit, lowTwistLimit);
        end.highTwistLimit = SetSoftJointLimitLimit(end.highTwistLimit, highTwistLimit);
        end.swing1Limit = SetSoftJointLimitLimit(end.swing1Limit, swing1Limit);

        //target.parent = transform;
        if (endRestrained)
        {
            end.GetComponent<Rigidbody>().isKinematic = true;
        }
        if (startRestrained)
        {
            transform.GetComponent<Rigidbody>().isKinematic = true;
        }
        // Rope = true, The rope now exists in the scene!
        _rope = true;
    }

    private void AddJointPhysics(int n)
    {
        _joints[n] = new GameObject("Joint_" + n);
        _joints[n].transform.parent = transform;
        Rigidbody rigid = _joints[n].AddComponent<Rigidbody>();
        if (!useMeshCollision)
        {
            SphereCollider col = _joints[n].AddComponent<SphereCollider>();
            col.radius = ropeWidth;
        }
        CharacterJoint ph = _joints[n].AddComponent<CharacterJoint>();
        ph.swingAxis = swingAxis;

        //SoftJointLimit lowTwistLimitCopy = ph.lowTwistLimit;
        //lowTwistLimitCopy.limit = lowTwistLimit;
        //ph.lowTwistLimit = lowTwistLimitCopy;

        ////ph.lowTwistLimit.limit = lowTwistLimit;
        ////ph.highTwistLimit.limit = highTwistLimit;
        //ph.swing1Limit.limit = swing1Limit;

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