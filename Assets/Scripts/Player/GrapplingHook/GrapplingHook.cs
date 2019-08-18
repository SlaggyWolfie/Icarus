using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrapplingHook : MonoBehaviour
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
    private GameObject _tempRope;

    public GameObject rope;

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

    public delegate void OnAttach(bool attached, RaycastHit objectHit);
    public event OnAttach OnAttachEvent;

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
        RaycastHit aimHit;
        Physics.Raycast(_cam.GetComponent<CrosshairComponent>().crosshairRay, out aimHit, _maxHookDistance);
        _rigid.velocity = Camera.main.GetComponent<CrosshairComponent>().crosshairRay.direction.normalized * _speed;
        _tempRope = Instantiate(rope);
        StartCoroutine(allowHookReturn(_timeBeforeAllowingReturn));
    }

    private void FixedUpdate ()
    {
        _tempRope.transform.position = transform.position;
        _camToHookDelta = transform.position - _cam.transform.position;
        if ( _camToHookDelta.magnitude > _maxHookDistance )
        {
            _cam.GetComponentInChildren<HookController>().cannotHook = false;
            Destroy(gameObject);
        }
        if ( _attached )
            return;

        //Attach hook
        if ( Physics.Raycast(new Ray(transform.position, transform.TransformDirection(Vector3.left)), out _hit, _stickRange) && !_hit.collider.isTrigger && _hit.collider != Camera.main.GetComponentInParent<PlayerControls>().GetComponent<Collider>() )
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
            _cam.GetComponentInChildren<HookController>().cannotHook = false;

            if (OnAttachEvent != null) OnAttachEvent(attached, objectHit);
        }
    }

    private IEnumerator allowHookReturn ( float pTime )
    {
        yield return new WaitForSeconds(pTime);
        canReturn = true;
    }

    private void OnDestroy ()
    {
        Destroy(_tempRope);
    }
}
