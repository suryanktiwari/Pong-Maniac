using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadS : MonoBehaviour {

	private const float PAD_FORWARD_FORCE=1600f;
	private const float PAD_TIME_INTERVAL = 0.4f;
	private const float PAD_LIFE_DURATION = 3.0f;
	private Vector3 MARGIN = new Vector3(1.5f,0,0);

	public GameObject PadPrefabLeft;
	public GameObject PadPrefabRight;
	public GameObject Pad_Emmiter_Left;
	public GameObject Pad_Emmiter_Right;

	private GameObject Temporary_Pad_Handler_Left;
	private GameObject Temporary_Pad_Handler_Right;
	private Rigidbody Temporary_Rigidbody_Left;
	private Rigidbody Temporary_Rigidbody_Right;
	private Vector3 direction;


	void Awake () {
		//Physics.IgnoreCollision (bulletPrefab.GetComponent<Collider>(),gamePad);
		//Time.fixedDeltaTime = BULLET_TIME_INTERVAL;
		InvokeRepeating ("Shoot", PAD_TIME_INTERVAL, PAD_TIME_INTERVAL);
		direction = Vector3.right;
		//PadPrefab.transform.Rotate (new Vector3(0,90,0));
	}

	void Shoot()
	{	if(this.gameObject.activeSelf){
			Temporary_Pad_Handler_Left = Instantiate (PadPrefabLeft, Pad_Emmiter_Left.transform.position + MARGIN, Quaternion.Euler(0,90,0)) as GameObject; 
			Temporary_Pad_Handler_Right = Instantiate (PadPrefabRight, Pad_Emmiter_Right.transform.position + MARGIN, Quaternion.Euler(0,90,0)) as GameObject; 
		//	Temporary_Pad_Handler_Left.transform.Rotate (0,0,90);
		//	Temporary_Pad_Handler_Right.transform.Rotate (0,0,90);

			Temporary_Rigidbody_Left = Temporary_Pad_Handler_Left.GetComponent<Rigidbody> ();
			Temporary_Rigidbody_Right = Temporary_Pad_Handler_Right.GetComponent<Rigidbody> ();
		
			Temporary_Rigidbody_Left.AddForce (direction * PAD_FORWARD_FORCE);
			Temporary_Rigidbody_Right.AddForce (direction * PAD_FORWARD_FORCE);
			//	yield return new WaitForSeconds (Time.time+BULLET_TIME_INTERVAL);
			//	Destroy(Temporary_Bullet_Handler_Left,BULLET_LIFE_DURATION);
			//	Destroy(Temporary_Bullet_Handler_Right,BULLET_LIFE_DURATION);
		}
	}

}
