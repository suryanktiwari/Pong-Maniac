using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class coinBehaviour : MonoBehaviour {
	private const float ROTATION_SPEED = 4f;
	private bool turn;
	private GameObject ball;
	private float COIN_SPEED=4f;
	private float dirX=-1f;
	private bool goBack,goUp;

	private bool notCollided = true,collected;

	void Update () {
		if (notCollided) {
			transform.Rotate (new Vector3 (90, 0, 0) * Time.deltaTime * ROTATION_SPEED);
			transform.position = new Vector3 (transform.position.x + Time.deltaTime * dirX * COIN_SPEED, transform.position.y, transform.position.z);
		}
		if (collected) {
			if (goUp) {
				transform.position = Vector3.Slerp (transform.position, new Vector3 (transform.position.x, 2.2f, transform.position.z), 0.1f);
	
				if (transform.position.y>=2f) {
					goUp = false;
					goBack = true;

				}
			}
			if (goBack) {
				transform.position = Vector3.Slerp (transform.position, new Vector3 (transform.position.x, 0, transform.position.z), 0.1f);
				if (transform.position.y <= 0.4f) {
					goBack = false;
					Destroy (this.transform.parent.gameObject);
					Destroy (this.gameObject);
				}
			}
		}
	}
	void OnTriggerEnter(Collider col){
		if (col.gameObject.CompareTag ("player")) {
			if (goBack||goUp) {
				return;
			}
			GameManager.Instance.coinCount++;
			notCollided = false;
			collected=true;
			goUp = true;

			GameObject temp = new GameObject ();
			temp.name="CoinContainer";
			temp.transform.SetParent (col.transform);
			temp.transform.localScale = new Vector3 (1/col.transform.localScale.x, 1/col.transform.localScale.y, 1/col.transform.localScale.z);
			this.transform.SetParent (temp.transform);

		}
		else if (col.gameObject.name.Contains ("Wall"))
			Destroy (this.gameObject);
	}


}
