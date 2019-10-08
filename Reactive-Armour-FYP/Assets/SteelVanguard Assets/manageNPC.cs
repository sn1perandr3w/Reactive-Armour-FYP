using UnityEngine;
using System.Collections;

public class manageNPC : MonoBehaviour {

	private int health;
	public GameObject smoke;

	void Start () {
		health = 100;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (health <= 0) {
			destroy ();
		}
	}

	public void gotHit(int amount)
	{
		health -= amount;
	}

	public void destroy()
	{
		GameObject lastSmoke = (GameObject)(Instantiate (smoke, transform.position, Quaternion.identity));
		Destroy (lastSmoke, 3);
		Destroy (gameObject);
	}
}
