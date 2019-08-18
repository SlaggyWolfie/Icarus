using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbientSound1 : MonoBehaviour {
	
    private AudioSource _audio;

	// Use this for initialization
	void Start () {
        _audio = GetComponent<AudioSource>();
	}
	
	void OnTriggerEnter (Collider other){
		
		if (other.tag == "Player") {
        
            _audio.Play();
	}
		
}
	// Update is called once per frame
	void Update () {
		
	}
}