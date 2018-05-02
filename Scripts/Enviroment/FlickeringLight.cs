using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour {

	public Light flickeringLight;
	// public Light secondFlashinglight;
	private float randomNumber;

	// Use this for initialization
	void Start () {
		flickeringLight.enabled = false;
		//secondFlashingLight.enabled = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		randomNumber = Random.value;

		if (randomNumber <= 0.95f) {
			flickeringLight.enabled = true;
			// secondFlashingLight.enabled = true;
		} else {

			flickeringLight.enabled = false;
			// secondFlashingLight.enabled = false;
		}
	}
}
