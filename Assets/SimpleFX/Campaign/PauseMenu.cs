using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	private string aww = "No power-ups in the special modes";
	private const float ballForce = 400.0f;
	private const int noOfPowerUps = 7;
	public GameObject pauseMenu;
	public GameObject panel;
	public GameObject levelButtonPrefab;
	public GameObject levelButtonContainer;
	public GameObject confirmScreen;
	public GameObject powerUpScreen;
	public GameObject noPowerupText;
	public Button soundButt,osoundButt;
	public bool paused = false;
	public GameObject a;
	private GameObject[] containers;
	private int[] isSelected;

	void Start () {
		if (youdidthistoher.Instance.backgroundMusic == 1) {
		//	soundButt.GetComponent<Image> ().sprite = youdidthistoher.Instance.pressedSound;

		} else {
		//	soundButt.GetComponent<Image> ().sprite = youdidthistoher.Instance.unpressedSound;
			
		}
		if (youdidthistoher.Instance.effectsSound == 1) {
		//	osoundButt.GetComponent<Image> ().sprite = youdidthistoher.Instance.pressedoSound;
			a.SetActive (true);
		} else {
	//		osoundButt.GetComponent<Image> ().sprite = youdidthistoher.Instance.unpressedoSound;
			a.SetActive (false);
		}
			containers = new GameObject[noOfPowerUps];
			isSelected = new int[noOfPowerUps];
		if (youdidthistoher.Instance.MCDActive == 0 && youdidthistoher.Instance.DrunkActive == 0) {
			LoadKardeChal ();
		} else {
			noPowerupText.SetActive (true);
			noPowerupText.transform.GetChild(0).gameObject.GetComponent<Text> ().text = aww;
		}
			pauseMenu.SetActive (false);
			panel.SetActive (true);
			if (PlayerPrefs.GetInt ("InputType")==1||PlayerPrefs.GetInt ("InputType")==2)
			confirmScreen.SetActive(false);	
	}
		

	void LoadKardeChal()
	{
		Sprite[] thumbnails = Resources.LoadAll<Sprite> ("Power");
		int i = 0;
		foreach (Transform t in levelButtonContainer.transform) 
		{
			Destroy (t.gameObject);
		}
		noPowerupText.SetActive (true);
		foreach (Sprite thumbnail in thumbnails) 
		{
			if (youdidthistoher.Instance.powerUpArray [i] != 0) 
			{
				noPowerupText.SetActive (false);
				containers [i] = Instantiate (levelButtonPrefab) as GameObject;
				containers [i].GetComponent<Image> ().sprite = thumbnail;
				containers [i].transform.SetParent (levelButtonContainer.transform, false);
				string LevelName = thumbnail.name;
				containers [i++].GetComponent<Button> ().onClick.AddListener (() => LoadMenu (LevelName));
			} else {
				i++;
			}
		}
	}

	private void LoadMenu(string LevelName)
	{
		int temp = int.Parse (LevelName);
		//print (LevelName);
		if (isSelected [temp] == 0) {
			containers [temp].transform.position += Vector3.up * 50.0f;
			isSelected [temp] = 1;
			youdidthistoher.Instance.powerUpArray [temp]--;
			//Resolving clash

			switch (temp) {
			case 1:		//flare
				if (isSelected [5] == 1) {
					isSelected [5] = 0;
					containers [5].transform.position += Vector3.down * 50.0f;
					youdidthistoher.Instance.powerUpArray [5]++;
				}
				break;
			case 2:	//gun
				if (isSelected [4] == 1) {
					isSelected [4] = 0;
					containers [4].transform.position += Vector3.down * 50.0f;
					youdidthistoher.Instance.powerUpArray [4]++;
				}
				break;
			case 4:	//magnet
				if (isSelected [2] == 1) {
					isSelected [2] = 0;
					containers [2].transform.position += Vector3.down * 50.0f;
					youdidthistoher.Instance.powerUpArray [2]++;
				}
				break;
			case 5: //vip
				if (isSelected [1] == 1) {
					isSelected [1] = 0;
					containers [1].transform.position += Vector3.down * 50.0f;
					youdidthistoher.Instance.powerUpArray [1]++;
				}
				break;
			default:
				break;
			}
		} else {
			containers [temp].transform.position += Vector3.down * 50.0f;
			isSelected [temp] = 0;
			youdidthistoher.Instance.powerUpArray [temp]++;
		}
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (paused == false) {
				pause ();
			} else {
				if (confirmScreen.activeInHierarchy)
					actionConfirmed ();
				else
					Back ();
			}
		}
	}

	public void unpause()
	{	///////
//		AdManager.Instance.HideBanner();
		///////
		paused = false;
		pauseMenu.SetActive (false);
		panel.SetActive (true);
		if (PlayerPrefs.GetInt ("InputType")==1||PlayerPrefs.GetInt ("InputType")==2)
			//joystick.SetActive (false);
		Time.timeScale = 1.0f;

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
			}
		}
		youdidthistoher.Instance.Save ();
	}

	public void pause()
	{
		////////
		//AdManager.Instance.ShowBanner();
		////////
		LoadKardeChal ();
		paused = true;
		pauseMenu.SetActive (true);
		panel.SetActive (false);
		Time.timeScale = 0.0f;
		for (int i = 0; i < noOfPowerUps; i++)
				isSelected [i] = 0;
	}

	public void Back()
	{
		Time.timeScale = 0.0f;
		//are you sure
		pauseMenu.SetActive(true);
		pauseMenu.transform.GetChild(0).gameObject.SetActive(false);
		pauseMenu.transform.GetChild(1).gameObject.SetActive(false);
		confirmScreen.SetActive(true);
		powerUpScreen.SetActive (false);
		panel.SetActive(true);
	}

	public void actionConfirmed()
	{
		Time.timeScale = 1.0f;
		confirmScreen.SetActive(false);
		panel.SetActive (true);
			for (int i = 0; i < noOfPowerUps; i++) {
				if (isSelected [i] == 1)
					youdidthistoher.Instance.powerUpArray [i]++;
			}
			youdidthistoher.Instance.Save ();
		SceneManager.LoadScene ("Intermediate");
	}

	public void actionDenied()
	{
		Time.timeScale = 1.0f;
		confirmScreen.SetActive (false);
		panel.SetActive (true);
		pauseMenu.transform.GetChild(0).gameObject.SetActive(true);
		pauseMenu.transform.GetChild(1).gameObject.SetActive(true);
		pauseMenu.SetActive (false);
			for (int i = 0; i < noOfPowerUps; i++) {
				if (isSelected [i] == 1)
					youdidthistoher.Instance.powerUpArray [i]++;
			}
			youdidthistoher.Instance.Save ();
		powerUpScreen.SetActive (true);
	}



}
