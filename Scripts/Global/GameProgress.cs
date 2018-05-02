using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgress : MonoBehaviour
{
    [SerializeField]
    private int _timeUntilGameOver = 900;

    [SerializeField]
    [Range(0, 0.5f)]
    private float _increaseRate = 0.05f;

    [SerializeField]
    [Range(0, 0.5f)]
    private float _cooldownRate = 0.1f;

    private float _progressBonus = 0;
    private float _timeHeatWasStarted = 0;
    private bool _heatActive = false;

    public float progressBonus
    {
        get
        {
            return _progressBonus;
        }
    }

    private float _timeBarProgress
    {
        get
        {
            return (Time.time - _timeHeatWasStarted) / _timeUntilGameOver;
        }
    }

    public float totalProgress
    {
        get
        {
            float progress = 0;
            if (_heatActive) progress = _timeBarProgress + _progressBonus;
            return progress;
        }
    }

    public void Cooldown()
    {
        _progressBonus = Mathf.MoveTowards(_progressBonus, 0, _cooldownRate);
    }

    public void Bonus()
    {
        //_progressBonus++;
        //_progressBonus = Mathf.Clamp(_progressBonus, 0, 100 - _timeBarProgress);
        _progressBonus = Mathf.MoveTowards(_progressBonus, 1f - _timeBarProgress, _increaseRate);
    }

    public void StartHeat()
    {
        _heatActive = true;
        _timeHeatWasStarted = Time.time;
    }
}
