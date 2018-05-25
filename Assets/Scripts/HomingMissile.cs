using UnityEngine;
using System.Collections;
using System;

public class HomingMissile : MonoBehaviour {
	
	private Transform target;
	public float speed = 5f;
	public float rotateSpeed = 200f;

	private Rigidbody rb;	
	public GameObject missileMod;
	public ParticleSystem SmokePrefab; 
   ParticleSystem myParticleSystem;
	ParticleSystem.EmissionModule emissionModule;
	private GameObject parent;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		
        myParticleSystem = GetComponentInChildren<ParticleSystem>();
        emissionModule = myParticleSystem.emission;

 
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		target = GameObject.FindGameObjectWithTag("Player 2").transform;
		var targetRotation = Quaternion.LookRotation(target.position - transform.position);
 
    	rb.velocity = transform.forward * 2.5f;
		rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, 0.5f));		
	}
	private void OnTriggerEnter(Collider other) {
		bool destroyObject = false;
		if(other.gameObject.CompareTag("Player 1") && !parent.tag.Equals("Player 1")){
			destroyObject = true;
			other.gameObject.GetComponent<P1Controller>().MissileHit();
		}else if(other.gameObject.CompareTag("Player 2") && !parent.tag.Equals("Player 2")){
			destroyObject = true;
			other.gameObject.GetComponent<P2Controller>().MissileHit();			
		}
		else if(other.gameObject.CompareTag("Top Border")||
		other.gameObject.CompareTag("Bottom Border")||
		other.gameObject.CompareTag("Left Border")||
		other.gameObject.CompareTag("Right Border")){
			destroyObject = true;
		}	

		if(destroyObject){
			emissionModule.rateOverTime = 0.0f;
			Destroy(missileMod.gameObject);
			Destroy(gameObject,10);
		}
	}

   internal void SetParent(GameObject gameObject)
   {
      parent = gameObject;
   }
}
