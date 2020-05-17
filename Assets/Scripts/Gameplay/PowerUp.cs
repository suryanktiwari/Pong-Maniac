	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class PowerUp : MonoBehaviour {
		private static PowerUp instance;
		public static PowerUp Instance{get{return instance; }}

		private const float NORMAL_BALL_SIZE = 1;
		private const float BIG_BALL_SIZE = 1.33f;
		private const float BALL_HEIGHT_FROM_GROUND=0.375f;

    private const float NORMAL_PAD_SIZE = 1.25f;
    private const float BIG_PAD_SIZE = 1.75f;
    private const float SMALL_PAD_SIZE = 0.75f;
    /*
    private const float NORMAL_PAD_SIZE = 2.5f;
		private const float BIG_PAD_SIZE = 3.5f;
		private const float SMALL_PAD_SIZE = 1.5f;
	*/	private const float PAD_X = 0.3f;
		private const float PAD_Y = 0.25f;

		private const float BLOKE_MIN_SPAWNX = -3f;
		private const float BLOKE_MAX_SPAWNX = 3f;

		public float SPEEDNORMAL=13.5f;
		private float SPEEDFAST=17f;
		public float SPEEDSLOW=7f;

		private const float POWER_5_DURATION =5f;				//gun
		private const float POWER_7_DURATION = 7f;			//multi ball,flare ball, vip ball;
		private const float POWER_10_DURATION = 10f;			//pad,bigball,speed ball,magnet
		private const float PURCHASED_DURATION = 24f;

		private const float NORM_Y=0.4f;
		private const float SMOOTH=0.2f;
		private GameObject ball,player,AI;

		//true - player || false - AI
		public Transform BlokeGroup;
		public Transform playerTransform, AITransform; 
		public GameObject ballTransform;
		public GameObject playerSlowField, AISlowField;

		public static bool padLongPurchased,flareBallPurchased,VIPBallPurchased,magnetPadPurchased,gunPadPurchased,bigBallPurchased,multiBallPurchased;
		///for current reference to the active pads
		private Magnet MagnetScript;
		private bool BlockGiraDe;

		public PowerVariables[] powerVar = new PowerVariables[15];



		//public Transform currentPlayerPad, currentAIPad;
		public GameObject[] ballList = new GameObject[3];

		public bool animPlayerPad,animAIPad,animBall,animMultiBall;
		public Vector3 playerPadScale,AIPadScale,BallScale;
		public Material[] materials;
		public GameObject drunkPad;

		// Use this for initialization

		void Start () {
		//	padLongPurchased = true;
			instance = this;
			BlokeGroup = GameObject.Find ("BlokeGroup").transform;
			materials = Resources.LoadAll<Material> ("SpecialMaterials");

	      	ballTransform.transform.position = new Vector3 (-7,0.5f,0);
			ballTransform.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			PUSet ();
		}

		// Update is called once per frame
		void Update () {
			khareedLiya ();
			PowerUpWorking ();
			BlokeGirao ();
		//	magnetRelease ();
			BallKoMaterialDo ();
		}

	    void BallKoMaterialDo(){
			if ((powerVar[(int)PowerTypes.VipBall].isWorking 
				||powerVar[(int)PowerTypes.PlayerSlowBall].isWorking
				||powerVar[(int)PowerTypes.AISlowBall].isWorking
				||powerVar[(int)PowerTypes.FastBall].isWorking
				||powerVar[(int)PowerTypes.FlareBall].isWorking
				) 
				&& (ballList [0].GetComponent<Renderer> ().material == materials [0])) {
				if (powerVar[(int)PowerTypes.VipBall].isWorking) {
					for (int i = 0; i < 3; i++) {
						ballList [i].GetComponent<Renderer> ().material = materials [3];
					}
				}  
				else if (powerVar[(int)PowerTypes.FlareBall].isWorking)
	            {
	                for (int i = 0; i < 3; i++)
	                {
	                    ballList[i].transform.GetChild(1).gameObject.SetActive(true);
	                }
	            }
				else {
					if (powerVar[(int)PowerTypes.FastBall].isWorking) {
						for (int i = 0; i < 3; i++) {
							ballList [i].GetComponent<Renderer> ().material = materials [2];
						}
					}  else {
						for (int i = 0; i < 3; i++) {
							ballList [i].GetComponent<Renderer> ().material = materials [1];
						}
					}

				}
			}
		}
	   
		void BlokeGirao(){
			if (BlockGiraDe) {
				bool kardo=true;
				for(int i=0;i<3;i++) {
					if (ballList[i].activeSelf && BLOKE_MIN_SPAWNX-0.5f < ballList[i].transform.position.x && ballList[i].transform.position.x< BLOKE_MAX_SPAWNX+0.5f )
						kardo = false;
				}
				if (kardo) {
					foreach (Transform t in BlokeGroup.transform) {
						if (t.gameObject.activeSelf) {
							while (t.position.y != NORM_Y) {
								Vector3 toMove = new Vector3 (t.position.x, NORM_Y, t.position.z);
								t.transform.position = Vector3.MoveTowards (t.transform.position, toMove, SMOOTH);
							}
						}
					}
				}
			}
		}
		void khareedLiya(){
			if(padLongPurchased){
				PowerPurchaseSet(PowerTypes.PlayerLong);
				padLongPurchased = false;
			}

			if(flareBallPurchased){
				PowerPurchaseSet(PowerTypes.FlareBall);
				flareBallPurchased = false;
			}

			if(VIPBallPurchased){
				PowerPurchaseSet(PowerTypes.VipBall);
				VIPBallPurchased = false;
			}

			if(bigBallPurchased){
				PowerPurchaseSet(PowerTypes.BigBall);
				bigBallPurchased = false;
			}

			if(multiBallPurchased){
				PowerPurchaseSet(PowerTypes.MultiBall);
				multiBallPurchased = false;
			}

			if(gunPadPurchased){
				PowerPurchaseSet(PowerTypes.PlayerGun);
				gunPadPurchased = false;
			}

			if(magnetPadPurchased){
				PowerPurchaseSet(PowerTypes.PlayerMagnet);
				magnetPadPurchased = false;

			}

		}
		void PowerPurchaseSet(PowerTypes powerType){
			PU (powerType);
			powerVar [(int)powerType].finishTime += PURCHASED_DURATION;
			powerVar [(int)powerType].finishTime -=  powerVar [(int)powerType].durationApplied;
		}

		// power up adre constantly checked if they are true or not
		// work bool 
		void PowerUpWorking(){
			for (int i = 0; i < 14; i++) {
				
	//			if (powerVar[i].isWorking) {
	//				print (Time.time + "   " + powerVar[i].finishTime);
	//			}
				if (powerVar[i].isWorking && Time.time>powerVar[i].finishTime) {
//					print ("power is reset " + powerVar[i]);
					powerVar[i].isWorking = false;
					powerVar[i].finishTime = 0f;
					PowerTypes powerType = (PowerTypes)i;
					PUDeactivate (powerType);
				}
			}
		}

		public void PU(PowerTypes powerType,bool turn = true){
			int i = (int)powerType;
			if (!powerVar [i].isWorking) {
				powerVar [i].finishTime = Time.time;
			}
			powerVar [i].finishTime += powerVar [i].durationApplied;
		
//			print (powerType + "isWorking");
//			print (powerVar [i].finishTime + "fin" + Time.time);
//			print ("dur " + powerVar [i].durationApplied);
			ClashingPower (powerType);
			PUActivate (powerType,turn);
			powerVar [i].isWorking = true;
		}

		public void ClashingPower(PowerTypes powerType){
			switch (powerType) {
			case PowerTypes.AILong:
				WorkingThenDeactivate (PowerTypes.AIShort);
				break;
			case PowerTypes.AIShort:
				WorkingThenDeactivate (PowerTypes.AILong);
				break;
			case PowerTypes.AIGun:
				WorkingThenDeactivate (PowerTypes.AIMagnet);
				break;
			case PowerTypes.AIMagnet:
				WorkingThenDeactivate (PowerTypes.AIGun);
				break;
			case PowerTypes.PlayerLong:
				WorkingThenDeactivate (PowerTypes.PlayerShort);
				break;
			case PowerTypes.PlayerShort:
				WorkingThenDeactivate (PowerTypes.PlayerLong);
				break;
			case PowerTypes.PlayerGun:
				WorkingThenDeactivate (PowerTypes.PlayerMagnet);
				break;
			case PowerTypes.PlayerMagnet:
				WorkingThenDeactivate (PowerTypes.PlayerGun);
				break;
			case PowerTypes.FlareBall:
				WorkingThenDeactivate (PowerTypes.VipBall);
				break;
			case PowerTypes.VipBall:
				WorkingThenDeactivate (PowerTypes.FlareBall);
				break;
			case PowerTypes.PlayerSlowBall:
				WorkingThenDeactivate (PowerTypes.FastBall);
				break;
			case PowerTypes.AISlowBall:
				WorkingThenDeactivate (PowerTypes.FastBall);
				break;
			case PowerTypes.FastBall:
				WorkingThenDeactivate (PowerTypes.AISlowBall);
				WorkingThenDeactivate (PowerTypes.PlayerSlowBall);
				break;
			}
		}
		public void WorkingThenDeactivate(PowerTypes Counter){
			if (powerVar [(int)Counter].isWorking) {
				PUDeactivate (Counter);
			}
		}
		public void PUDeactivate(PowerTypes powerType){
			switch (powerType) {
			case PowerTypes.AILong:
				AIPadScale= new Vector3 (PAD_X, PAD_Y, NORMAL_PAD_SIZE);
				animAIPad = true;

				break;
			case PowerTypes.AIShort:
				AIPadScale= new Vector3 (PAD_X, PAD_Y, NORMAL_PAD_SIZE);
				animAIPad = true;

				break;
			case PowerTypes.AIGun:
				AIS.Instance.gun.SetActive (false);
				break;
			case PowerTypes.AIMagnet:
				AIS.Instance.magnet.GetComponent<Magnet> ().release ();
				AIS.Instance.magnet.SetActive (false);

				break;
			case PowerTypes.PlayerLong:
				playerPadScale= new Vector3 (PAD_X, PAD_Y, NORMAL_PAD_SIZE);
				animPlayerPad = true;

				break;
			case PowerTypes.PlayerShort:
				playerPadScale= new Vector3 (PAD_X, PAD_Y, NORMAL_PAD_SIZE);
				animPlayerPad = true;
				break;

			case PowerTypes.PlayerGun:
				PlayerS.Instance.gun.SetActive (false);
				break;

			case PowerTypes.PlayerMagnet:

				PlayerS.Instance.magnet.GetComponent<Magnet> ().release ();
				PlayerS.Instance.magnet.SetActive (false);
				break;

			case PowerTypes.BigBall:
				BallScale=new Vector3 (NORMAL_BALL_SIZE, NORMAL_BALL_SIZE, NORMAL_BALL_SIZE);
				animBall = true;

				break;
			case PowerTypes.MultiBall:
				animMultiBall = true;

				break;
			case PowerTypes.FlareBall:
				foreach (Transform child in BlokeGroup.transform)
				{	
					if(child.GetComponent<BlockToggle>().isActiveAndEnabled&&child.GetComponent<BlockToggle>().state==0)
						child.GetComponent<Collider> ().isTrigger = true;
					else
					{
						child.GetComponent<Collider> ().isTrigger = false;
					}	
				}
				
				for (int i = 0; i < 3; i++) {
					ballList [i].transform.GetChild (1).gameObject.SetActive (false);
				}
				break;
			case PowerTypes.VipBall:
				BlockGiraDe = true;
				GameObject.Find("Ground").GetComponent<vipBehaviour>().enabled=false;
				for (int i = 0; i < 3; i++) {
					ballList [i].transform.GetChild (4).gameObject.SetActive (false);
					ballList [i].transform.GetChild (5).gameObject.SetActive (false);
					ballList [i].GetComponent<Renderer> ().material = materials [0];
				}
				break;
			case PowerTypes.PlayerSlowBall:
			
				playerSlowField.SetActive (false);

				for (int i = 0; i < 3; i++) {
					ballList [i].GetComponent<BallS> ().currentVelocity = SPEEDNORMAL;
					ballList [i].transform.GetChild (0).gameObject.SetActive (false);
					ballList [i].transform.GetChild (2).gameObject.SetActive (false);
					ballList [i].GetComponent<Renderer> ().material = materials [0];
				}
				break;
			case PowerTypes.AISlowBall:
				AISlowField.SetActive (false);

				for (int i = 0; i < 3; i++) {
					ballList [i].GetComponent<BallS> ().currentVelocity = SPEEDNORMAL;
					ballList [i].transform.GetChild (0).gameObject.SetActive (false);
					ballList [i].transform.GetChild (2).gameObject.SetActive (false);
					ballList [i].GetComponent<Renderer> ().material = materials [0];
				}
				break;
			case PowerTypes.FastBall:
				for (int i = 0; i < 3; i++) {
					ballList [i].GetComponent<BallS> ().currentVelocity = SPEEDNORMAL;	
					ballList [i].transform.GetChild (0).gameObject.SetActive (false);
					ballList [i].transform.GetChild (2).gameObject.SetActive (false);
					ballList [i].GetComponent<Renderer> ().material = materials [0];
				}
				break;
			}
		}

		public void PUActivate(PowerTypes powerType,bool turn = true){

			switch (powerType) {
			case PowerTypes.AILong:
				AIPadScale = new Vector3 (PAD_X, PAD_Y, BIG_PAD_SIZE);
				animAIPad = true;

				break;
			case PowerTypes.AIShort:
				AIPadScale= new Vector3 (PAD_X, PAD_Y, SMALL_PAD_SIZE);
				animAIPad = true;

				break;
			case PowerTypes.AIGun:
				AIS.Instance.gun.SetActive (true);
				break;
			case PowerTypes.AIMagnet:
				AIS.Instance.magnet.SetActive (true);
				break;
			case PowerTypes.PlayerLong:
				playerPadScale= new Vector3 (PAD_X, PAD_Y, BIG_PAD_SIZE);
				animPlayerPad = true;
				break;
			case PowerTypes.PlayerShort:
				playerPadScale= new Vector3 (PAD_X, PAD_Y, SMALL_PAD_SIZE);
				animPlayerPad = true;
				break;

			case PowerTypes.PlayerGun:
				PlayerS.Instance.gun.SetActive (true);
				break;
			case PowerTypes.PlayerMagnet:
				PlayerS.Instance.magnet.SetActive (true);
				break;
			case PowerTypes.BigBall:
				BallScale=new Vector3 (BIG_BALL_SIZE, BIG_BALL_SIZE, BIG_BALL_SIZE);
				animBall = true;
				break;
			case PowerTypes.MultiBall:
				ballList [1].gameObject.SetActive (true);
				ballList [2].gameObject.SetActive (true);

				if (!powerVar [(int)PowerTypes.MultiBall].isWorking) {
					ballList [1].transform.localScale = ballList [0].transform.localScale;
					ballList [2].transform.localScale = ballList [0].transform.localScale;

					ballList [1].transform.position = ballList [0].transform.position;
					ballList [2].transform.position = ballList [0].transform.position;

					ballList [1].GetComponent<Rigidbody> ().velocity = ballList [0].GetComponent<Rigidbody> ().velocity;
					ballList [2].GetComponent<Rigidbody> ().velocity = ballList [0].GetComponent<Rigidbody> ().velocity;
				}
				break;

			case PowerTypes.FlareBall:
				for (int i = 0; i < 3; i++) {
					ballList [i].transform.GetChild (1).gameObject.SetActive (true);
				}
                foreach (Transform child in BlokeGroup.transform)
                {
                    child.GetComponent<Collider>().isTrigger = true;
                }
                break;

			case PowerTypes.VipBall:
				GameManager.Instance.ground.GetComponent<vipBehaviour> ().enabled = true;
				GameManager.Instance.ground.GetComponent<vipBehaviour> ().turn = turn;

				for (int i = 0; i < 3; i++) {
					ballList [i].transform.GetChild (4).gameObject.SetActive (true);
					ballList [i].transform.GetChild (5).gameObject.SetActive (true);

					ballList [i].GetComponent<Renderer> ().material = materials [3];
				}
				break;
			case PowerTypes.PlayerSlowBall:
				playerSlowField.SetActive (true);
		/*		for (int i = 0; i < 3; i++) {

					ballList [i].GetComponent<BallS> ().currentVelocity = SPEEDSLOW;
					ballList [i].transform.GetChild (2).gameObject.SetActive (true);
					ballList [i].GetComponent<Renderer> ().material = materials [1];
				}
		*/
				break;
			case PowerTypes.AISlowBall:
					AISlowField.SetActive (true);
				break;
			case PowerTypes.FastBall:
				for (int i = 0; i < 3; i++) {
					ballList [i].GetComponent<BallS> ().currentVelocity = SPEEDFAST;
					ballList [i].transform.GetChild (0).gameObject.SetActive (true);
					ballList [i].GetComponent<Renderer> ().material = materials [2];
				}
				
				break;
			}
		}
		public void PUSet(){
			for (int i = 0; i < 14; i++) {
				switch (i) {
				case (int)PowerTypes.AILong:
					powerVar[i].durationApplied = POWER_10_DURATION;
					break;
				case (int)PowerTypes.AIShort:
					powerVar[i].durationApplied = POWER_10_DURATION;
					break;
				case (int)PowerTypes.AIGun:
					powerVar[i].durationApplied = POWER_5_DURATION;
					break;
				case (int)PowerTypes.AIMagnet:
					powerVar[i].durationApplied = POWER_10_DURATION;
					break;
				case (int)PowerTypes.PlayerLong:
					powerVar[i].durationApplied = POWER_10_DURATION;
					break;
				case (int)PowerTypes.PlayerShort:
					powerVar[i].durationApplied = POWER_10_DURATION;
					break;
				case (int)PowerTypes.PlayerGun:
					powerVar[i].durationApplied = POWER_5_DURATION;
					break;
				case (int)PowerTypes.PlayerMagnet:

					powerVar[i].durationApplied = POWER_10_DURATION;
					break;
				case (int)PowerTypes.BigBall:
					powerVar[i].durationApplied = POWER_10_DURATION;
					break;
				case (int)PowerTypes.MultiBall:

					powerVar[i].durationApplied = POWER_7_DURATION;
					break;
				case (int)PowerTypes.FlareBall:

					powerVar[i].durationApplied = POWER_7_DURATION;
					break;
				case (int)PowerTypes.VipBall:
					powerVar[i].durationApplied = POWER_7_DURATION;
					break;
				case (int)PowerTypes.PlayerSlowBall:
					powerVar[i].durationApplied = POWER_10_DURATION;
					break;
				case (int)PowerTypes.AISlowBall:
					powerVar[i].durationApplied = POWER_10_DURATION;
					break;
				case (int)PowerTypes.FastBall:
					powerVar[i].durationApplied = POWER_10_DURATION;
					break;

				}
			//	print ( "kjaskd"+ powerVar[i].durationApplied);

			}
		}
	}
	public struct PowerVariables{
		public bool isWorking;
		public float finishTime;
		public float durationApplied;
	}