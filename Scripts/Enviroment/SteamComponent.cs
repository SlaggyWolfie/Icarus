using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SteamComponent : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private PlayerCollisions _playerCollisions;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        if (_particleSystem == null) Debug.LogError("WARNING: Particle system missing from Game Object attached to SteamComponent");

        //Design choice maybe; may won't work
        //_particleSystem.trigger.SetCollider(1, GlobalObjectManager.Instance.player.GetComponent<Collider>());
        _playerCollisions = GlobalObjectManager.Instance.player.GetComponent<PlayerCollisions>();
    }

    private void OnParticleTrigger()
    {
        if (isActiveAndEnabled && _playerCollisions != null) _playerCollisions.hitBySteam = true;
    }
}
