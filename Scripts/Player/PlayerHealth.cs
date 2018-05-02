using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0414
public class PlayerHealth : MonoBehaviour
{
    //[SerializeField]
    private bool _canTakeDamage = true;
    //[SerializeField]
    private bool _canDie = true;
    //[SerializeField]
    private bool _healthRegenerates = true;
    
    private float _regenerationDelay = 5;
    //[SerializeField]
    private float _healthPointsRegen = 0.5f;
    //[SerializeField]
    private float _maxHealth;

    public float currentHealth;
    private bool _recentlyDamaged;
    private float _timeSinceDamage;

    private GameProgress _gpRef;
    private bool _death = false;

    //private UI_HealthOverlay healthUI;

    private void UpdateVariables ()
    {
        _maxHealth = SettingsComponent.gl_MaxHealth;
        _regenerationDelay = SettingsComponent.gl_SecondsBeforeHealthRegenerationStarts;
        _healthPointsRegen = SettingsComponent.gl_HealthPointsRegeneration;
        _canTakeDamage = SettingsComponent.gl_CanTakeDamage;
        _canDie = SettingsComponent.gl_CanDie;
        _healthRegenerates = SettingsComponent.gl_HealthRegenerates;
    }

    // Use this for initialization
    private void Start()
    {
        SettingsComponent.variableUpdatesEvent += UpdateVariables;
        UpdateVariables();
        currentHealth = _maxHealth;

        _gpRef = GlobalObjectManager.Instance.gameObject.GetComponent<GameProgress>();
        if (_gpRef == null) Debug.LogError("WARNING: " + GetType().ToString() +
            " missing " + _gpRef.GetType().ToString() + " reference.");
        //healthUI = GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_HealthOverlay>();
        //if (healthUI == null) Debug.LogError("WARNING: Health Overlay Object missing!");
    }

    private void Update()
    {
        if (_gpRef.totalProgress >= 1 && !_death)
        {
            if (GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_Heatbar>().bar.GetComponent<UnityEngine.UI.Image>().fillAmount < 1) return;
            StartCoroutine(ReturnToMenu.GoToMenu(2f));
            var topkek = GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_ObjectiveText>();
            topkek.UpdateText("YOU DIED.");
            topkek.PopUp();
            //Time.timeScale = 0;
            _death = true;
            //Game Over
            return;
        }
    }

    // Update is called once per frame
    //private void Update()
    //{
    //    if (_canDie && currentHealth <= 0)
    //    {
    //    }

    //    //Health Regen
    //    if (_healthRegenerates && !_recentlyDamaged && currentHealth <= _maxHealth)
    //    {
    //        //_currentHealth = (int)Mathf.LerpUnclamped(_currentHealth, _maxHealth, 0.1f);
    //        currentHealth += _healthPointsRegen;
    //        if (currentHealth >= _maxHealth) currentHealth = _maxHealth;
    //        //if (healthUI != null) healthUI.OnHealthChange();
    //    }

    //    if (_recentlyDamaged)
    //        if (Time.time > _timeSinceDamage + _regenerationDelay) _recentlyDamaged = false;
    //}

    public void DamageHealth(float damageNumber = 1)
    {
        if (_canTakeDamage)
        {
            currentHealth -= damageNumber;
            _recentlyDamaged = true;
            _timeSinceDamage = Time.time;
            //if (healthUI != null) healthUI.OnHealthChange();
            //Debug.Log(_currentHealth);
        }
    }
}
