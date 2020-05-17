using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIS : MonoBehaviour {
	private static AIS instance;
	public static AIS Instance{get{ return instance;}}

	private float SMOOTH_MOVEMENT=0.26f;
	private float ORIGINAL_FORCE_MODIFIER = 0.3f;
	private float MAX_FORCE = 500.0f;

	private const float POWERUP_CATCH_LINE= 6f;
	private const float SLOW_POWERUP_MODE_MIN_CATCH_LINE = 7f;
	private const float PAD_THICKNESS=0.5f;
	private const float PADDLE_BACK_FORCE = 500.0f;
	private const float PADDLE_BACK_ANIM_DUR = 0.12f;

	private const int NEGATIVE_CORRECT = -1;

	private Vector3 REST_POS = new Vector3 (8, 0.4f, 0);

	private Vector3 temp;
	private float maximumZ;
	public GameObject ball;

	private float dirZ = 0f;
	//private float kahan_dhakka_mara;
	private float zForce;
	private float xForce;
	private bool back_motion_mode = false;

    private const float MAST_VALUE_PAD_NORMAL = 2.5f;
	private const float MAST_VALUE_PAD_SHORT=1.5f;
	private const float MAST_VALUE_PAD_LONG=3.5f;
    
	private Rigidbody rb;
	private Rigidbody bRb;
	private Vector3 oldPostion;
	public GameObject gun, magnet;

	private float MIN_X, MAX_X, MAX_Z, MIN_Z;

	private bool vella,ballAtBack;
	private GameObject ballPiche;
	void Awake(){
		rb = GetComponent<Rigidbody> ();

		rb.collisionDetectionMode = CollisionDetectionMode.Continuous;						// heavy physics but ball doesnt go through objects at high SPEED anymore
																// temporary solution is to reduce the max force applied on ball
	}
	void Start(){
		instance = this;
		MIN_X =	5.5f + transform.localScale.x/2;
		MAX_X = 11.5f - transform.localScale.x/2;
		MAX_Z=	8.5f-transform.localScale.z/2;						
		MIN_Z=	-8.5f+transform.localScale.z/2;		
		magnet = transform.GetChild (0).gameObject;
		gun = transform.GetChild (1).gameObject;

		int AIMat = Random.Range(0,8);
		this.gameObject.GetComponent<Renderer>().material=youdidthistoher.Instance.materials[AIMat];
		if (AIMat == 6) {
			//sides
			transform.GetChild(3).gameObject.SetActive(true);
			transform.GetChild (3).GetChild(0).GetComponent<MeshRenderer> ().material = youdidthistoher.Instance.materials [6];
			transform.GetChild (3).GetChild(1).GetComponent<MeshRenderer> ().material = youdidthistoher.Instance.materials [6];
		} else if (AIMat == 7) {
			//horns
			transform.GetChild(2).gameObject.SetActive(true);
		}
	}


	void Update () {
        oldPostion = transform.position;
		vella = false;
		ballAtBack = false;

		// for selecting which ball to follow 
		if (!back_motion_mode) {
			
			GameObject[] ballList = GameObject.FindGameObjectsWithTag ("Ball"); 
			float minApproach = Mathf.Infinity;
			foreach (var ball in ballList) {
				float tempAngle = Vector3.Angle (ball.GetComponent<Rigidbody> ().velocity, Vector3.right);
				float tempDistance = Vector3.Distance (ball.transform.position, transform.position);
				tempAngle = tempAngle > 90f ? 90f : tempAngle;
				//print (tempAngle);
				//print (Mathf.Cos(Mathf.Deg2Rad*tempAngle)+"  COs");
				if (ball.transform.position.x > transform.position.x - 0.25f) {
					ballAtBack = true;
					ballPiche = ball;
				}
				if (ball.transform.position.x < transform.position.x - PAD_THICKNESS / 2 &&
				    (ball.GetComponent<Rigidbody> ().velocity.x > 0 || ball.transform.position.x > MIN_X)) {
					float approach = tempDistance * Mathf.Cos (Mathf.Deg2Rad * tempAngle) * ball.GetComponent<Rigidbody> ().velocity.magnitude;
					if (approach < minApproach) {
						this.ball = ball;
						minApproach = approach;
					}
				}
			}
			if (minApproach == Mathf.Infinity) {
//			print ("vella");;
				vella = true;
			}

            if (magnet.activeInHierarchy && magnet.transform.childCount > 0)
            {
                float minDistance = Mathf.Infinity, tempDistance;
                Transform targetBlock = null;
                foreach (Transform temp in GameManager.Instance.BlokeGroup)
                {
                    if (temp.GetComponent<Block>().blockType != BlockTypes.Rock && temp.GetComponent<Block>().blockType != BlockTypes.Blink && temp.GetComponent<Block>().blockType != BlockTypes.Toggle)
                    {
                        tempDistance = Vector3.Distance(temp.transform.position, transform.position);
                        if (tempDistance < minDistance)
                        {
                            minDistance = tempDistance;
                            targetBlock = temp;
                        }
                    }
                }
                if (minDistance != Mathf.Infinity)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetBlock.position, SMOOTH_MOVEMENT); //toward block
                }
            }
            else
            {

                if (!vella)
                {
                    Vector3 ballPos = ball.transform.position;
                    if (PowerUp.Instance.powerVar[(int)PowerTypes.AISlowBall].isWorking && ball.transform.position.x < transform.position.x)
                        ballPos.x = 7f;

                    transform.position = Vector3.MoveTowards(transform.position, ballPos, SMOOTH_MOVEMENT);
                }
                else
                {
                    if (ballAtBack)
                    {
                        Vector3 temp = transform.position;
                        if (temp.x + PAD_THICKNESS > MAX_X)
                        {
                            temp.x = ball.transform.position.x - PAD_THICKNESS;
                        }
                        else
                        {
                            temp.x = ball.transform.position.x + 2f;
                        }
                        if (ballPiche.transform.position.z > 0)
                        {
                            temp.z = temp.z - 1f;
                            transform.position = Vector3.MoveTowards(transform.position, temp, SMOOTH_MOVEMENT);
                        }
                        else
                        {
                            temp.z = temp.z + 1f;
                            transform.position = Vector3.MoveTowards(transform.position, temp, SMOOTH_MOVEMENT);
                        }
                    }
                    else
                    {
                        GameObject[] powerUps = GameObject.FindGameObjectsWithTag("POWERUPS");   //towards powerUPs
                        if (powerUps.Length > 0)
                        {
                            float minDistance = Mathf.Infinity;
                            GameObject PUpakdo = null;
                            foreach (var PU in powerUps)
                            {
                                float tempDistance = Vector3.Distance(PU.transform.position, transform.position);
                                if (PU.transform.position.x > POWERUP_CATCH_LINE)
                                {
                                    float distance = tempDistance;
                                    if (distance < minDistance)
                                    {
                                        PUpakdo = PU;
                                        minDistance = distance;
                                    }
                                }
                            }
                            if (minDistance != Mathf.Infinity)
                            {
                                transform.position = Vector3.MoveTowards(transform.position, PUpakdo.transform.position, SMOOTH_MOVEMENT); //toward powerUp
                            }
                            else
                            {
                                transform.position = Vector3.MoveTowards(transform.position, REST_POS, SMOOTH_MOVEMENT); //towards resting
                            }
                        }

                    }
                }
            }
		}
			
		temp = transform.position;
		temp.x = Mathf.Clamp (temp.x, MIN_X, MAX_X);												//Field Play Constraint
		temp.z = Mathf.Clamp (temp.z, MIN_Z, MAX_Z);												//Field Play Constraint
		transform.position = temp;

		//CHECKING IF PAD GOT OUT OF PLAY ZONE DUE TO EXTERNAL FACTORS


		if (transform.position.x > MAX_X || transform.position.x < MIN_X || transform.position.z > MAX_Z || transform.position.z < MIN_Z) {
			temp.x = (MAX_X + MIN_X) / 2;
			temp.z = (MAX_Z + MIN_Z) / 2;
			transform.position = Vector3.MoveTowards (transform.position, temp, SMOOTH_MOVEMENT * 4);
		}
		bRb = ball.GetComponent<Rigidbody> ();
		if (bRb.velocity.magnitude != 0) {
			//kahan_dhakka_mara = bRb.velocity.normalized.x;
		}
		
	}


	void OnCollisionEnter(Collision col)
	{	CalculateMaximumZ ();
		

		if (col.gameObject.CompareTag ("Ball")) {
			

			foreach (ContactPoint contact in col.contacts) {
				contact.otherCollider.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				dirZ = contact.point.z - transform.position.z;
				if (Mathf.Abs (dirZ) > maximumZ) {
					maximumZ = Mathf.Abs (dirZ);
				}
				dirZ = dirZ / maximumZ;
				dirZ = dirZ * Mathf.PI / 2;
				xForce = MAX_FORCE * Mathf.Cos (dirZ);
				zForce = MAX_FORCE * Mathf.Sin (dirZ);
				if (contact.point.x < transform.position.x)   //change for another AI
					xForce = -xForce;

				contact.otherCollider.GetComponent<Rigidbody> ().AddForce (xForce, 0, zForce);

			//	Invoke ("padBack", PADDLE_BACK_ANIM_DUR);
			}
			//kheech ke maar
			if (oldPostion.x < this.transform.position.x) {
				//	print ("chal jaa bhai");
				col.gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (xForce, 0, zForce) * ORIGINAL_FORCE_MODIFIER);

			}
			//PadBackAnim ();
			Invoke ("reset", PADDLE_BACK_ANIM_DUR);
		}
	}


	void CalculateMaximumZ(){
		// maximumZ is value that is used in calculation for hitting ball in z direction
		if (transform.localScale.z > 2.5) {
			maximumZ = MAST_VALUE_PAD_LONG;
		} else if (transform.localScale.z < 2.5) {
			maximumZ=MAST_VALUE_PAD_SHORT;
		} else {
			maximumZ = MAST_VALUE_PAD_NORMAL;
		}
	}


	void reset()
	{
		rb.velocity = Vector3.zero;
		back_motion_mode = false;
	}

	public void pushBack()
	{
		Vector3 tempPos = transform.position;
		tempPos.x += 0.5f;
		transform.position = tempPos;
	}
}
