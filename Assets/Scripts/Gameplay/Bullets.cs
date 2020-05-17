using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour {

	private const float BULLET_FORWARD_FORCE=500f;
	private const float BULLET_TIME_INTERVAL = 0.4f;
	private const float BULLET_LIFE_DURATION = 3.0f;

	public GameObject bulletPrefab;
	public GameObject Bullet_Emmiter_Left;
	public GameObject Bullet_Emmiter_Right;
	public Collider ball;
//	public Collider gamePad;
	private GameObject Temporary_Bullet_Handler_Left;
	private GameObject Temporary_Bullet_Handler_Right;
	private Rigidbody Temporary_Rigidbody_Left;
	private Rigidbody Temporary_Rigidbody_Right;
	private Vector3 direction;
	public AudioSource a;
	public AudioClip b;
	void Start () {
	//	Physics.IgnoreCollision (bulletPrefab.GetComponent<Collider>(),GameObject.Find("Ball").GetComponent<Collider>());
		InvokeRepeating ("Shoot", BULLET_TIME_INTERVAL, BULLET_TIME_INTERVAL);
		if (transform.parent.CompareTag ("player")) {
			direction = Vector3.right;
		} else {
			direction = Vector3.left;
		}
	}
	
	void Shoot()
	{	if(this.gameObject.activeSelf){
		Temporary_Bullet_Handler_Left = Instantiate (bulletPrefab, Bullet_Emmiter_Left.transform.position, Bullet_Emmiter_Left.transform.rotation) as GameObject; 
		Temporary_Bullet_Handler_Right = Instantiate (bulletPrefab, Bullet_Emmiter_Right.transform.position, Bullet_Emmiter_Right.transform.rotation) as GameObject; 
			a.PlayOneShot (b, 0.2f);
			if (gameObject.tag.Equals ("player")) {
				Temporary_Bullet_Handler_Left.name = "player_goli";
				Temporary_Bullet_Handler_Right.name = "player_goli";
			} else if(gameObject.tag.Equals ("AI")) {
				Temporary_Bullet_Handler_Left.name = "AI_goli";
				Temporary_Bullet_Handler_Right.name = "AI_goli";
			}
				
		Temporary_Bullet_Handler_Left.GetComponent<Rigidbody> ().AddForce (direction * BULLET_FORWARD_FORCE);
		Temporary_Bullet_Handler_Right.GetComponent<Rigidbody> ().AddForce (direction * BULLET_FORWARD_FORCE);

		}
	}
		
}
