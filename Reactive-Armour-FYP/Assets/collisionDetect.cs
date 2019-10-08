using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionDetect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		print("The scene has started.");
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		string nameOfObject = hit.collider.gameObject.name;
		if(nameOfObject == "redBox")
		{
			Destroy(hit.collider.gameObject);
		}
	}
}
