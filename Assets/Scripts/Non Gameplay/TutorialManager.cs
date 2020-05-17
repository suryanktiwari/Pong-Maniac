using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

	private int OFFSET = 4; 	//for endless, dark, mcd and drunk
	private int[] tutorialLevels = {1,2,3,4,5,6,7,8,9,10,11,12,15,20};
	private int curDataElement=0, curTutElement;

	public GameObject tutorialPanel, ball, kheloKudoBadho, MCD;
	public GameObject[] powerUpButtons;
	private string[] level, curData;

	private bool endlessPlay=false, darkPlay=false, MCDPlay=false, drunkPlay=false;

	TextAsset txt;

	void Start () {
		if(youdidthistoher.Instance.currentPlayingLevel == youdidthistoher.Instance.campaignLevelReached || youdidthistoher.Instance.currentPlayingLevel<=tutorialLevels[tutorialLevels.Length-1])
			powerupAdjuster (youdidthistoher.Instance.currentPlayingLevel);

		if (youdidthistoher.Instance.firstEndlessPlay == 1 && youdidthistoher.Instance.gameplayType == 1) {
			endlessPlay = true;
			loadData (-4);
		} else if (youdidthistoher.Instance.firstDarkPlay == 1 && youdidthistoher.Instance.gameplayType == 2) {
			darkPlay = true;
			loadData (-3);
		} else if (youdidthistoher.Instance.tutorialLevel <= tutorialLevels [tutorialLevels.Length - 1] && youdidthistoher.Instance.currentPlayingLevel == youdidthistoher.Instance.tutorialLevel && youdidthistoher.Instance.gameplayType == 0) {
			if(youdidthistoher.Instance.currentPlayingLevel<=tutorialLevels[tutorialLevels.Length-1])
				powerupAdjuster (youdidthistoher.Instance.currentPlayingLevel-1);
			for (int i = 0; i < tutorialLevels.Length; i++) {
				if (youdidthistoher.Instance.tutorialLevel == tutorialLevels [i]) {
					curTutElement = i;
					break;
				}
			}
			loadData (curTutElement);
		} else if (youdidthistoher.Instance.MCDActive == 1 && youdidthistoher.Instance.firstMCDPlay == 1) {
			MCDPlay = true;
			MCD.SetActive (false);
			loadData (-2);
			youdidthistoher.Instance.tutorialOnly = true;
		}
		else if (youdidthistoher.Instance.DrunkActive == 1 && youdidthistoher.Instance.firstDrunkPlay == 1) {
			drunkPlay = true;
			loadData (-1);
			youdidthistoher.Instance.tutorialOnly = true;
		}
		else if (youdidthistoher.Instance.forceTutorialLevel != 0) {
			loadData (youdidthistoher.Instance.forceTutorialLevel - 1);
			powerupAdjuster (youdidthistoher.Instance.forceTutorialLevel);
			youdidthistoher.Instance.tutorialOnly = true;
		}
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			skip ();
		}
	}

	void loadData(int levelNo)
	{
		txt = (TextAsset)Resources.Load ("Tutorial", typeof(TextAsset));
		string content = txt.text;
		level = content.Split ('\n');
		curData = level [levelNo+OFFSET].Split ('*');
		tutorialPanel.SetActive (true);
		ball.SetActive (false);
		tutorialPanel.transform.GetChild (0).GetComponent<Text> ().text = curData [curDataElement];
	}

	public void nextData()
	{
		if (curDataElement + 1 < curData.Length) {
			tutorialPanel.transform.GetChild (0).GetComponent<Text> ().text = curData [++curDataElement];
		} else {
			skip ();
		}
	}

	public void skip()
	{
		tutorialPanel.SetActive (false);
		ball.SetActive (true);
		ball.GetComponent<BallS>().startTheGameMan();
		if (endlessPlay) {
			endlessPlay = false;
			youdidthistoher.Instance.firstEndlessPlay = 0;
		} else if (darkPlay) {
			darkPlay = false;
			youdidthistoher.Instance.firstDarkPlay = 0;
		} else if (MCDPlay) {
			youdidthistoher.Instance.firstMCDPlay = 0;
			MCD.SetActive (true);
		} else if (drunkPlay) {
			youdidthistoher.Instance.firstDrunkPlay = 0;
		}
		else
		{
            if (curTutElement + 1 < tutorialLevels.Length)
            {
                youdidthistoher.Instance.tutorialLevel = tutorialLevels[curTutElement + 1];
            }
            else
            {
                youdidthistoher.Instance.tutorialLevel = -1;
            }
		}
		youdidthistoher.Instance.Save ();
	}

	void powerupAdjuster (int levelNo) {
		GameObject pu;
		bool powerupDu;
		if (youdidthistoher.Instance.campaignLevelReached == levelNo || youdidthistoher.Instance.forceTutorialLevel!=0)
			powerupDu = true;
		else
			powerupDu = false;
		switch (levelNo) {
		case 1:
		case 2:
		case 3:
			GameManager.Instance.powerupTypeLimiter (-1);
			disablePowerupFromPause (7);
			if(youdidthistoher.Instance.gameplayType<=1)
				kheloKudoBadho.SetActive (true);
			break;
		case 4:
			GameManager.Instance.powerupTypeLimiter (4);
			if (powerupDu) {
				pu = Instantiate (GameManager.Instance.bigBall, GameManager.Instance.gameObject.transform.position, Quaternion.identity);
				pu.GetComponent<rotator> ().turn = true;
			}
			disablePowerupFromPause (6);
			break;
		case 5:
			GameManager.Instance.powerupTypeLimiter (12);
			if (powerupDu) {
				pu = Instantiate (GameManager.Instance.padLong, GameManager.Instance.gameObject.transform.position, Quaternion.identity);
				pu.GetComponent<rotator> ().turn = true;
			}
			disablePowerupFromPause (5);
			break;
		case 6:
			GameManager.Instance.powerupTypeLimiter (15);
			if (powerupDu) {
				pu = Instantiate (GameManager.Instance.flareBall, GameManager.Instance.gameObject.transform.position, Quaternion.identity);
				pu.GetComponent<rotator> ().turn = true;
			}
			disablePowerupFromPause (4);
			break;
		case 7:
			GameManager.Instance.powerupTypeLimiter (18);
			if (powerupDu) {
				pu = Instantiate (GameManager.Instance.VIPBall, GameManager.Instance.gameObject.transform.position, Quaternion.identity);
				pu.GetComponent<rotator> ().turn = true;
			}
			disablePowerupFromPause (3);
			break;
		case 8:
			GameManager.Instance.powerupTypeLimiter (26);
			if (powerupDu) {
				pu = Instantiate (GameManager.Instance.speedDown, GameManager.Instance.gameObject.transform.position, Quaternion.identity);
				pu.GetComponent<rotator> ().turn = true;
			}
			disablePowerupFromPause (3);
			break;
		case 9:
			GameManager.Instance.powerupTypeLimiter (28);
			if (powerupDu) {
				pu = Instantiate (GameManager.Instance.gunPad, GameManager.Instance.gameObject.transform.position, Quaternion.identity);
				pu.GetComponent<rotator> ().turn = true;
			}
			disablePowerupFromPause (2);
			break;
		case 10:
			GameManager.Instance.powerupTypeLimiter (28);
			disablePowerupFromPause (2);
			break;
		case 11:
			GameManager.Instance.powerupTypeLimiter (31);
			if (powerupDu) {
				pu = Instantiate (GameManager.Instance.multiBall, GameManager.Instance.gameObject.transform.position, Quaternion.identity);
				pu.GetComponent<rotator> ().turn = true;
			}
			disablePowerupFromPause (1);
			break;
		case 12:
			GameManager.Instance.powerupTypeLimiter (33);
			if (powerupDu) {
				pu = Instantiate (GameManager.Instance.magnetPad, GameManager.Instance.gameObject.transform.position, Quaternion.identity);
				pu.GetComponent<rotator> ().turn = true;
			}
			break;
		case 15:
		case 20:
			GameManager.Instance.powerupTypeLimiter(33);
			break;
		default:
			break;
		}
	}

	void OnDestroy()
	{
		if(MCDPlay && youdidthistoher.Instance.skinAvailabilityMCD == 0) {
			youdidthistoher.Instance.MCDActive = 0;
		}
		if (drunkPlay && youdidthistoher.Instance.skinAvailabilityDrunk == 0) {
			youdidthistoher.Instance.DrunkActive = 0;
		}
		youdidthistoher.Instance.Save ();

		if (youdidthistoher.Instance.firstEndlessPlay == 0 && youdidthistoher.Instance.firstDarkPlay == 0 && youdidthistoher.Instance.tutorialLevel == -1) {
	/*		Social.ReportProgress (GPGSIds.achievement_baby_steps, 100.0f, (bool success) => {
                if (success)
                    print("baby steps");
                else
                    print("noi baby steps");
			});		
            */
		}
	}

	void disablePowerupFromPause(int limit)
	{
		for (int i = 6; i >=7-limit; i--) {
			powerUpButtons [i].SetActive(false);
		}
	}
}

/*
	if (powerChoice < 4) pu = Instantiate(bigBall, tempBloke.transform.position, Quaternion.identity);
			else if (powerChoice < 8) pu = Instantiate(padShort, tempBloke.transform.position, Quaternion.identity);
			else if (powerChoice < 12) pu = Instantiate(padLong, tempBloke.transform.position, Quaternion.identity);
			else if (powerChoice < 15) pu = Instantiate(flareBall, tempBloke.transform.position, Quaternion.identity);
			else if (powerChoice < 18) pu = Instantiate(VIPBall, tempBloke.transform.position, Quaternion.identity);
			else if (powerChoice < 22) pu = Instantiate(speedUp, tempBloke.transform.position, Quaternion.identity);
			else if (powerChoice < 26) pu = Instantiate(speedDown, tempBloke.transform.position, Quaternion.identity);
			else if (powerChoice < 28) pu = Instantiate(gunPad, tempBloke.transform.position, Quaternion.identity);
			else if (powerChoice < 31) pu = Instantiate(multiBall, tempBloke.transform.position, Quaternion.identity);
			else if (powerChoice < 33) pu = Instantiate(magnetPad, tempBloke.transform.position, Quaternion.identity);
			else return;
*/