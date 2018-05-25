using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

	private ParticleSystem ring;
   ParticleSystem.EmissionModule emissionModule;
	private ParticleSystem.MinMaxCurve originalEmissionRate; 

	// Use this for initialization
	void Start () {
		ring = gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
		emissionModule = ring.emission;
		originalEmissionRate = emissionModule.rateOverTime;
		emissionModule.rateOverTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other) {
		if(other.tag.Equals("Ball")){
			emissionModule.rateOverTime = originalEmissionRate;
		}

		Destroy(this,2);
	}
}
