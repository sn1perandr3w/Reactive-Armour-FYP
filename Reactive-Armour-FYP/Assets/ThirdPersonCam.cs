using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam: MonoBehaviour
{

	public GameObject playerCamera;


	void Update()
	{


		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 50.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 50.0f;
		var y = 0;

		if(Input.GetKey("space")) {
			print("VERTICAL MOVEMENT");
			y++;
		}

		if(Input.GetKey("left ctrl")) {
			print("VERTICAL MOVEMENT");
			y--;
		}


		transform.Translate(x, 0, 0);
		transform.Translate(0, y, 0);
		transform.Translate(0, 0, z);


		//ADD PLAYER FACING CAMERA DIRECTION HERE
		//transform.Rotate();

	}
}