using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrapplingHook3D : MonoBehaviour
{
    private float _speed;
    private float _maxHookDistance;
    private float _timeBeforeAllowingReturn;
    private float _stickRange;
    private Camera _cam;
    private Rigidbody _rigid;
    private bool _attached = false;
    private Vector3 _camToHookDelta;
    private RaycastHit _hit;
    //private GameObject _tempRope;

    public GameObject rope;
    private RopeLineHook _rope;

    public RaycastHit objectHit
    {
        get
        {
            return _hit;
        }
    }


    public bool attached
    {
        get
        {
            return _attached;
        }
    }

    public bool canReturn
    {
        get;
        private set;
    }

    public void UpdateVariables ()
    {
        _speed = SettingsComponent.gl_HookLaunchSpeed;
        _maxHookDistance = SettingsComponent.gl_MaxHookDistance;
        _timeBeforeAllowingReturn = SettingsComponent.gl_TimeBeforeAllowingHookReturn;
        _stickRange = SettingsComponent.gl_StickRange;
    }

    // Use this for initialization
    private void Start ()
    {
        SettingsComponent.variableUpdatesEvent += UpdateVariables;
        UpdateVariables();
        _rigid = GetComponent<Rigidbody>();
        _cam = Camera.main;
        _camToHookDelta = transform.position - _cam.transform.position;
        _rigid.velocity = _cam.GetComponent<CrosshairComponent>().crosshairRay.direction * _speed;

        _rope = GetComponent<RopeLineHook>();
        _rope.target = GlobalObjectManager.Instance.player.GetComponentInChildren<HookController3D>().gameObject.transform;

        StartCoroutine(allowHookReturn(_timeBeforeAllowingReturn));
    }

    private void FixedUpdate ()
    {
        //_tempRope.transform.position = transform.position;
        _rope.BuildRopeJoints();
        _camToHookDelta = transform.position - _cam.transform.position;
        if ( _camToHookDelta.magnitude > _maxHookDistance )
        {
            _cam.GetComponentInChildren<HookController3D>().cannotHook = false;
            Destroy(gameObject);
        }
        if ( _attached )
            return;

        //Attach hook
        if ( Physics.Raycast(new Ray(transform.position, transform.TransformDirection(Vector3.right)), out _hit, _stickRange) && !_hit.collider.isTrigger )
        {
            _rigid.velocity = Vector3.zero;     //reset velocity
            _rigid.detectCollisions = false;    //don't detect collision so we don't bounce off of the surface we've hit
            _rigid.freezeRotation = true;
            transform.position = _hit.point;     //"stick" to the surface we've hit
            if ( _hit.rigidbody != null )
            {
                HingeJoint joint = gameObject.AddComponent<HingeJoint>();
                joint.connectedBody = _hit.rigidbody;
            }
            _attached = true;
            _cam.GetComponentInChildren<HookController3D>().cannotHook = false;
        }
    }

    private IEnumerator allowHookReturn ( float pTime )
    {
        yield return new WaitForSeconds(pTime);
        canReturn = true;
    }

    private void OnDestroy ()
    {
        //Destroy(_tempRope);
    }
}
