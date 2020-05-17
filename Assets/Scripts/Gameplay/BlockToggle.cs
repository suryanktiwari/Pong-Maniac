using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockToggle : MonoBehaviour {
	public int state = 1;
	public Material invisibleMat;
	private Material original;
	// Use this for initialization
	void Start () {
		original = this.GetComponent<Renderer> ().material;
	}
	void OnCollisionEnter(Collision col){
   //     print(gameObject.GetComponent<Block>().blockType + "yo hai");
		if (gameObject.GetComponent<Block> ().blockType != BlockTypes.Toggle)
			return;
		if (col.gameObject.CompareTag ("Ball") || col.gameObject.CompareTag ("Bullet")||col.gameObject.CompareTag("padGoli")) {
			gameObject.GetComponent<Collider>().isTrigger=true;
			state = 0;
			this.GetComponent<Renderer> ().material = invisibleMat;

		}

	}
	void OnTriggerEnter(Collider col){
		        
	}

	void OnTriggerExit(Collider col)
	{
		if (gameObject.GetComponent<Block> ().blockType != BlockTypes.Toggle)
			return;
		//print(gameObject.GetComponent<Block>().blockType + "yo hai");

		if (col.gameObject.CompareTag ("Bullet") || col.gameObject.CompareTag ("padGoli")) {
			gameObject.GetComponent<Collider> ().isTrigger = false;
			this.GetComponent<Renderer> ().material = original;
			state = 1;
		} else if (col.gameObject.CompareTag ("Ball")) {
			if (state == 1) {
				this.GetComponent<Renderer> ().material = invisibleMat;
				state = 0;
			} else {
				this.GetComponent<Renderer> ().material = original;
				state = 1;
			}
			if (!PowerUp.Instance.powerVar [(int)PowerTypes.FlareBall].isWorking) {
				gameObject.GetComponent<Collider> ().isTrigger = false;
				state = 1;
			}
		}
	}
}
