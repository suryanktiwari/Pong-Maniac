using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionCorrector : MonoBehaviour {

	public GameObject player;

	void Start () {
		
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Ball") {
		//	print ("idhar bhi aya tha control");
			col.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			player.GetComponent<BoxCollider> ().isTrigger = true;
			Invoke ("cancelTrigger", 0.5f);
			col.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(player.GetComponent<PlayerS>().xForce, 0f, player.GetComponent<PlayerS>().zForce));
		}
	}

	void cancelTrigger()
	{
		player.GetComponent<BoxCollider> ().isTrigger = false;
	}
}
