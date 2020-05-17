using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BallS : MonoBehaviour {
	private const float NORMAL_BALL_SIZE = 1f;

	public float currentVelocity = 13.5f;

	public Rigidbody ballRig;
	private Vector3 direction;

	public bool turn;  //bool
	void Start()
	{	ballRig = GetComponent<Rigidbody> ();
		turn = true;
		Invoke ("startTheGameMan", 1f);
	}

	public void startTheGameMan()
	{
		GetComponent<Rigidbody> ().AddForce (Vector3.left * 5f);
	}

	void FixedUpdate(){
		if (ballRig.velocity.magnitude != currentVelocity && transform.parent.name =="Balls") {
			ballRig.velocity = ballRig.velocity.normalized * currentVelocity;
		}
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.CompareTag ("Block")) {
			col.GetComponent<Block> ().HitBlock (turn);
			col.GetComponent<Block> ().ResetBlock (turn);
		}		
	}


	void OnCollisionEnter(Collision col){
		if ((col.gameObject.name == "EastWall")) {			//wall behind AI
			GameManager.Instance.AI_WallPoint++;
			GameManager.Instance.a.PlayOneShot (GameManager.Instance.a3, 0.5f);
		} else if (col.gameObject.name == "WestWall") {
			//wall behind player
			GameManager.Instance.player_WallPoint++;
			GameManager.Instance.a.PlayOneShot (GameManager.Instance.a3, 0.5f);
		} else if (col.gameObject.name == "NorthWall") {
		
		} else if (col.gameObject.name == "SouthWall") {
		
		}  else if (col.gameObject.CompareTag ("player")) {
            turn = true;
			GameManager.Instance.a.PlayOneShot (GameManager.Instance.a4, 0.6f);

		} else if (col.gameObject.CompareTag ("AI")) {
            turn = false;
			GameManager.Instance.a.PlayOneShot (GameManager.Instance.a4, 0.6f); 
		
		} else if (col.gameObject.CompareTag ("Block")) {
			col.gameObject.GetComponent<Block> ().HitBlock (turn);
			col.gameObject.GetComponent<Block> ().ResetBlock (turn);
			if (this.transform.parent.localScale.z != NORMAL_BALL_SIZE) {

               GameManager.Instance.Blast(col.gameObject, turn);
                //removeADJBloke (col.gameObject);
			}
		}
	}
    /*
	void removeADJBloke(GameObject obj){
		Transform[] childObjects = GameManager.Instance.BlokeGroup.GetComponentsInChildren<Transform> ();
		Vector3 pos = obj.transform.position;
		foreach (Transform temp in childObjects) {
			if (temp == GameManager.Instance.BlokeGroup)
				continue;

			for (int i = (int)pos.x - 1; i <= (int)pos.x + 1; i++) {
				for (int j = (int)pos.z - 1; j <= (int)pos.z + 1; j++) {
					if (i == (int)pos.x && j == (int)pos.z) {
						continue;			
					}
					if (temp.position.x != i && temp.position.z != j) {
						temp.gameObject.GetComponent<Block> ().HitBlock (turn);
						temp.gameObject.GetComponent<Block> ().ResetBlock (turn);
					}
				}
			}
		}
	}*/
}