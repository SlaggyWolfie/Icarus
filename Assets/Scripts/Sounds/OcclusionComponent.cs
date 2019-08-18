using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OcclusionComponent : MonoBehaviour {

    private Transform _targetTransform;
    private AudioSource _audioSource;
    private AudioLowPassFilter _audioLPF;

    public bool affectReverb = false;
    [Tooltip("If it exists.")]
    public bool affectLowPassFilter = false;

    public float lerpModifier = 0.7f;

    [Header("Normal Values")]
    [Range(0, 1)]
    public float normalVolume = 1;
    [Range(0, 1.1f)]
    public float normalReverb = 1;
    [Range(0, 22000)]
    public float normalCutOff = 5000;

    [HideInInspector]
    public bool occluded = false;
    [Header("Occlusion")]
    [Range(0, 1)]
    public float occludedVolume = 0.34f;
    [Range(0, 1.1f)]
    public float occludedReverb = 0.66f;
    [Range(0, 22000)]
    public float occludedCutOff = 2000;

    private void Start()
    {
        _targetTransform = gameObject.GetComponent<Transform>();
        _audioSource = gameObject.GetComponent<AudioSource>();
        _audioLPF = gameObject.GetComponent<AudioLowPassFilter>();
    }

    private void FixedUpdate()
    {
        UpdateOcclusion();
    }

    public void UpdateOcclusion()
    {
        occluded = isActiveAndEnabled && !SightCheck.PlayerInSightRay(_targetTransform.position, 0);
        //Debug.Log("Volume: " + _audioSource.volume);
        //Debug.Log("Occluded: " + occluded);
        if (occluded)
        {
            _audioSource.volume = occludedVolume;
            if (affectReverb) _audioSource.reverbZoneMix = occludedReverb;
            if (affectLowPassFilter && _audioLPF != null) _audioLPF.cutoffFrequency = occludedCutOff;
        }
        else
        {
            _audioSource.volume = normalVolume;
            if (affectReverb) _audioSource.reverbZoneMix = normalReverb;
            if (affectLowPassFilter && _audioLPF != null) _audioLPF.cutoffFrequency = normalCutOff;
        }
    }

    public void UpdateOcclusionLerp()
    {
        occluded = isActiveAndEnabled && !SightCheck.PlayerInSightLine(_targetTransform.position, 0);
        //occluded = isActiveAndEnabled && !SightCheck.PlayerInSightRay(_targetTransform.position, 0);
        //Debug.Log("Volume: " + _audioSource.volume);
        if (occluded)
        {
            _audioSource.volume = Mathf.Lerp(_audioSource.volume, occludedVolume, lerpModifier);
            if (affectReverb) _audioSource.reverbZoneMix = Mathf.Lerp(_audioSource.reverbZoneMix, occludedReverb, lerpModifier);
            if (affectLowPassFilter && _audioLPF != null) _audioLPF.cutoffFrequency = Mathf.Lerp(_audioLPF.cutoffFrequency, occludedCutOff, lerpModifier);
        }
        else
        {
            _audioSource.volume = Mathf.Lerp(_audioSource.volume, normalVolume, lerpModifier); ;
            if (affectReverb) _audioSource.reverbZoneMix = Mathf.Lerp(_audioSource.reverbZoneMix, normalReverb, lerpModifier);
            if (affectLowPassFilter && _audioLPF != null) _audioLPF.cutoffFrequency = Mathf.Lerp(_audioLPF.cutoffFrequency, normalCutOff, lerpModifier);
        }
    }
}
