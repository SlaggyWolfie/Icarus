using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#pragma warning disable 0414
public class PantingSounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip _slowPanting;
    [SerializeField]
    private AudioClip _fastPanting;

    private PlayerCollisions _pc;
    private AudioSource _playerAudioSource;
    private GameProgress _gpRef;

    //[SerializeField]
    private float _playSlowEverySeconds = 14;

    //[SerializeField]
    private float _playFastEverySeconds = 6;

    private float _timeSinceLastSound = 0;

    private float _audioSourcePitch;

    public enum PantingMode
    {
        SlowForSteamAndFastForHeat, SlowForHeatAndFastForSteam, ProgressBased, ProgressBasedWhileBeingHit
    }

    public PantingMode pantingMode = PantingMode.ProgressBasedWhileBeingHit;

    [Header("Randomization purposes")]
    [SerializeField]
    [Range(0, 1)]
    private float _minPitch = 0.5f;

    [SerializeField]
    [Range(0, 1)]
    private float _maxPitch = 1f;

    [Header("Only applies to progress based.")]
    [SerializeField]
    [Range(0, 1)]
    private float _minSlow = 0.4f;

    [SerializeField]
    [Range(0, 1)]
    private float _minFast = 0.7f;


    private void Start()
    {
        _pc = GlobalObjectManager.Instance.player.GetComponent<PlayerCollisions>();
        if (_pc == null) Debug.LogError("WARNING: " + GetType().ToString() +
            " missing " + _pc.GetType().ToString() + " reference.");

        _playerAudioSource = GlobalObjectManager.Instance.player.GetComponent<AudioSource>();
        if (_playerAudioSource == null) Debug.LogError("WARNING: " + GetType().ToString() +
            " missing " + _playerAudioSource.GetType().ToString() + " reference.");

        _audioSourcePitch = _playerAudioSource.pitch;

        _gpRef = GlobalObjectManager.Instance.gameObject.GetComponent<GameProgress>();
        if (_gpRef == null) Debug.LogError("WARNING: " + GetType().ToString() +
            " missing " + _gpRef.GetType().ToString() + " reference.");

        if (_fastPanting != null) _playFastEverySeconds = _fastPanting.length;
        if (_slowPanting != null) _playSlowEverySeconds = _slowPanting.length;

        _timeSinceLastSound = Time.time;
    }

    private void Update()
    {
        if (_slowPanting != null && _fastPanting != null)
        {
            switch (pantingMode)
            {
                case PantingMode.ProgressBased:
                    {
                        if (_minSlow != _minFast) ProgressBased(_minSlow < _minFast, false);
                        break;
                    }
                case PantingMode.ProgressBasedWhileBeingHit:
                    {
                        if (_minSlow != _minFast) ProgressBased(_minSlow < _minFast, true, _pc.hitBySteam || !_pc.safeArea);
                        break;
                    }
                case PantingMode.SlowForHeatAndFastForSteam:
                    {
                        if (_pc.hitBySteam)
                        {
                            if (Time.time > _timeSinceLastSound + _playFastEverySeconds)
                            {
                                PlaySound(_fastPanting);
                                _timeSinceLastSound = Time.time;
                            }
                        }
                        else if (!_pc.safeArea)
                        {
                            if (Time.time > _timeSinceLastSound + _playSlowEverySeconds)
                            {
                                PlaySound(_slowPanting);
                                _timeSinceLastSound = Time.time;
                            }
                        }
                        else _timeSinceLastSound = Time.time;
                        break;
                    }
                case PantingMode.SlowForSteamAndFastForHeat:
                    {
                        if (_pc.hitBySteam)
                        {
                            if (Time.time > _timeSinceLastSound + _playSlowEverySeconds)
                            {
                                PlaySound(_slowPanting);
                                _timeSinceLastSound = Time.time;
                            }
                        }
                        else if (!_pc.safeArea)
                        {
                            if (Time.time > _timeSinceLastSound + _playFastEverySeconds)
                            {
                                PlaySound(_fastPanting);
                                _timeSinceLastSound = Time.time;
                            }
                        }
                        else _timeSinceLastSound = Time.time;
                        break;
                    }

            }
        }
    }

    private void ProgressBased(bool slowSmallerThanFast, bool shouldGetHit = false, bool gettingHit = false)
    {
        if (slowSmallerThanFast)
        {
            if ((shouldGetHit && gettingHit) || (!shouldGetHit))
            {
                if (_gpRef.totalProgress >= _minFast)
                {
                    if (Time.time > _timeSinceLastSound + _playFastEverySeconds)
                    {
                        PlaySound(_fastPanting);
                        _timeSinceLastSound = Time.time;
                    }
                }
                else if (_gpRef.totalProgress >= _minSlow)
                {
                    if (Time.time > _timeSinceLastSound + _playSlowEverySeconds)
                    {
                        PlaySound(_slowPanting);
                        _timeSinceLastSound = Time.time;
                    }
                }
            }
        }
        else
        {
            if ((shouldGetHit && gettingHit) || (!shouldGetHit))
            {
                if (_gpRef.totalProgress >= _minSlow)
                {
                    if (Time.time > _timeSinceLastSound + _playSlowEverySeconds)
                    {
                        PlaySound(_slowPanting);
                        _timeSinceLastSound = Time.time;
                    }
                }
                else if (_gpRef.totalProgress >= _minFast)
                {
                    if (Time.time > _timeSinceLastSound + _playFastEverySeconds)
                    {
                        PlaySound(_fastPanting);
                        _timeSinceLastSound = Time.time;
                    }
                }
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        //Debug.Log("hello");
        float pitch = _playerAudioSource.pitch;
        _playerAudioSource.pitch = UnityEngine.Random.Range(_minPitch, _maxPitch);
        _playerAudioSource.PlayOneShot(clip);
        _playerAudioSource.pitch = pitch;
    }
}
