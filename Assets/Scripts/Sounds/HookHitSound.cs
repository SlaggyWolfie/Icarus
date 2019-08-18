using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class HookHitSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip _wallHitSound;

    [SerializeField]
    private AudioClip _hitSound;

    private AudioSource _as;
    private GrapplingHook _gh;

    private void Start()
    {
        _as = GetComponent<AudioSource>();
        if (_as == null) Debug.LogError("WARNING: " + GetType().ToString() +
            " missing " + _as.GetType().ToString() + " reference.");

        _gh = GetComponent<GrapplingHook>();
        Other.NullCheck(_gh, this);

        _gh.OnAttachEvent += OnAttach;
    }

    private void OnAttach(bool attached, RaycastHit objectHit)
    {
        if (attached)
        {
            if (objectHit.collider.GetComponent<Rigidbody>() != null)
            {
                if (_hitSound != null)
                {
                    _as.PlayOneShot(_hitSound);
                }
            }
            else if (_wallHitSound != null)
            {
                _as.PlayOneShot(_wallHitSound);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<Rigidbody>() != null)
        {
            if (_hitSound != null)
            {
                _as.PlayOneShot(_hitSound);
            }
        }
        else if (_wallHitSound != null)
        {
            _as.PlayOneShot(_wallHitSound);
        }
    }
}