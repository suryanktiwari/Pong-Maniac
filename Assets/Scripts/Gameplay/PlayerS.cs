using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerS : MonoBehaviour {
	private static PlayerS instance;
	public static PlayerS Instance{get{ return instance;}}

	private float ORIGINAL_FORCE_MODIFIER = 0.4f;
	private float MAX_FORCE = 500.0f;

	private const float PADDLE_BACK_ANIM_DUR = 0.12f;
	private float MIN_X, MAX_X, MAX_Z, MIN_Z;
    
    private const float MAST_VALUE_PAD_NORMAL = 2.5f;
	private const float MAST_VALUE_PAD_SHORT=1.5f;
	private const float MAST_VALUE_PAD_LONG=3.5f;
   
	private float dirZ=0;
	public float zForce;
	public float xForce;
	public Vector3 oldPosition;

	private Rigidbody rb;
	public float maximumZ,minimum;

	public GameObject gun, magnet;

	void Start () {
		instance = this;
		magnet = transform.GetChild (0).gameObject;
		gun = transform.GetChild (1).gameObject;

		MIN_X =	-11.5f + transform.localScale.x/2;
		MAX_X = -5.5f - transform.localScale.x/2;
		MAX_Z=	8.5f-transform.localScale.z/2;						
		MIN_Z=	-8.5f+transform.localScale.z/2;

		
		if (gameObject.name == "player") {
			this.gameObject.GetComponent<Renderer> ().material = youdidthistoher.Instance.materials [youdidthistoher.Instance.currentSkinIndexPad];
			if (youdidthistoher.Instance.currentSkinIndexPad == 6) {
				//sides
				transform.GetChild(3).gameObject.SetActive(true);
				transform.GetChild (3).GetChild(0).GetComponent<MeshRenderer> ().material = youdidthistoher.Instance.materials [6];
				transform.GetChild (3).GetChild(1).GetComponent<MeshRenderer> ().material = youdidthistoher.Instance.materials [6];
			} else if (youdidthistoher.Instance.currentSkinIndexPad == 7) {
				//horns
				transform.GetChild(2).gameObject.SetActive(true);
			}
		}

		InvokeRepeating ("CalculateMaximumZ", 0.0f, 0.5f);
	}

	void OnCollisionEnter(Collision col)
	{   if (gameObject.transform.GetChild(0).gameObject.activeInHierarchy) return;
		if (!GameManager.Instance.gameStarted)
			GameManager.Instance.gameStarted = true;
		if (col.gameObject.CompareTag ("Ball")) {
			foreach (ContactPoint contact in col.contacts) {
                //setting ball velocity zero
				contact.otherCollider.GetComponent<Rigidbody> ().velocity = Vector3.zero;
                //contact point in z-direction
                dirZ = contact.point.z - transform.position.z;

                //updating maximumz for better play
                if (Mathf.Abs (dirZ) > maximumZ) {
					maximumZ = Mathf.Abs (dirZ);
				}
                //calculating angle
                float theta= dirZ / maximumZ;
				theta = theta * Mathf.PI / 2;

                xForce = MAX_FORCE * Mathf.Cos (theta);
				zForce = MAX_FORCE * Mathf.Sin (theta);

                //hitting backside force will be opposite in direction
                if (contact.point.x < transform.position.x) {
					xForce = -xForce;
				}
                //adding force to the ball
                contact.otherCollider.GetComponent<Rigidbody> ().AddForce (xForce, 0, zForce);
			}
            //forward moving force 
            if (oldPosition.x<this.transform.position.x){
				col.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(xForce,0,zForce)*ORIGINAL_FORCE_MODIFIER);
			}
			if (oldPosition != transform.position) {
				oldPosition = transform.position;
			}

		}
	}
		
	public void Move(Vector3 toMove){
        //Vector3 pos = GetComponent<Rigidbody> ().position + toMove;
        Vector3 pos = transform.position + toMove;
        pos = withinBoundary (pos);
        //GetComponent<Rigidbody> ().MovePosition (pos);
        transform.position = pos;
//        print(toMove.magnitude+" val");
	}
	void CalculateMaximumZ(){
		// maximumZ is value that is used in calculation for hitting ball in z direction
		if (transform.localScale.z > 2.5) {
			maximumZ = MAST_VALUE_PAD_LONG;
		} else if (transform.localScale.z < 2.5) {
			maximumZ = MAST_VALUE_PAD_SHORT;
		} else {
			maximumZ = MAST_VALUE_PAD_NORMAL;
		}
	}

	Vector3 withinBoundary(Vector3 pos)
    {
        Vector3 temp = pos;
		if (temp.x > MAX_X)
			GameManager.Instance.ground.transform.GetChild (1).GetComponent<Animation> ().Play ();	//Play the area limit animation
        temp.x = Mathf.Clamp(temp.x, MIN_X, MAX_X);                                             //Field Play Constraint
        temp.z = Mathf.Clamp(temp.z, MIN_Z, MAX_Z);                                             //Field Play Constraint
		return temp;
    }
}
