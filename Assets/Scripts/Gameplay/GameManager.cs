using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	private static GameManager instance;
	public static GameManager Instance{get{ return instance;}}
	private const float COIN_PROB=0.3f;
    private float POWER_UP_PROB = 0.12f;
	private float DARK_MODE_AI_POWERUP_PROB = 35f;
	private float DARK_MODE_AI_POWERUP_TIME = 10f;
	private const float SPAWN_RATE = 3f;
	private const float BLOKE_MIN_SPAWNX = -3f;
	private const float BLOKE_MAX_SPAWNX = 3f;
	private const float	BLOKE_MIN_SPAWNZ = -7.5f;
	private const float BLOKE_MAX_SPAWNZ = 5f;
	public const int WIN_LIMIT_ENDLESS = 7;
	public const int WIN_LIMIT_DARK = 1;
	private const float BLOKE_MULTIPLIER = 1f;
	private const float WALL_MULTIPLIER = 3f;
	private float PAD_MULTIPLIER = 1f;
	private const float BLOKE_HEIGHT = 1f;
	private const float BLOKE_WIDTH=0.5f;
	private const float BLOKE_HEIGHT_FROM_GROUND = 0.4f;
	private const int TYPE_OF_BLOKE=7;
	private const int CORD_X_MAX = 12 ;
	private const int CORD_Z_MAX= 16;
	private int MAX_POWERUP_COUNT = 33;

	public bool BlokeHit, gameStarted;
	public bool GameOver;

	public GameObject padLong,padShort,bigBall,speedUp,speedDown,flareBall,multiBall,magnetPad,gunPad,VIPBall;
	public GameObject coin;
	public Transform BlokeGroup,DeadPool,BallContainer, SelectableGrounds;
//	public GameObject g,w1,w2,w3,w4;
	public GameObject camera1, camera2, camera3, backPanel1, backPanel2, backPanel3, comingSoon;

	private int currentPlayingLevel, countBlockSpawnerFreq=0;
    private float initialTime;

	private List  <GameObject>  BlastList; 

    private GameObject[] ballList;
    public GameObject specials, MCD, drunk;
	public Material[] blockMaterial;
	public Material blacky;
	public AudioSource a;
	public AudioClip a1,a2,a3,a4;
	public GameObject BlastAnim;

	public GameObject gameCanvas, powerup, gameoverAnimationButton, lightSystem;

	public int coinCount;
	private float AI_Point, player_Point, prevPlayerWallPoint;
	public float AI_BlokePoint,player_BlokePoint;
	public float AI_WallPoint, player_WallPoint;
	public GameObject ground;
	public GameObject AI_Score,player_Score;
	private bool ShowInterAd;
	private float gameStartTime;
	private int currLimit;

	public Text coinsEarned, hitsLeft;
	public float HighScore;
	private bool HighScoreTuta;
	public bool goGameManager=true;

    void Start () {
		gameStartTime = Time.time;
		AI_Point = player_Point = AI_BlokePoint = player_BlokePoint = AI_WallPoint = player_WallPoint =prevPlayerWallPoint =0;
		currLimit = (youdidthistoher.Instance.gameplayType == 1) ? WIN_LIMIT_ENDLESS : WIN_LIMIT_DARK;
		GameOver = false;

		for(int i=0;i<3;i++)
			blockMaterial [i] = youdidthistoher.Instance.blokeMaterials[youdidthistoher.Instance.currentSkinIndexBloke*3+i];

		instance = this;
		DeadPool = GameObject.Find ("DeadPool").transform;
		resetTriggers ();												//flare powerup reset correction
		BlastList = new List<GameObject> ();
        Time.timeScale = 1f;
		////////
		/// 
//		AdManager.Instance.HideBanner();
		/// /////
		SelectableGrounds.GetChild (youdidthistoher.Instance.currentGround).gameObject.SetActive (true);

		camReset ();
		switch (youdidthistoher.Instance.currentCameraMode) {
		case 0:
			camera1.SetActive (true);
			backPanel1.SetActive (true);
			break;
		case 1:
			camera2.SetActive (true);
			backPanel2.SetActive (true);
			break;
		case 2:
			camera3.SetActive (true);
			backPanel3.SetActive (true);
			break;
		}

		if (youdidthistoher.Instance.gameplayType == 0) {
			currentPlayingLevel = youdidthistoher.Instance.currentPlayingLevel;
			LevelMaker (currentPlayingLevel);
			ExtraFeatures ();
		} else if (youdidthistoher.Instance.gameplayType == 1) {
			InvokeRepeating ("BlokeSpawner", SPAWN_RATE, SPAWN_RATE);
		} else if (youdidthistoher.Instance.gameplayType == 2) {
			InvokeRepeating ("BlokeSpawner", SPAWN_RATE, SPAWN_RATE);
			InvokeRepeating ("AIPowerSupplier", DARK_MODE_AI_POWERUP_TIME, DARK_MODE_AI_POWERUP_TIME);
			lightSystem.transform.GetChild(0).gameObject.SetActive (false);		//normal lights
			lightSystem.transform.GetChild(1).gameObject.SetActive (true);		//dark mode lights
			POWER_UP_PROB = 0f;													//disable powerups
		}

		if (youdidthistoher.Instance.gameplayType<=1 && youdidthistoher.Instance.forceTutorialLevel==0)
        {
            if (youdidthistoher.Instance.MCDActive == 1)
            {
				PAD_MULTIPLIER = 0.5f;
				MCD.transform.parent.GetChild (0).gameObject.SetActive (false);
                MCD.SetActive(true);
				AIS.Instance.GetComponent<Rigidbody> ().isKinematic = false;
				AIS.Instance.GetComponent<Rigidbody> ().useGravity = true;
            }
            else if (youdidthistoher.Instance.DrunkActive == 1)
            {
				PAD_MULTIPLIER = 2f;
				MCD.transform.parent.GetChild (0).gameObject.SetActive (false);
                drunk.SetActive(true);
            }
        }
      
    }

	void resetTriggers()
	{
		foreach (Transform t in DeadPool) 
		{
			t.gameObject.GetComponent<BoxCollider> ().isTrigger = false;
			t.SetParent (DeadPool);
		}
	}

	void camReset()
	{
		camera1.SetActive (false);
		camera2.SetActive (false);
		camera3.SetActive (false);
		backPanel1.SetActive (false);
		backPanel2.SetActive (false);
		backPanel3.SetActive (false);
	}

    void Update()
	{	if(GameOver) return;

		if (youdidthistoher.Instance.gameplayType==0) {
			//current Level
			coinsEarned.text = coinCount.ToString ();
			player_Point = player_BlokePoint * BLOKE_MULTIPLIER + (AI_WallPoint)*WALL_MULTIPLIER;
			AI_Point = AI_BlokePoint * BLOKE_MULTIPLIER+(player_WallPoint)*WALL_MULTIPLIER;
			player_Point *= PAD_MULTIPLIER;

			player_Score.GetComponent<Text> ().text = player_Point + "";
			AI_Score.GetComponent<Text> ().text = AI_Point+"";
			if (player_Point > AI_Point) {
				player_Score.GetComponent<Text> ().color = Color.green;
			} else {
				player_Score.GetComponent<Text> ().color = Color.red;
			}

			if (BlokeRemaining() == 0 && goGameManager) {
				//if(false){	

				int levelLooseCount=PlayerPrefs.GetInt("levelLooseCount");
				levelLooseCount++;												//level end count
				if (levelLooseCount % 4 == 0) {
					ShowInterAd = true;
				}
				youdidthistoher.Instance.LevelLooseCount = levelLooseCount;
				GameOver = true;
				if (player_Point > AI_Point) {

					if (player_Point - AI_Point >= 100) {
			//			Social.ReportProgress (GPGSIds.achievement_im_just_so_good, 100.0f, (bool success) => {
			//			});
					}

					int levelReached =PlayerPrefs.GetInt ("campaignLevelReached");
					int currentLevel = PlayerPrefs.GetInt ("currentPlayingLevel");

					if(currentLevel ==levelReached){
						if (currentLevel == youdidthistoher.Instance.totalNoOfLevels) {
							comingSoon.SetActive (true);
							youdidthistoher.Instance.currency += 2000;
							youdidthistoher.Instance.Save ();
			//				Social.ReportProgress (GPGSIds.achievement_pong_maniac, 100.0f, (bool success) => {
			//				});
						} else {
							youdidthistoher.Instance.campaignLevelReached = ++levelReached;
							youdidthistoher.Instance.Save ();
							if (levelReached == 50) {
			//					Social.ReportProgress (GPGSIds.achievement_getting_the_hang_of_it, 100.0f, (bool success) => {
			//					});			
							} else if (levelReached == 100) {
			//					Social.ReportProgress (GPGSIds.achievement_professional_ponger, 100.0f, (bool success) => {
			//					});			
							} else if (levelReached == 150) {
			//					Social.ReportProgress (GPGSIds.achievement_master_mcawesomeville, 100.0f, (bool success) => {
			//					});		
							}
							else if (levelReached == 200) {
			//					Social.ReportProgress (GPGSIds.achievement_pong_jedi, 100.0f, (bool success) => {
			//					});			
							}
							if ((levelReached) % 5 == 0) {
								//////
								//							AdManager.Instance.ShowVideo ();
								/////
								ShowInterAd = false;

							}
						}
					}
					//player is winner

					gogoScreen(0);
					print ("It is indeed coming here");
				}else{
					gogoScreen(1);
				}				
			}
		} else if (youdidthistoher.Instance.gameplayType==1||youdidthistoher.Instance.gameplayType==2) {
			if (Time.time - gameStartTime > 100f) {
				ShowInterAd = true;
			}

			coinsEarned.text = coinCount.ToString ();
			player_Point = player_BlokePoint * BLOKE_MULTIPLIER + (AI_WallPoint)*WALL_MULTIPLIER ;
			AI_Point = AI_BlokePoint * BLOKE_MULTIPLIER + (player_WallPoint )*WALL_MULTIPLIER;
			player_Point *= PAD_MULTIPLIER;
			player_Score.GetComponent<Text> ().text = player_Point + "";			
			AI_Score.GetComponent<Text> ().text = AI_Point+"";
			if (prevPlayerWallPoint != player_WallPoint) {
				hitsLeft.text = (currLimit - player_WallPoint).ToString();
				hitsLeft.gameObject.GetComponent<Animation> ().Play ();
				prevPlayerWallPoint = player_WallPoint;
			}
			if (player_Point > AI_Point) {
				player_Score.GetComponent<Text> ().color = Color.green;
			} else {
				player_Score.GetComponent<Text> ().color = Color.red;
			}
			if (youdidthistoher.Instance.gameplayType == 1) {
				if (player_WallPoint >= WIN_LIMIT_ENDLESS) {
					GameOver = true;

					if (player_Point - AI_Point >= 100) {
			//			Social.ReportProgress (GPGSIds.achievement_im_just_so_good, 100.0f, (bool success) => {
			//			});
					}

					if (youdidthistoher.Instance.HighScoreEndless < player_Point) {
						if (player_Point > AI_Point) {
							youdidthistoher.Instance.HighScoreEndless = (int)player_Point;				//highscore is broken only if player points are greater than AI points
							youdidthistoher.Instance.Save ();

			//				Social.ReportScore((long)player_Point, GPGSIds.leaderboard_endless_mode, (bool success) => {});

							gogoScreen (0, (int)player_Point);
						}
						else if (player_Point < AI_Point) {
							player_Point = -1;
							gogoScreen (1, (int)player_Point);
						}
					}else {
						gogoScreen (1, (int)player_Point);
					}
				}
			}else {
				if (player_WallPoint >= WIN_LIMIT_DARK) {
					GameOver = true;

					if (player_Point - AI_Point >= 100) {
			//			Social.ReportProgress (GPGSIds.achievement_im_just_so_good, 100.0f, (bool success) => {
			//			});
					}

					if (player_Point >= 100) {
			//			Social.ReportProgress (GPGSIds.achievement_shadow_fighter, 100.0f, (bool success) => {
			//			});
					}

					if (youdidthistoher.Instance.HighScoreDark < player_Point) {
						if (player_Point > AI_Point) {
							youdidthistoher.Instance.HighScoreDark = (int)player_Point;
							youdidthistoher.Instance.Save ();

			//				Social.ReportScore((long)player_Point, GPGSIds.leaderboard_dark_mode, (bool success) => {});

							gogoScreen (0, (int)player_Point);
						} else if (player_Point < AI_Point) {
							player_Point = -1;
							gogoScreen (1, (int)player_Point);
						}
					} else{
						gogoScreen (1, (int)player_Point);
					}
				}
			}
		}
		EmptyBlastList ();
    }

	void BlokeSpawner() {
		if (countBlockSpawnerFreq > 20) {				// if more than x attempts have been made to place block
			bool isFilled = true;
//			print ("more than 50");
			foreach (Transform temp in BlokeGroup) {
				if (!temp.gameObject.activeInHierarchy) {
					countBlockSpawnerFreq = 0;
					GameObject obj = ObjectPool.Instance.GetObject ();
					obj.transform.position = temp.position;
					obj.GetComponent<Block> ().SetBlock (1);		//type 1 block
					return;
//					print ("block found");
				}
			}

			CancelInvoke (methodName: "BlokeSpawner");
//			print ("invoke cancelled");
			InvokeRepeating ("BlokeSpawner", SPAWN_RATE * 20f, SPAWN_RATE);			//start function after x times the spawn rate
			countBlockSpawnerFreq = 0;
		} else {
//			print ("less than 50");
			int CordX, CordZ,CordType;
			CordX = Random.Range (0, CORD_X_MAX);
			CordZ = Random.Range (0, CORD_Z_MAX);
			int typesOfBlocks = (youdidthistoher.Instance.gameplayType == 1) ? 3 : 7;		// only three types of blocks for endless while all 7 types of blocks for dark mode
			CordType = Random.Range (0,typesOfBlocks);

			Vector3 pos = new Vector3 (BLOKE_MIN_SPAWNX + CordX * BLOKE_WIDTH, BLOKE_HEIGHT_FROM_GROUND, BLOKE_MIN_SPAWNZ + CordZ * BLOKE_HEIGHT);
			bool canSpawn = true;
			Transform[] childObjects = BlokeGroup.GetComponentsInChildren<Transform> ();
			foreach (Transform temp in childObjects) {
				if (temp.gameObject.activeInHierarchy && temp.position == pos) {
					canSpawn = false;
				}
			}
			if (canSpawn) {
				GameObject obj = ObjectPool.Instance.GetObject ();
				obj.transform.position = pos;
				obj.GetComponent<Block> ().SetBlock (CordType);
				countBlockSpawnerFreq = 0;
			} else {
				BlokeSpawner ();
				countBlockSpawnerFreq++;
			}
		}
	}

	void LevelMaker(int levelNo)
	{
		string level = PlayerPrefs.GetString("level" + levelNo);
		foreach (string blokeName in level.Split('*'))
		{
			string[] blokeValues = blokeName.Split('-');
			int type, i, j;
			int.TryParse(blokeValues[0], out type);
			int.TryParse(blokeValues[1], out i);
			int.TryParse(blokeValues[2], out j);

			//BlokeGroup.Find("Bloke" + type + "-" + i.ToString() + "X-" + j.ToString() + "Z").gameObject.SetActive(true);
			GameObject obj = ObjectPool.Instance.GetObject();
			obj.transform.position = new Vector3 (BLOKE_MIN_SPAWNX+i*BLOKE_WIDTH, BLOKE_HEIGHT_FROM_GROUND ,BLOKE_MIN_SPAWNZ+j*BLOKE_HEIGHT);
			obj.GetComponent<Block> ().SetBlock (type-1);
		}
	}

    private void ExtraFeatures()
    {
        ballList = PowerUp.Instance.ballList;
        if (currentPlayingLevel % 10 == 0)
        {
            foreach (GameObject ball in ballList)
            {
                ball.transform.GetChild(3).gameObject.SetActive(true);
                ball.transform.GetChild(7).gameObject.SetActive(true);
            }
            specials.SetActive(true);
            specials.transform.GetChild(0).gameObject.SetActive(true);
        }
    }


    // make coins at the bloke places
    public void makeCoin(GameObject tempBloke){
	    //Random.Range (0f, 1f);

		if (Random.Range(0, 100) < COIN_PROB*100 ) {
			Instantiate (coin, tempBloke.transform.position, Quaternion.Euler (0, 0, 90));
		}
	}
    // power up created at bloke places	
	public void makePowerUp(GameObject tempBloke,bool turn){
		if (youdidthistoher.Instance.MCDActive == 1 ||youdidthistoher.Instance.DrunkActive == 1) {
			return;
		}
		float chance = Random.Range (0f, 1f);
		if (chance < POWER_UP_PROB) {
			if (MAX_POWERUP_COUNT != -1) {
			//	print ("chaling");
				int powerChoice = Random.Range (0, MAX_POWERUP_COUNT);
				//  		powerChoice = 32;
				//		var pu=new GameObject();
				GameObject pu;
				/*	if (powerChoice < 4) pu = Instantiate(padLong, tempBloke.transform.position, Quaternion.identity);
            else if (powerChoice < 8) pu = Instantiate(padShort, tempBloke.transform.position, Quaternion.identity);
            else if (powerChoice < 12) pu = Instantiate(bigBall, tempBloke.transform.position, Quaternion.identity);
            else if (powerChoice < 16) pu = Instantiate(speedUp, tempBloke.transform.position, Quaternion.identity);
            else if (powerChoice < 20) pu = Instantiate(speedDown, tempBloke.transform.position, Quaternion.identity);
            else if (powerChoice < 23) pu = Instantiate(flareBall, tempBloke.transform.position, Quaternion.identity);
            else if (powerChoice < 26) pu = Instantiate(multiBall, tempBloke.transform.position, Quaternion.identity);
            else if (powerChoice < 29) pu = Instantiate(VIPBall, tempBloke.transform.position, Quaternion.identity);
            else if (powerChoice < 31) pu = Instantiate(gunPad, tempBloke.transform.position, Quaternion.identity);
            else if (powerChoice < 33) pu = Instantiate(magnetPad, tempBloke.transform.position, Quaternion.identity);
            else return;
*/
				if (powerChoice < 4)
					pu = Instantiate (bigBall, tempBloke.transform.position, Quaternion.identity);
				else if (powerChoice < 8)
					pu = Instantiate (padShort, tempBloke.transform.position, Quaternion.identity);
				else if (powerChoice < 12)
					pu = Instantiate (padLong, tempBloke.transform.position, Quaternion.identity);
				else if (powerChoice < 15)
					pu = Instantiate (flareBall, tempBloke.transform.position, Quaternion.identity);
				else if (powerChoice < 18)
					pu = Instantiate (VIPBall, tempBloke.transform.position, Quaternion.identity);
				else if (powerChoice < 22)
					pu = Instantiate (speedUp, tempBloke.transform.position, Quaternion.identity);
				else if (powerChoice < 26)
					pu = Instantiate (speedDown, tempBloke.transform.position, Quaternion.identity);
				else if (powerChoice < 28)
					pu = Instantiate (gunPad, tempBloke.transform.position, Quaternion.identity);
				else if (powerChoice < 31)
					pu = Instantiate (multiBall, tempBloke.transform.position, Quaternion.identity);
				else if (powerChoice < 33)
					pu = Instantiate (magnetPad, tempBloke.transform.position, Quaternion.identity);
				else
					return;

				pu.GetComponent<rotator> ().turn = turn;
			}
        }

    }

    bool turn;
	public void Blast(GameObject blokeTemp, bool turn=true){

        this.turn = turn;
        Transform[] childObjects = BlokeGroup.GetComponentsInChildren<Transform> ();
		Vector3 pos = blokeTemp.transform.position;	
		foreach (Transform temp in childObjects) {
            if (temp == BlokeGroup || (temp.position.x == pos.x && temp.position.z == pos.z))
                continue;
            else if (temp.position.x >= pos.x - 0.5f && temp.position.x <= pos.x + 0.5f &&
                temp.position.z >= pos.z - 1 && temp.position.z <= pos.z + 1)
            {
                if (!BlastList.Contains(temp.gameObject))
                {
                    BlastList.Add(temp.gameObject);
                }
            }

            /*if (temp == BlokeGroup)
			    continue;
            
			for (int i = (int)pos.x - 1; i <= (int)pos.x + 1; i++) {
				for (int j = (int)pos.z - 1; j <= (int)pos.z + 1; j++) {
					if (i == (int)pos.x && j == (int)pos.z) {
						continue;			
					}
					if (temp.position.x == i && temp.position.z == j) {
						if (!BlastList.Contains (temp.gameObject)) {
							BlastList.Add (temp.gameObject);
						}
					}
				}
			}*/
        }

    }

	private void EmptyBlastList(){
		if (BlastList.Count > 0) {
			GameObject temp = BlastList [0];
			if(BlastList.Remove (BlastList [0])){
//                print(temp.GetComponent<Block>().blockType);
                temp.GetComponent<Block>().HitBlock(turn);
                temp.GetComponent<Block>().ResetBlock(turn);

            }
        }
	}

	public void gogoScreen(int state, int score=0)
	{	/////
		if(ShowInterAd){
			ShowInterAd = false;
			/////how
			print("Working");
			Invoke ("showAd", 1f);
			///// 
		}
		ObjectPool.Instance.Reset();

		coinsEarned.gameObject.transform.parent.gameObject.SetActive(false);
		gameoverAnimationButton.GetComponent<UIAnimController>().PanelActive();
		gameCanvas.GetComponent<GameUI> ().gameOver(state, GameManager.Instance.coinCount, score);
		for (int i = 0; i < powerup.GetComponent<PowerUp> ().ballList.Length; i++)
			powerup.GetComponent<PowerUp> ().ballList [i].SetActive (false);
	}

	private void showAd()
	{
	//	AdManager.Instance.ShowInterstitial();
	}

	private int BlokeRemaining(){
		int count = 0;
		foreach (Transform t in BlokeGroup.transform) {
			if (t.gameObject.activeSelf) {
				Block temp = t.GetComponent<Block> ();
				if(temp!=null && ((int)temp.blockType<3||temp.blockType==BlockTypes.Blast))
					count++;
			}
		}
		return count;
	}

	public void BlokePoint(bool turn){
		if (turn) {
			player_BlokePoint++;	
		}
		else {
			AI_BlokePoint++;
		}
	}
	public void BlastAnimation(Vector3 pos){
		GameObject temp = Instantiate (BlastAnim, pos, Quaternion.identity);
		Destroy (temp, 3f);
	}

	private void AIPowerSupplier()
	{
		float chance = Random.Range (0, 100);
		if (chance < DARK_MODE_AI_POWERUP_PROB) {
			int powerUpDecideFactor = Random.Range (0, 100);
			if (powerUpDecideFactor < 33) {
				PowerUp.Instance.PU (PowerTypes.AILong);
				PowerUp.Instance.PU (PowerTypes.AILong);
			} else if (powerUpDecideFactor < 66) {
				PowerUp.Instance.PU (PowerTypes.AISlowBall);
				PowerUp.Instance.PU (PowerTypes.AISlowBall);
			} else {
				PowerUp.Instance.PU (PowerTypes.VipBall, false);		//AI
				PowerUp.Instance.PU (PowerTypes.VipBall, false);
			}
		}
	}

	public void powerupTypeLimiter(int no)
	{
		MAX_POWERUP_COUNT = no;
	}
}
