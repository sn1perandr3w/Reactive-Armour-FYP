using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class civCrowdEntity : MonoBehaviour {

	Animator anim;
	AnimatorStateInfo info;

	public GameObject targetWayPoint;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		info = anim.GetCurrentAnimatorStateInfo (0);

		if (info.IsName ("moving")) 
		{
			GetComponent<UnityEngine.AI.NavMeshAgent> ().destination = targetWayPoint.transform.position;
			GetComponent<UnityEngine.AI.NavMeshAgent> ().Resume ();//Unity5  version
			//GetComponent<NavMeshAgent> ().isStopped = false;//Unity5  version

		}

	}


}
