using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningLights : MonoBehaviour {

	public Light warningLight;
	public float delay;
	public float minIntensity;
	public float maxIntensity;
	public bool startAtMin;
	private float timeElapsed;

	// Use this for initialization
	void Start () {
		warningLight = GetComponent<Light> ();
		if (warningLight != null) {
			warningLight.intensity = startAtMin ? minIntensity : maxIntensity;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if (warningLight != null) {
			
			timeElapsed += Time.deltaTime;
		
			if (timeElapsed >= delay) {
			
				timeElapsed = 0;

				ToggleLight ();
			}
		}
	}
	public void ToggleLight() {
	
		if (warningLight != null) {
		
			if(warningLight.intensity == minIntensity) {
			
				warningLight.intensity = maxIntensity;
			}
			else if (warningLight.intensity == maxIntensity) {
			
				warningLight.intensity = minIntensity;
			}
		}
	}
}