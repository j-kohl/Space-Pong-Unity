using UnityEngine;
using System.Collections;

public class CameraBoundsInit : MonoBehaviour {

	public GameObject bottomBorder;
	public GameObject topBorder;
	public GameObject leftBorder;
	public GameObject rightBorder;
	
	// Use this for initialization
	void Start () {
		Camera camera = gameObject.GetComponent<Camera>();
		float cameraDistance = gameObject.transform.position.y;
      Vector3 bottomLeftCorner = camera.ViewportToWorldPoint(new Vector3(0,0,cameraDistance));
      //Vector3 bottomRightCorner = camera.ViewportToWorldPoint(new Vector3(1,0,cameraDistance));
      //Vector3 topLeftCorner = camera.ViewportToWorldPoint(new Vector3(0,1,cameraDistance));
      Vector3 topRightCorner = camera.ViewportToWorldPoint(new Vector3(1,1,cameraDistance));
		bottomBorder.transform.position = new Vector3(0,0,bottomLeftCorner.z);
		topBorder.transform.position = new Vector3(0,0,topRightCorner.z);
		leftBorder.transform.position = new Vector3(bottomLeftCorner.x,0,0);
		rightBorder.transform.position = new Vector3(topRightCorner.x,0,0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
