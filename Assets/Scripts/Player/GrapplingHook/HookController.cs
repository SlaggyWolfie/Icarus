using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    private Camera _cam;
    private Vector3 _deltaFromHook;
    private bool _cannotHook = false;
    private GameObject _tempInstanceOfHook;
    private MyControl _shootHook;
    private MyControl _releaseHook;


    private float _pullSpeed;
    private float _distanceFromDetach;
    private float _legalWallShootingRange;

    private GameObject _player;
    public GameObject hookPrefab;

    public bool cannotHook
    {
        get
        {
            return _cannotHook;
        }
        set
        {
            _cannotHook = value;
        }
    }

    private void UpdateVariables ()
    {
        _pullSpeed = SettingsComponent.gl_PullSpeed;
        _distanceFromDetach = SettingsComponent.gl_distanceFromDetach;
        _legalWallShootingRange = SettingsComponent.gl_minimumHookRange;

        _shootHook = new MyControl("ShootGrapplingHook" + SettingsComponent.ControllerSuffix);
        _releaseHook = new MyControl("ReleaseGrapplingHook" + SettingsComponent.ControllerSuffix);
    }
    // Use this for initialization
    void Start ()
    {
        SettingsComponent.variableUpdatesEvent += UpdateVariables;
        UpdateVariables();

        _player = GlobalObjectManager.Instance.player;
        _cam = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        bool canPull = _tempInstanceOfHook != null && _tempInstanceOfHook.GetComponent<GrapplingHook>().attached;
        if ( canPull )
        {
            _deltaFromHook = _tempInstanceOfHook.transform.position - transform.position;   //distance from the hook to this game object - vector is facing towards the hook
            //"Collects" the hook if the player is too close to it.
            if ( _deltaFromHook.magnitude < _distanceFromDetach && _shootHook.GetAxis() == 0 )
            {
                _cannotHook = false;    //re-enables mid-air hooking because the hook is reset (destroyed)
                DestroyHook();
            }
            //----------------------------------------------------
        }
        if ( _shootHook.GetAxisBool() && !_cannotHook && !WallBlockingPath() )
        {
            if ( canPull )
            {
                RaycastHit hit = _tempInstanceOfHook.GetComponent<GrapplingHook>().objectHit;
                Vector3 deltaFromProp = _player.transform.position - hit.transform.position; //distance from the prop hit to the player - vector is facing towards the player
                if ( hit.rigidbody != null )
                {
                    hit.rigidbody.AddForce(deltaFromProp.normalized * _pullSpeed, ForceMode.Force);  //pulls the object hit towards the player
                }
                _player.GetComponent<Rigidbody>().AddForce(_deltaFromHook.normalized * _pullSpeed);  //pulls the player towards the object hit
                return;
            }
            _tempInstanceOfHook = Instantiate(hookPrefab, transform.position, _cam.transform.rotation * Quaternion.AngleAxis(90, Vector3.up)/* * Quaternion.AngleAxis(10, Vector3.forward)*/); //makes an instance of a hook
            Physics.IgnoreCollision(_tempInstanceOfHook.GetComponent<Collider>(), _player.GetComponent<Collider>(), true);  //ignores collision with player
            _cannotHook = true;     //disables mid-air hooking
        }

        if ( _releaseHook.GetAxis() > 0 && (canPull || (_tempInstanceOfHook != null && _tempInstanceOfHook.GetComponent<GrapplingHook>().canReturn)) )
        {
            _cannotHook = false;    //re-enables mid-air hooking because the hook is reset (destroyed)
            DestroyHook();
        }
    }
    /// <summary>
    /// Checks if the player is facing towards a wall. Distance from wall can be changed from the "legalShootingRangeFromWall" variable.
    /// </summary>
    /// <returns>Returns (raycast) true if the camera is facing towards a wall.</returns>
    private bool WallBlockingPath ()
    {
        RaycastHit hitInfo;
        bool raycast = Physics.Raycast(_cam.GetComponent<CrosshairComponent>().crosshairRay, out hitInfo, _legalWallShootingRange);
        return raycast && !hitInfo.collider.isTrigger;
    }

    private void DestroyHook ()
    {
        if ( _tempInstanceOfHook != null )
        {
            Destroy(_tempInstanceOfHook);
        }
    }
}
