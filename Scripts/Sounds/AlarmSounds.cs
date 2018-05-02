using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmSounds : MonoBehaviour
{
    private AudioSource _audioSource = null;

    private bool _goingDown = true;
    //private float _maxPitch = 1;

    [SerializeField]
    private float _maxVolume = 0.5f;
    [SerializeField]
    private float _minVolume = 0.1f;
    [SerializeField]
    private bool _rollOff = false;
    [SerializeField]
    private float _rollOffInterval = 0.1f;

    // Use this for initialization
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        Debug.Assert(_audioSource != null, "Alarm Audio Source Null");

        _audioSource.volume = _maxVolume;
        //_maxVolume = _audioSource.volume / 2;
        //_maxPitch = _audioSource.pitch;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (_rollOff)
        {
            if (_goingDown)
            {
                _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, _minVolume, _rollOffInterval);
                //_audioSource.pitch = Mathf.MoveTowards(_audioSource.pitch, 0, _rollOffInterval);

                if (_audioSource.volume <= _minVolume + float.Epsilon) _goingDown = false;
            }
            else
            {
                _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, _maxVolume, _rollOffInterval);
                //_audioSource.pitch = Mathf.MoveTowards(_audioSource.pitch, _maxPitch, _rollOffInterval);

                if (_audioSource.volume >= _maxVolume - float.Epsilon) _goingDown = true;
            }
        }
    }
}
