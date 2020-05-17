using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class padUdaDe : MonoBehaviour {

	private const float CPU_CONFUSION_FORCE = 300.0f;
	private GameObject ground;
	private const float FORCE_MULTIPLIER = 300.0f;
	private GameManager GMScript;
	void Start(){
		ground = GameObject.Find ("Ground");
		GMScript = ground.GetComponent<GameManager> ();
		Invoke ("destroy", 4.0f);
	}

	void Update()
	{
		if (transform.position.y < 0)
			Destroy (this.gameObject);
		if (transform.position.y > 5)
		{
			this.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			this.gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (Random.Range (0, 1), -Random.Range (1, 5), Random.Range (0, 1)) * FORCE_MULTIPLIER);
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.CompareTag ("Block")) {   
			col.gameObject.GetComponent<Block> ().HitBlock (true);
			col.gameObject.GetComponent<Block> ().ResetBlock (true);
			if(!col.gameObject.GetComponent<BlockToggle>().isActiveAndEnabled)
			{
				GameManager.Instance.player_BlokePoint++;
				Destroy (gameObject);
			}
		} else if (col.gameObject.CompareTag ("AI")) {
			col.gameObject.GetComponent<Rigidbody> ().AddForce (Vector3.left*10f);
			Destroy (this.gameObject);
		}
	}
		void destroy()
		{
			Destroy (this.gameObject);
		}
	}
