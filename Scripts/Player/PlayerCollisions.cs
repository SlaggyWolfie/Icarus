using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth), typeof(Rigidbody))] //typeof(HUD)
public class PlayerCollisions : MonoBehaviour
{
    [Header("NEEDS TO BE FINISHED: Rope maybe hinge joints")]
    [HideInInspector]
    public float spaceDamageProbablyNot;

    [Header("Heat")]
    private float _heatDamage;
    [HideInInspector]
    public bool safeArea;

    [Header("Steam Pipes")]
    [HideInInspector]
    public bool hitBySteam;
    private float _steamDamage;
    private float _timeSinceLastDamage;
    private float _cooldownOnGettingDamaged = 1;

    private PlayerHealth _healthComponent;
    private UI_HeatOverlay _heatOverlay;
    private GameProgress _gameProgress;

    [HideInInspector]
    public bool hasGrapplingGun;

    [HideInInspector]
    public bool hasKeycard;

    [HideInInspector]
    public bool hasPowercell;

    //private Rigidbody _playerRB;

    private void UpdateVariables()
    {
        _cooldownOnGettingDamaged = SettingsComponent.gl_CooldownOnGettingDamagedInSeconds;
        _steamDamage = SettingsComponent.gl_SteamDamage;
        _heatDamage = SettingsComponent.gl_HeatDamage;
    }

    private void Start()
    {
        _healthComponent = GetComponent<PlayerHealth>();
        if (_healthComponent == null) Debug.LogError("Missing PlayerHealth Component");

        _heatOverlay = GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_HeatOverlay>();
        if (_heatOverlay == null) Debug.LogError("WARNING: PlayerCollisions Component missing UI_HeatOverlay reference");

        _gameProgress = GlobalObjectManager.Instance.gameObject.GetComponent<GameProgress>();
        if (_gameProgress == null) Debug.LogError("WARNING: PlayerCollisions Component missing GameProgress reference");

        //_playerRB = GetComponent<Rigidbody>();
        //if (_healthComponent == null) Debug.LogError("Missing Rigidbody Component");
        
        hitBySteam = false;
        safeArea = true;
        _timeSinceLastDamage = Time.time;

        SettingsComponent.variableUpdatesEvent += UpdateVariables;
        UpdateVariables();
    }

    private void FixedUpdate()
    {
        if (hitBySteam) GetDamaged_Fixed(hitBySteam, _cooldownOnGettingDamaged, _steamDamage);
        if (!safeArea) GetDamaged_Fixed(!safeArea, _cooldownOnGettingDamaged, _heatDamage);
    }

    private void LateUpdate()
    {
        if (hitBySteam || !safeArea) _gameProgress.Bonus();
        else if (_gameProgress.progressBonus != 0) _gameProgress.Cooldown();
    }

    public void GetDamagedUntil(ref bool gettingHit, float cooldown, float damage = 1)
    {
        //Update()
        if (gettingHit)
        {
            if (Time.time > _timeSinceLastDamage + cooldown)
            {
                _healthComponent.DamageHealth(damage);
                gettingHit = false;
                _timeSinceLastDamage = Time.time;
            }
        }
        else
            _timeSinceLastDamage = Time.time;
    }

    public void GetDamagedUntil_Fixed(ref bool gettingHit, float cooldown, float damage = 1)
    {
        //FixedUpdate()
        if (gettingHit)
        {
            if (Time.fixedTime > _timeSinceLastDamage + cooldown)
            {
                _healthComponent.DamageHealth(damage);
                gettingHit = false;
                _timeSinceLastDamage = Time.fixedTime;
            }
        }
        else
            _timeSinceLastDamage = Time.fixedTime;
    }

    private void GetDamaged(bool condition, float cooldown, float damage = 1)
    {
        if (condition)
        {
            if (Time.time > _timeSinceLastDamage + cooldown)
            {
                _healthComponent.DamageHealth(damage);
                _timeSinceLastDamage = Time.time;
            }
        }
        else
            _timeSinceLastDamage = Time.time;
    }

    private void GetDamaged_Fixed(bool condition, float cooldown, float damage = 1)
    {
        if (condition)
        {
            if (Time.fixedTime > _timeSinceLastDamage + cooldown)
            {
                _healthComponent.DamageHealth(damage);
                _timeSinceLastDamage = Time.fixedTime;
            }
        }
        else
            _timeSinceLastDamage = Time.fixedTime;
    }
}
