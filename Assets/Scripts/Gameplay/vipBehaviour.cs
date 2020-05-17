using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vipBehaviour : MonoBehaviour {
	private const float RANGE=4f;
	private const float SMOOTH=0.3f;
	private Rigidbody rb;
	private float NORM_Y=0.4f,UPPER_Y=2f;
	private GameObject BlokeGroup;
	private GameObject[] ballList;
	public bool turn;

	void Start () {
		BlokeGroup = GameObject.Find ("BlokeGroup");
	//	rb = GetComponent<Rigidbody> ();
//		print (ballList);
	}
	

	void Update () {
		
		ballList = PowerUp.Instance.ballList;

		foreach (Transform bloke in BlokeGroup.transform) {
			if (bloke.gameObject.activeSelf) {
				Vector3 toMove = Vector3.zero;
				foreach (GameObject temp in ballList) {
					if (!temp.activeSelf || temp.GetComponent<BallS> ().turn == turn)
						continue;
					Rigidbody rb = temp.GetComponent<Rigidbody> ();
					if (Vector3.Distance (rb.position, bloke.position) < RANGE) {
						toMove = new Vector3 (bloke.position.x, UPPER_Y, bloke.position.z);
						break;
					} else {
						toMove = new Vector3 (bloke.position.x, NORM_Y, bloke.position.z);
					}

				
				}
				if (toMove != Vector3.zero) {
					bloke.transform.position = Vector3.MoveTowards (bloke.transform.position, toMove, SMOOTH);
				}
			}
		}
	}

}
