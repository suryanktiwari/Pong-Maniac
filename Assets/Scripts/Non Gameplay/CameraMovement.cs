using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	private float speed = 0.006f;
	private float RANDOM_RANGE =  0.5f;

	public Vector3[] posList;
	public GameObject lookAt;
	private Vector3 vel;

	public Vector3 targetPos;

	void Start () {
		targetPos=posList[Random.Range (0, posList.Length)];
		if (youdidthistoher.Instance.DrunkActive == 1) {
			increaseSpeed ();
		}
		InvokeRepeating ("changeCameraOffset", 0f, 10f);
	}
	
	void Update () {
		if (transform.position == targetPos) {
			targetPos=posList[Random.Range (0, posList.Length)];
		} else {
			transform.position = Vector3.MoveTowards (transform.position, targetPos, speed);
			transform.LookAt (lookAt.transform);
		}
	}

	void changeCameraOffset()
	{
		vel = GetComponent<CameraMovement>().targetPos;
		vel.x += Random.Range (-RANDOM_RANGE, RANDOM_RANGE);
		vel.y += Random.Range (-RANDOM_RANGE, RANDOM_RANGE);
		vel.z += Random.Range (-RANDOM_RANGE, RANDOM_RANGE);
		GetComponent<CameraMovement> ().targetPos = vel;
	}

	void increaseSpeed()
	{
		speed = 0.06f;
	}
}
