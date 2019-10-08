using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemSpawnScript : MonoBehaviour {

	public List<GameObject> itemSpawnPoints;

	public GameObject healthPack;
	public GameObject ammoPack;

	// Use this for initialization
	void Start () {

		foreach (GameObject itemSpawnPoint in GameObject.FindGameObjectsWithTag("itemSpawnPoint")) {
			itemSpawnPoints.Add (itemSpawnPoint);
		}

		GenerateFromText ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void GenerateFromText()
	{
		TextAsset t1 = (TextAsset)Resources.Load("items", typeof(TextAsset));
		string s = t1.text;
		for(int i = 0; i < s.Length; i++)
		{
			int randSpawn = Random.Range(0,2);
			if (s [i] == '1' && i < itemSpawnPoints.Count && randSpawn == 1) 
			{
				GameObject g = (GameObject)Instantiate (healthPack, itemSpawnPoints[i].transform.position, Quaternion.identity);	
			}else
				if (s [i] == '2' && i < itemSpawnPoints.Count && randSpawn == 1) 
			{
					GameObject g = (GameObject)Instantiate (ammoPack, itemSpawnPoints[i].transform.position, Quaternion.identity);
			}
	}


}
}