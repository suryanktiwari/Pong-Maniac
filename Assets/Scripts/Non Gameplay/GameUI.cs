using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
	
	private const int noOfWinText=20;
	private const int noOfLoseText=20;
	private const float SET_VOLUME_TYPE01_THEME = 0.25f;
	private const float SET_VOLUME_TYPE01_EFFECTS = 0.8f;
	private const float SET_VOLUME_TYPE2_THEME = 1f;
	private const float SET_VOLUME_TYPE2_EFFECTS = 0.1f;
	private float alphaVal=0.25f;
	private float alphaStepFactor = 0.15f;
	private const int noOfPowerUps = 7;
	private const float moveSpeed = 2f;

	public GameObject pausePanel, gameoverPanel, pauseButton, gameoverAnimationButton, continueButton, restartButtonGO, restartButtonPause, blockGroup, themeSource, effectsSource, inputHandler, comingSoon, doubleCoins;
	public GameObject soundButton, magnetButton, gunButton, multiBallButton, vipButton, bigBallButton, padLongButton, flareButton, gridLock, darknessPanel, iconHolder, pauseAnimatable;

	public Text goText, descriptionText, levelText, coinsEarned, highScore;

	private int[] isSelected;
	private bool secondPause=false;

	private string[] parsed;
	private TextAsset txt;
	private float SET_VOLUME;
	public AudioClip type01, type2, gWin, gLose;

	private Color tempCol;

	void Start () {
		isSelected = new int[noOfPowerUps];
		parsed = new string[noOfWinText];
		tempCol = gridLock.GetComponent<Image> ().color;
		if(youdidthistoher.Instance.tutorialOnly)
			restartButtonPause.GetComponent<Button> ().interactable = false;
		if (youdidthistoher.Instance.backgroundMusic == 0)
			themeSource.SetActive(false);
		if(youdidthistoher.Instance.effectsSound==0)
			effectsSource.SetActive(false);
		if(youdidthistoher.Instance.backgroundMusic==0&&youdidthistoher.Instance.effectsSound==0)
			soundButton.transform.GetChild (1).gameObject.SetActive (true);

		if (youdidthistoher.Instance.DrunkActive == 1 || youdidthistoher.Instance.MCDActive == 1) {
		//	gunButton.GetComponent<Button> ().interactable = false;
		//	magnetButton.GetComponent<Button> ().interactable = false;
		//	padLongButton.GetComponent<Button> ().interactable = false;
			gunButton.SetActive(false);
			magnetButton.SetActive (false);
			padLongButton.SetActive (false);
		}

		if (youdidthistoher.Instance.gameplayType == 0) {
			levelText.text = (youdidthistoher.Instance.currentPlayingLevel).ToString ();
			themeSource.GetComponent<AudioSource> ().clip = type01;
			SET_VOLUME = SET_VOLUME_TYPE01_THEME;
			iconHolder.transform.GetChild (0).gameObject.SetActive (true);
		}
		else if (youdidthistoher.Instance.gameplayType == 1) {
			levelText.transform.parent.GetComponent<Text> ().color = Color.grey;
			levelText.color = Color.grey;
			levelText.text = "-";
			themeSource.GetComponent<AudioSource> ().clip = type01;
			themeSource.GetComponent<AudioSource> ().volume = SET_VOLUME_TYPE01_THEME;
			effectsSource.GetComponent<AudioSource> ().volume = SET_VOLUME_TYPE01_EFFECTS;
			SET_VOLUME = SET_VOLUME_TYPE01_THEME;
			highScore.transform.parent.gameObject.SetActive (true);
			highScore.text = youdidthistoher.Instance.HighScoreEndless.ToString();
			highScore.transform.parent.GetChild(1).GetChild(0).GetComponent<Text>().text = GameManager.WIN_LIMIT_ENDLESS.ToString();
			continueButton.GetComponent<Button> ().interactable = false;
//			continueButton.GetComponent<ButtonBackRotator> ().enabled = false;
			iconHolder.transform.GetChild (1).gameObject.SetActive (true);
		} else if (youdidthistoher.Instance.gameplayType == 2) {
			levelText.transform.parent.GetComponent<Text> ().color = Color.grey;
			levelText.color = Color.grey;
			levelText.text = "-";
			themeSource.GetComponent<AudioSource> ().Play ();
			themeSource.GetComponent<AudioSource> ().clip = type2;
			themeSource.GetComponent<AudioSource> ().volume = SET_VOLUME_TYPE2_THEME;
			effectsSource.GetComponent<AudioSource> ().volume = SET_VOLUME_TYPE2_EFFECTS;
			themeSource.GetComponent<AudioSource> ().Play ();
			SET_VOLUME = SET_VOLUME_TYPE2_THEME;
			highScore.transform.parent.gameObject.SetActive (true);
			highScore.text = youdidthistoher.Instance.HighScoreDark.ToString ();
			highScore.transform.parent.GetChild(1).GetChild(0).GetComponent<Text>().text = GameManager.WIN_LIMIT_DARK.ToString();
			continueButton.GetComponent<Button> ().interactable = false;
//			continueButton.GetComponent<ButtonBackRotator> ().enabled = false;
			iconHolder.transform.GetChild (2).gameObject.SetActive (true);
		}
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if (pausePanel.activeInHierarchy || gameoverPanel.activeInHierarchy) {
				SceneManager.LoadScene ("Main Scene");
			} else {
				pauseAnimatable.GetComponent<UIAnimController> ().PanelActive ();
		//		AdManager.Instance.ShowBanner ();
			}
		}
			
	}

	public void gameOver(int state, int coinCount, int score=0)
	{
		showAd ();
		youdidthistoher.Instance.forceTutorialLevel = 0;
		themeSource.GetComponent<AudioSource> ().volume = 0f;
		inputHandler.SetActive (false);
		coinsEarned.text = coinCount.ToString ();
		pauseButton.GetComponent<Button> ().interactable = false;

		if(coinCount==0)
			doubleCoins.SetActive (false);

		if (youdidthistoher.Instance.tutorialOnly) {
			continueButton.GetComponent<Button> ().interactable = false;
			restartButtonGO.GetComponent<Button> ().interactable = false;
		}
		switch (state) 
		{
		case 0:
			if (youdidthistoher.Instance.gameplayType == 0) {
				txt = (TextAsset)Resources.Load ("Text/win", typeof(TextAsset));
				parsed = txt.text.Split ("\n" [0]);
				descriptionText.text = parsed [Random.Range (0, noOfWinText)];
			} else if (youdidthistoher.Instance.gameplayType == 1) {
				descriptionText.text = "High Score Broken!!!\nNew High Score: " + score.ToString ();
				descriptionText.transform.GetChild (0).GetComponent<Text> ().text = "Stats:";
			} else {
				descriptionText.text = "High Score Broken!!!\nNew High Score: " + score.ToString ();
				descriptionText.transform.GetChild (0).GetComponent<Text> ().text = "Stats:";
			}
			effectsSource.GetComponent<AudioSource> ().PlayOneShot (gWin, 0.5f);
			goText.text = "SUSTAINED";
			break;
		case 1:
			if (youdidthistoher.Instance.gameplayType == 0) {
				txt = (TextAsset)Resources.Load ("Text/lose", typeof(TextAsset));
				parsed = txt.text.Split ("\n" [0]);
				descriptionText.text = parsed [Random.Range (0, noOfLoseText)];
				if (youdidthistoher.Instance.currentPlayingLevel == youdidthistoher.Instance.campaignLevelReached) {
					continueButton.transform.GetChild(0).GetComponent<ButtonBackRotator> ().enabled = false;
					continueButton.GetComponent<Button> ().interactable = false;
				}
			}
			else if (youdidthistoher.Instance.gameplayType == 1) {
				if(score==-1)
					descriptionText.text = "You beat the High Score but Alas! you couldn't beat the AI, what a waste";
				else
					descriptionText.text = "High Score: "+youdidthistoher.Instance.HighScoreEndless.ToString() + "\nYour Score: "+ score.ToString();
				descriptionText.transform.GetChild (0).GetComponent<Text> ().text = "Stats:";
			} else {
				if(score==-1)
					descriptionText.text = "You beat the High Score but Alas! you couldn't beat the AI, what a waste";
				else
					descriptionText.text = "High Score: "+youdidthistoher.Instance.HighScoreDark.ToString() + "\nYour Score: "+ score.ToString();
				descriptionText.transform.GetChild (0).GetComponent<Text> ().text = "Stats:";
			}
			effectsSource.GetComponent<AudioSource> ().PlayOneShot (gLose);
			goText.text = "THUMPED";
			break;
		}
		youdidthistoher.Instance.currency+=coinCount;
		youdidthistoher.Instance.Save ();
		Time.timeScale = 0f;
	}
		
	public void pause()
	{
		GameManager.Instance.goGameManager = false;
		Time.timeScale = 0f;
		inputHandler.SetActive (false);
		themeSource.GetComponent<AudioSource> ().volume = 0f;
        effectsSource.GetComponent<AudioSource>().volume = 0f;
		pauseButton.GetComponent<Button> ().interactable = false;
		loadPowerUpCount ();
		if(secondPause)
			resetPowerUps ();
		if (youdidthistoher.Instance.gameplayType == 2) //DARK MODE
		{
			darknessPanel.SetActive (true);
			gridLock.SetActive (false);
		}
	}

	public void unPause()
	{
		GameManager.Instance.goGameManager = true;
		secondPause = true;
		inputHandler.SetActive (true);
		for (int i = 0; i < noOfPowerUps; i++) {
			if (isSelected [i] == 1) {
				switch (i) 
				{
				case 0:	
					PowerUp.bigBallPurchased = true;
					break;
				case 1:
					PowerUp.flareBallPurchased = true;
					break;
				case 2:
					PowerUp.gunPadPurchased = true;
					break;
				case 3:
					PowerUp.padLongPurchased = true;
					break;
				case 4:
					PowerUp.magnetPadPurchased = true;
					break;
				case 5:
					PowerUp.VIPBallPurchased = true;
					break;
				case 6:
					PowerUp.multiBallPurchased = true;			
					break;
				}
				isSelected[i] = 0;
			}
		}
		youdidthistoher.Instance.Save ();
		Time.timeScale = 1f;
        if (youdidthistoher.Instance.gameplayType == 2)
        {
            effectsSource.GetComponent<AudioSource>().volume = SET_VOLUME_TYPE2_EFFECTS;
            themeSource.GetComponent<AudioSource>().volume = SET_VOLUME_TYPE2_THEME;
        }
        else
        {
            effectsSource.GetComponent<AudioSource>().volume = SET_VOLUME_TYPE01_EFFECTS;
            themeSource.GetComponent<AudioSource>().volume = SET_VOLUME_TYPE01_THEME;
        }
        pauseButton.GetComponent<Button> ().interactable = true;
	}

	public void restart()
	{
		Time.timeScale = 1.0f;
		gameoverAnimationButton.GetComponent<UIAnimController>().PanelInactive();
		SceneManager.LoadScene ("Pong_Breaker");
        ObjectPool.Instance.Reset();

	}

	public void back()
	{
		youdidthistoher.Instance.forceTutorialLevel = 0;
	//	AdManager.Instance.HideBanner ();
		youdidthistoher.Instance.tutorialOnly = false;
		for (int i = 0; i < noOfPowerUps; i++) {
			if (isSelected [i] == 1) {
				youdidthistoher.Instance.powerUpArray [i]++;
			}
		}
		Time.timeScale = 1f;
        ObjectPool.Instance.Reset();
        SceneManager.LoadScene ("Main Scene");    
	  }

	public void powerUpSelected(int buttonNo)
	{
		int selectUp = 0;
		switch (buttonNo) {
		case 1:		//flare
			if (isSelected [buttonNo] == 0) {
				//if button is not already selected
				if (youdidthistoher.Instance.powerUpArray [buttonNo] > 0) {
					flareButton.GetComponent<Animator> ().ResetTrigger ("flareDown");
					flareButton.GetComponent<Animator> ().SetTrigger ("flareUp");
					selectUp = 1;
					if (isSelected [5] == 1) {
						isSelected [5] = 0;
						alphaVal -= alphaStepFactor;
						youdidthistoher.Instance.powerUpArray [5]++;
						vipButton.GetComponent<Animator> ().ResetTrigger ("vipUp");
						vipButton.GetComponent<Animator> ().SetTrigger ("vipDown");
					}
				} else {
					//if there is no ammo left
					flareButton.GetComponent<Animator>().SetTrigger("none");
					selectUp = -1;
				}
			} else {
				//if button is already selected
				flareButton.GetComponent<Animator> ().ResetTrigger ("flareUp");
				flareButton.GetComponent<Animator> ().SetTrigger ("flareDown");
			}
			break;
		case 2:	//gun
			if (isSelected [buttonNo] == 0) {
				if (youdidthistoher.Instance.powerUpArray [buttonNo] > 0) {
					gunButton.GetComponent<Animator> ().ResetTrigger ("gunDown");
					gunButton.GetComponent<Animator> ().SetTrigger ("gunUp");
					selectUp = 1;
					if (isSelected [4] == 1) {
						isSelected [4] = 0;
						alphaVal -= alphaStepFactor;
						youdidthistoher.Instance.powerUpArray [4]++;
						magnetButton.GetComponent<Animator> ().ResetTrigger ("magnetUp");
						magnetButton.GetComponent<Animator> ().SetTrigger ("magnetDown");
					}
				} else {
					gunButton.GetComponent<Animator>().SetTrigger("none");
					selectUp = -1;
				}
			} else {
				gunButton.GetComponent<Animator> ().ResetTrigger ("gunUp");
				gunButton.GetComponent<Animator> ().SetTrigger ("gunDown");
			}
			break;
		case 4:	//magnet
			if (isSelected [buttonNo] == 0) {
				if (youdidthistoher.Instance.powerUpArray [buttonNo] > 0) {
					magnetButton.GetComponent<Animator> ().ResetTrigger ("magnetDown");
					magnetButton.GetComponent<Animator> ().SetTrigger ("magnetUp");
					selectUp = 1;
					if (isSelected [2] == 1) {
						isSelected [2] = 0;
						alphaVal -= alphaStepFactor;
						youdidthistoher.Instance.powerUpArray [2]++;
						gunButton.GetComponent<Animator> ().ResetTrigger ("gunUp");
						gunButton.GetComponent<Animator> ().SetTrigger ("gunDown");
					}
				} else {
					magnetButton.GetComponent<Animator> ().SetTrigger ("none");
					selectUp = -1;
				}
			} else {
				magnetButton.GetComponent<Animator> ().ResetTrigger ("magnetUp");
				magnetButton.GetComponent<Animator> ().SetTrigger ("magnetDown");
			}
			break;
		case 5: //vip
			if (isSelected [buttonNo] == 0) {
				if (youdidthistoher.Instance.powerUpArray [buttonNo] > 0) {
					vipButton.GetComponent<Animator> ().ResetTrigger ("vipDown");
					vipButton.GetComponent<Animator> ().SetTrigger ("vipUp");
					selectUp = 1;
					if (isSelected [1] == 1) {
						isSelected [1] = 0;
						alphaVal -= alphaStepFactor;
						youdidthistoher.Instance.powerUpArray [1]++;
						flareButton.GetComponent<Animator> ().ResetTrigger ("flareUp");
						flareButton.GetComponent<Animator> ().SetTrigger ("flareDown");
					}
				} else {
					vipButton.GetComponent<Animator> ().SetTrigger ("none");
					selectUp = -1;
				}
			} else {
				vipButton.GetComponent<Animator> ().ResetTrigger ("vipUp");
				vipButton.GetComponent<Animator> ().SetTrigger ("vipDown");
			}
			break;
		case 3: //padlong
			if (isSelected [buttonNo] == 0) {
				if (youdidthistoher.Instance.powerUpArray [buttonNo] > 0) {
					padLongButton.GetComponent<Animator> ().ResetTrigger ("padLongDown");
					padLongButton.GetComponent<Animator> ().SetTrigger ("padLongUp");
					selectUp = 1;
				} else {
					padLongButton.GetComponent<Animator> ().SetTrigger ("none");
					selectUp = -1;
				}
			} else {
				padLongButton.GetComponent<Animator> ().ResetTrigger ("padLongUp");
				padLongButton.GetComponent<Animator> ().SetTrigger ("padLongDown");
			}
			break;
		case 0: //big ball
			if (isSelected [buttonNo] == 0) {
				if (youdidthistoher.Instance.powerUpArray [buttonNo] > 0) {
					bigBallButton.GetComponent<Animator> ().ResetTrigger ("bigBallDown");
					bigBallButton.GetComponent<Animator> ().SetTrigger ("bigBallUp");
					selectUp = 1;
				} else {
					bigBallButton.GetComponent<Animator> ().SetTrigger ("none");
					selectUp = -1;
				}
			} else {
				bigBallButton.GetComponent<Animator> ().ResetTrigger ("bigBallUp");
				bigBallButton.GetComponent<Animator> ().SetTrigger ("bigBallDown");
			}
			break;
		case 6: //multi ball
			if (isSelected [buttonNo] == 0) {
				if (youdidthistoher.Instance.powerUpArray [buttonNo] > 0) {
					multiBallButton.GetComponent<Animator> ().ResetTrigger ("multiBallDown");
					multiBallButton.GetComponent<Animator> ().SetTrigger ("multiBallUp");
					selectUp = 1;
				} else {
					multiBallButton.GetComponent<Animator> ().SetTrigger ("none");
					selectUp = -1;
				}
			} else {
				multiBallButton.GetComponent<Animator> ().ResetTrigger ("multiBallUp");
				multiBallButton.GetComponent<Animator> ().SetTrigger ("multiBallDown");
			}
			break;
		default:
			break;
		}

		if (selectUp==1) {
				isSelected [buttonNo] = 1;
				youdidthistoher.Instance.powerUpArray [buttonNo]--;
				alphaVal += alphaStepFactor;
				tempCol.a = alphaVal;
				gridLock.GetComponent<Image> ().color = tempCol;
		} else if(selectUp==0){
				isSelected [buttonNo] = 0;
				youdidthistoher.Instance.powerUpArray [buttonNo]++;
				alphaVal -= alphaStepFactor;
				tempCol.a = alphaVal;
				gridLock.GetComponent<Image> ().color = tempCol;
		}
	}
			
	private void loadPowerUpCount()
	{
		bigBallButton.transform.GetChild (1).transform.GetChild (0).GetComponent<Text> ().text = youdidthistoher.Instance.powerUpArray [0].ToString();//youdidthistoher.Instance.p_bigBall.ToString();
		flareButton.transform.GetChild (1).transform.GetChild (0).GetComponent<Text>().text = youdidthistoher.Instance.powerUpArray [1].ToString();//youdidthistoher.Instance.p_flareBall.ToString();
		gunButton.transform.GetChild (1).transform.GetChild (0).GetComponent<Text>().text = youdidthistoher.Instance.powerUpArray [2].ToString();//youdidthistoher.Instance.p_gun.ToString();
		magnetButton.transform.GetChild (1).transform.GetChild (0).GetComponent<Text>().text = youdidthistoher.Instance.powerUpArray [4].ToString();//youdidthistoher.Instance.p_magnet.ToString();
		vipButton.transform.GetChild (1).transform.GetChild (0).GetComponent<Text>().text = youdidthistoher.Instance.powerUpArray [5].ToString();//youdidthistoher.Instance.p_VIPBall.ToString();
		padLongButton.transform.GetChild (1).transform.GetChild (0).GetComponent<Text>().text = youdidthistoher.Instance.powerUpArray [3].ToString();//youdidthistoher.Instance.p_padLong.ToString();
		multiBallButton.transform.GetChild (1).transform.GetChild (0).GetComponent<Text>().text = youdidthistoher.Instance.powerUpArray [6].ToString();//youdidthistoher.Instance.p_multiBall.ToString();
	}

	private void resetPowerUps ()
	{
		bigBallButton.GetComponent<Animator>().SetTrigger("bigBallDown");
		flareButton.GetComponent<Animator>().SetTrigger("flareDown");
		gunButton.GetComponent<Animator>().SetTrigger("gunDown");
		padLongButton.GetComponent<Animator>().SetTrigger("padLongDown");
		magnetButton.GetComponent<Animator>().SetTrigger("magnetDown");
		vipButton.GetComponent<Animator>().SetTrigger("vipDown");
		multiBallButton.GetComponent<Animator>().SetTrigger("multiBallDown");
	}

	public void backGO()
	{	
		Time.timeScale = 1.0f;
		gameoverAnimationButton.GetComponent<UIAnimController>().PanelInactive();
		//PlayerPrefs.SetInt ("currentPlayingLevel", PlayerPrefs.GetInt ("currentPlayingLevel") + 1);
		youdidthistoher.Instance.currentPlayingLevel++;
		youdidthistoher.Instance.Save ();
		SceneManager.LoadScene ("Main Scene");
	}

	public void nextLevelGO()
	{	
		Time.timeScale = 1.0f;
		gameoverAnimationButton.GetComponent<UIAnimController>().PanelInactive();
		youdidthistoher.Instance.currentPlayingLevel++;
		youdidthistoher.Instance.Save ();
		SceneManager.LoadScene ("Pong_Breaker");

	}

	public void doubleCoin()
	{
	//	AdManager.Instance.ShowRewardedVideo (0);
		doubleCoins.SetActive (false);
        doubleCoins.transform.parent.GetChild(0).GetComponent<Button>().interactable = false;
        doubleCoins.transform.parent.GetChild(1).GetComponent<Button>().interactable = false;
        string s = doubleCoins.transform.parent.GetChild (0).GetComponent<Text> ().text;
		int coins = int.Parse(s);
		doubleCoins.transform.parent.GetChild (0).GetComponent<Text> ().text = (coins * 2).ToString ();
	}


	public void soundButtonController()
	{
		if (youdidthistoher.Instance.backgroundMusic == 1 || youdidthistoher.Instance.effectsSound == 1) {
			youdidthistoher.Instance.backgroundMusic = 0;
			youdidthistoher.Instance.effectsSound = 0;
			effectsSource.SetActive (false);
			themeSource.SetActive(false);
			soundButton.transform.GetChild (1).gameObject.SetActive (true);
		} else {
			youdidthistoher.Instance.backgroundMusic = 1;
			youdidthistoher.Instance.effectsSound = 1;
			effectsSource.SetActive (true);
			themeSource.SetActive(true);
			soundButton.transform.GetChild (1).gameObject.SetActive (false);
		}
		youdidthistoher.Instance.Save ();
	}

	public void comingSoonBack()
	{
		comingSoon.SetActive (false);
	}

	public void hideAd()
	{
//		AdManager.Instance.HideBanner ();
	}

	public void showAd()
	{
//		AdManager.Instance.ShowBanner ();
	}

}
