using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotSafeArea : MonoBehaviour
{
    private GameObject _player;
    private PlayerCollisions _playerCollisions;
    private UI_HeatOverlay _heatOverlay;

    private void Start()
    {
        _player = GlobalObjectManager.Instance.player;
        if (_player == null) Debug.LogError("WARNING: SafeArea Component missing Player");

        _playerCollisions = _player.GetComponent<PlayerCollisions>();
        if (_playerCollisions == null) Debug.LogError("WARNING: SafeArea Component missing PlayerCollisions reference");

        _heatOverlay = GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_HeatOverlay>();
        if (_heatOverlay == null) Debug.LogError("WARNING: SafeArea Component missing UI_HeatOverlay reference");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _player)
        {
            _playerCollisions.safeArea = false;
            _heatOverlay.StartHeat();
            //_heatOverlay.OnHeatTrigger(!_playerCollisions.safeArea);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _player)
        {
            _playerCollisions.safeArea = true;
            _heatOverlay.EndHeat();
            //_heatOverlay.OnHeatTrigger(!_playerCollisions.safeArea);
        }
    }
}
