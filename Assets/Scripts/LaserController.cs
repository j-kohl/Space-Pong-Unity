using UnityEngine;
using System.Collections;

public class LaserController : MonoBehaviour {

	public float speed;
	private GameObject parent;
	void FixedUpdate() {
		
	}

	void Start(){
		Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
		rigidbody.velocity = transform.forward * speed;
	}

	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Player 1") && !parent.tag.Equals("Player 1")){
			Destroy(gameObject);
			other.gameObject.GetComponent<P1Controller>().LaserHit();
		}else if(other.gameObject.CompareTag("Player 2") && !parent.tag.Equals("Player 2")){
			Destroy(gameObject);
			other.gameObject.GetComponent<P2Controller>().LaserHit();			
		}
		else if(other.gameObject.CompareTag("Top Border")||
		other.gameObject.CompareTag("Bottom Border")){
			Destroy(gameObject,2);
		}	
	}

	internal void SetParent(GameObject gameObject)
   {
      parent = gameObject;
   }
}
