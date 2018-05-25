using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

	private ParticleSystem explosion;
	private ParticleSystem ring;
   private bool hit = false;
	// Use this for initialization
	void Start () {
		explosion = gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
		ring = gameObject.GetComponent<ParticleSystem>();
		explosion.Stop();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other) {
		if(other.tag.Equals("Ball")){
			if(!hit){
				explosion.Play();
				ring.Stop(false);
				hit = true;
			}
		}
		Destroy(gameObject,2);
	}
}
