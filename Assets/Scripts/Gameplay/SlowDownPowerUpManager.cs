using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownPowerUpManager : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Ball") 
		{
			for (int i = 0; i < 3; i++) {

				PowerUp.Instance.ballList [i].GetComponent<BallS> ().currentVelocity = PowerUp.Instance.SPEEDSLOW;
				PowerUp.Instance.ballList [i].transform.GetChild (2).gameObject.SetActive (true);
				PowerUp.Instance.ballList [i].GetComponent<Renderer> ().material = PowerUp.Instance.materials [1];
			}
		}
	}

	void OnTriggerExit(Collider col)
	{
		for (int i = 0; i < 3; i++) {
			PowerUp.Instance.ballList [i].GetComponent<BallS> ().currentVelocity = PowerUp.Instance.SPEEDNORMAL;
			PowerUp.Instance.ballList [i].transform.GetChild (0).gameObject.SetActive (false);
			PowerUp.Instance.ballList [i].transform.GetChild (2).gameObject.SetActive (false);
			PowerUp.Instance.ballList [i].GetComponent<Renderer> ().material = PowerUp.Instance.materials [0];
		}
	}
}
