using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamTrigger : MonoBehaviour
{
    private Collider _collider;
    private GameObject _player;
    private PlayerCollisions _pc;
    private UI_HeatOverlay _heatOverlay;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        if (_collider == null) Debug.LogError("SteamTrigger missing Collider");
        if (!_collider.isTrigger) Debug.LogError("SteamTrigger Collider is not a target");

        _player = GlobalObjectManager.Instance.player;
        if (_player == null) Debug.LogError("Player missing from SteamTrigger");

        _pc = _player.GetComponent<PlayerCollisions>();
        if (_pc == null) Debug.LogError("Player collisions missing from SteamTrigger");

        _heatOverlay = GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_HeatOverlay>();
        if (_heatOverlay == null) Debug.LogError("WARNING: SteamTrigger Component missing UI_HeatOverlay reference");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _player)
        {
            //Debug.Log("enter");
            _pc.hitBySteam = true;
            _heatOverlay.StartHeat();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _player)
        {
            //Debug.Log("exit");
            _pc.hitBySteam = false;
            _heatOverlay.EndHeat();
        }
    }
}
