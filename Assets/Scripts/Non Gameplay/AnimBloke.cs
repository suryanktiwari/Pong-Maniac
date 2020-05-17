using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimBloke : MonoBehaviour {
	private const float SMOOTHNESS =0.04f;
	void Update () {
		transform.localScale = Vector3.MoveTowards (transform.localScale, Vector3.zero,SMOOTHNESS);
		gameObject.GetComponent<Collider> ().isTrigger = true;
		if (transform.localScale == Vector3.zero) {
			Destroy (this.gameObject);
		}
	}
}
