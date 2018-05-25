using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

	public GameObject laserCollectible;
	public GameObject missileCollectible;
	public GameObject shieldCollectible;
	private float timeElapsed;
	// Use this for initialization
	void Start () {
		timeElapsed = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(timeElapsed < 15){
			timeElapsed += Time.deltaTime;
		}else{
			spawnItem();
			timeElapsed = 0;
		}
	}

	private void spawnItem(){
		int type = Random.Range(0, 3);
		GameObject toSpawn = laserCollectible;
		switch(type){
			case 0:
				toSpawn = laserCollectible;
				break;
			case 1:
				toSpawn = missileCollectible;
				break;
			case 2:
				toSpawn = shieldCollectible;
				break;
		}
		float xPos = Random.Range(-2.5f,2.5f);
		Instantiate(toSpawn, new Vector3(xPos,0,0), toSpawn.transform.rotation);
	}	
}
