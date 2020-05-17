using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour 
{

	private const float cameraSpeed = 3.0f;
	private const int NO_OF_POWER_UPS = 7;
	private const int NO_OF_PADS = 10;
	private const int NO_OF_BLOKES = 5; 
	private const int NO_OF_EXTRAS = 10;

	//private float fingerStartTime = 0.0f;
	//private Vector2 fingerStartPos = Vector2.zero;

//	private bool isSwipe=false;
//	private float minSwipeDist = 50.0f;
//	private float maxSwipeTime = 0.5f;

	public Text description;
	public Text currencyText;
	public GameObject storeButtonPrefab;
	public GameObject storeButtonContainer;
	public GameObject coinPurchasePanel;
	public GameObject playerSample,blokeSample, powerUpCount;
	public GameObject pad, bloke, powerup, extras, specials, mastIdea, coinKhareedneWalaStore;
	public Sprite pressedPad, unpressedPad, pressedBloke, unpressedBloke ,pressedPowerUp, unpressedPowerUp, pressedSpecials, unpressedSpecials, pressedExtras, unpressedExtras;
	public Button joyStickButton,touchButton, gyroButton;
	public Sprite pressedJoystick, unpressedJoystick, pressedTouch, unpresedTouch, pressedGyro, unpressedGyro;
	public Button Dynamic, TopDown, FirstPerson, sond;
	public Sprite pressedDynamic, unpressedDynamic, pressedTopDown, unpressedTopDown, pressedFirstPerson, unpressedFirstPerson, pressedSound, unpressedSound;
	public Sprite pressedMCD, unpressedMCD, pressedDrunk, unpressedDrunk;
	public GameObject MCD, Drunk, specialPanel;
	public GameObject confirmBuy;
	public GameObject bloke1,bloke2,bloke3;
	public GameObject ground, w1, w2, w3, w4, g1, w5, w6, w7, w8;
	private string[] parsed;
	private Sprite[] thumbnails;
	private Transform cameraTransform;
	private Vector3 touchPosition;
	private Transform cameraDesiredLookAt;
	private Transform current;
	public GameObject settingResetHere, exitButton;
	public Transform store,main,about,settings,helper;
	public GameObject rating, quitSure;
	private int MOD, currentRollyElements, currentSettingRollyElement=3;
	private int curPrice;
	TextAsset txt;
	public ScrollRect scrollyRecty1, scrollyRecty2;

	private int isOnceClicked=-1;
	private int currentAvailability;
	private int currentBuyNum;
	private string storeFor="p";

	private string userName;

	private void Awake()
	{
		//	print ("1");
		string isLoginOnce  = PlayerPrefs.GetString ("_isLoginOnce");
		if(isLoginOnce !="True")
		{
			PlayerPrefs.SetString ("_isLoginOnce","True");
			PlayerPrefs.SetInt ("InputType", 1);
			//		PlayerPrefs.SetInt ("PlayerModePong",0);
		}
		parsed = new string[15];
		current = main;
		youdidthistoher.Instance.gameObject.GetComponent<AudioSource> ().enabled = true;
//		if (youdidthistoher.Instance.soundOn == 1) {
//			sond.GetComponent<Image> ().sprite = pressedSound;
//		} else {
//			sond.GetComponent<Image> ().sprite = unpressedSound;
		}
			

	private void Start()
	{	//////////
//		AdManager.Instance.HideBanner();
		/////////
		playerSample.GetComponent<Renderer> ().material = youdidthistoher.Instance.materials [youdidthistoher.Instance.currentSkinIndexPad];
		loadBlokes (youdidthistoher.Instance.currentSkinIndexBloke);
		loadExtras (youdidthistoher.Instance.currentGround);
		curPrice = youdidthistoher.Instance.padPriceDisplay;
		currentRollyElements = NO_OF_PADS;
		txt = (TextAsset)Resources.Load ("TextAssets/pad_descriptions",typeof(TextAsset));
		parser (txt.text,NO_OF_PADS+1);
		Loader (youdidthistoher.Instance.currentSkinIndexPad);
		currencyText.text = youdidthistoher.Instance.currency.ToString();
		cameraTransform = Camera.main.transform;
		pad.GetComponent<Image> ().sprite = pressedPad;
		switch (youdidthistoher.Instance.currentCameraMode) {
		case 0:		Dynamic.GetComponent<Image> ().sprite = pressedDynamic;break;
		case 1:		TopDown.GetComponent<Image> ().sprite = pressedTopDown;break;
		case 2:		FirstPerson.GetComponent<Image> ().sprite = pressedFirstPerson;break;
		}
		currentAvailability = youdidthistoher.Instance.skinAvailabilityPad;
		//	print ("yay");
		//	print (parsed.Length);
		thumbnails = youdidthistoher.Instance.pads;
		LoadIt ();
		if (PlayerPrefs.GetInt ("InputType") == 0) {
			joyStickButton.GetComponent<Image> ().sprite = pressedJoystick;
			touchButton.GetComponent<Image> ().sprite = unpresedTouch;
		} else if (PlayerPrefs.GetInt ("InputType") == 1) {
			joyStickButton.GetComponent<Image> ().sprite = unpressedJoystick;
			touchButton.GetComponent<Image> ().sprite = pressedTouch;
		}	

		if (youdidthistoher.Instance.hasRatedGame == 0) {
			if (youdidthistoher.Instance.gameOpenCount % 5 == 0) 
			{
				rating.SetActive (true);
				exitButton.SetActive (false);
			}
		}
	}

	private void Update()
	{

		if (cameraDesiredLookAt != null)
		{			cameraTransform.rotation = Quaternion.Slerp (cameraTransform.rotation, cameraDesiredLookAt.rotation, cameraSpeed * Time.deltaTime);

		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			//Application.Quit(); 
			if (current != main) {
				current = main;
				LookAtMenu (main);
			} else {
				if (quitSure.activeInHierarchy)
					ermm ();
				quitSure.SetActive (true);
			}
		}	

//			print (currentRollyElements);
		if (scrollyRecty1.transform.localPosition.x < -70 * currentRollyElements) {		 
				scrollyRecty1.transform.localPosition = new Vector3 (-70 * currentRollyElements,scrollyRecty1.transform.localPosition.y,scrollyRecty1.transform.localPosition.z);
				scrollyRecty1.StopMovement ();
			} else if (scrollyRecty1.transform.localPosition.x > 70) {	
				scrollyRecty1.transform.localPosition = new Vector3 (70,scrollyRecty1.transform.localPosition.y,scrollyRecty1.transform.localPosition.z);
				scrollyRecty1.StopMovement ();
			}

			if (scrollyRecty2.transform.localPosition.y < 10) {		
				scrollyRecty2.transform.localPosition = new Vector3 (scrollyRecty1.transform.localPosition.x,10f,scrollyRecty1.transform.localPosition.z);
				scrollyRecty2.StopMovement ();
			} else if (scrollyRecty2.transform.localPosition.y > 70 * currentSettingRollyElement) {
			scrollyRecty2.transform.localPosition = new Vector3 (scrollyRecty1.transform.localPosition.x,70 * currentSettingRollyElement,scrollyRecty1.transform.localPosition.z);
				scrollyRecty2.StopMovement ();
			}
		/*
		if (Input.touchCount == 0) 
			return;
		if (Input.touchCount > 0) {		
			foreach (Touch touch in Input.touches) {
				switch (touch.phase) {
				case TouchPhase.Began:
					//new touch
					isSwipe = true;
					fingerStartTime = Time.time;
					fingerStartPos = touch.position;
					break;
				case TouchPhase.Canceled:
					//cancelled
					isSwipe = false;
					break;
				case TouchPhase.Ended:
					float gestureTime = Time.time - fingerStartTime;
					float gestureDist = (touch.position - fingerStartPos).magnitude;

					if ((isSwipe) && (gestureTime < maxSwipeTime) && (gestureDist > minSwipeDist)) {
					
						Vector2 direction = touch.position - fingerStartPos;
						Vector2 swipeType = Vector2.zero;

						if (Mathf.Abs (direction.x) > Mathf.Abs (direction.y)) {
							//horizontal
							swipeType = Vector2.right * Mathf.Sign (direction.x);
						} else {
							//vertical
							swipeType = Vector2.up * Mathf.Sign (direction.y);
						}

						if (swipeType.x != 0.0f) {
							if (swipeType.x > 0.0f) {
								if (current == main) {
									LookAtMenu (settings);
									current = settings;
									settingResetHere.transform.localPosition = new Vector3(0f,10f,0f);
								} else if (current == store) {
									LookAtMenu (main);
									current = main;
								}
							} else {

								if (current == main) {
									LookAtMenu (store);
									current = store;
								} else if (current == settings) {
									LookAtMenu (main);
									current = main;
								}
							}
						}

						if (swipeType.y != 0.0f) {
							
								if (swipeType.y > 0.0f) {
									//up
								if (current == main) {	
									LookAtMenu (about);														
									current = about;
								} else if (current == helper) {
									LookAtMenu (main);
									current = main;
								}
							} else {
									//down
								if (current == main) {	
									LookAtMenu (helper);
									current = helper;
								} else if (current == about) {
									current = main;
									LookAtMenu (main);
								}
								}
						}
					}
					break;
				}
			}

		}*/

	}
						

	private void parser(string str, int lines)
	{
	//	for (int i = 0; i < 10; i++)
	//		parsed [i] = null;
		parsed=str.Split ("\n"[0]);
		description.text = parsed [0];
	}

	private void LoadIt()
	{
		foreach (Transform t in storeButtonContainer.transform) 
		{
			Destroy (t.gameObject);
		}

		foreach (Sprite thumbnail in thumbnails) 
		{
			GameObject container = Instantiate (storeButtonPrefab) as GameObject;
			container.GetComponent<Image> ().sprite = thumbnail;
			container.transform.SetParent (storeButtonContainer.transform,false);

			int objectNumber = int.Parse(thumbnail.name);
			container.GetComponent<Button> ().onClick.AddListener (() => Loader(objectNumber));

			container.transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = curPrice.ToString();

			if((currentAvailability & 1 << objectNumber) == 1 << objectNumber)
			{
				container.transform.GetChild (0).gameObject.SetActive (false);
			}
		}
	}

	private void Loader(int objectNumber)
	{

		switch (storeFor) 
		{
		case "p":			//print (objectNumber);
							description.text = parsed [objectNumber + 1];
							if (isOnceClicked != objectNumber) {
								//CHOOSING ITEMS
								playerSample.GetComponent<Renderer> ().material = youdidthistoher.Instance.materials [objectNumber];
				if ((youdidthistoher.Instance.skinAvailabilityPad & 1 << objectNumber) == 1 << objectNumber)
					youdidthistoher.Instance.currentSkinIndexPad = objectNumber;
								youdidthistoher.Instance.Save ();				// saves when bought
								isOnceClicked = objectNumber;
							} else {

								if ((youdidthistoher.Instance.skinAvailabilityPad & 1 << objectNumber) != 1 << objectNumber) {
									//Debug.Log (1 << objectNumber);
									//	print("chal to raha hai");
									buyingActivated (objectNumber);
								}
							}
							break;


		case "e":
			//print (objectNumber);
			description.text = parsed [objectNumber + 1];
			if (isOnceClicked != objectNumber) {
				//CHOOSING ITEMS

				if ((youdidthistoher.Instance.skinAvailabilityGround& 1 << objectNumber) == 1 << objectNumber)
				{	
				//	print ("in1");
					if (objectNumber < 4) {
					//	print ("gd1");
						youdidthistoher.Instance.currentGround = objectNumber;
					} else {
					//	print ("wl1");
//						youdidthistoher.Instance.currentWall = objectNumber;
					}	
					loadExtras (objectNumber);
					youdidthistoher.Instance.Save ();				// saves when bought
				}isOnceClicked = objectNumber;
			} else {

				if ((youdidthistoher.Instance.skinAvailabilityGround & 1 << objectNumber) != 1 << objectNumber) {
					//Debug.Log (1 << objectNumber);
					//	print("chal to raha hai");
					buyingActivated (objectNumber);
				}
			}
			break;


		case "b":
							description.text = parsed [objectNumber+1];
							//print (objectNumber);
							if (isOnceClicked != objectNumber) {
								//CHOOSING ITEMS
								blokeSample.GetComponent<Image> ().sprite = thumbnails[objectNumber];
								
				if ((youdidthistoher.Instance.skinAvailabilityBloke & 1 << objectNumber) == 1 << objectNumber) {
					youdidthistoher.Instance.currentSkinIndexBloke = objectNumber;
					youdidthistoher.Instance.Save ();
					loadBlokes (objectNumber);
				}				isOnceClicked = objectNumber;
							} else {
								if ((youdidthistoher.Instance.skinAvailabilityBloke & 1 << objectNumber) != 1 << objectNumber) {
									//Debug.Log (1 << objectNumber);
									buyingActivated(objectNumber);
								}
							}
							break;
		case "pu":			powerUpCount.GetComponent<Text>().text = "In Pocket : "+youdidthistoher.Instance.powerUpArray[objectNumber].ToString();
							description.text = parsed [objectNumber+1];
							//print (objectNumber);
							if (isOnceClicked != objectNumber) {
								//CHOOSING ITEMS
								isOnceClicked = objectNumber;
							} else {
									buyingActivated(objectNumber);
							}
							break;
		}
	}

	private void buyingActivated(int objectNumber)
	{
		confirmBuy.SetActive(true);
		currentBuyNum = objectNumber;
	}

	public void LookAtMenu(Transform menuTransform)
	{
		cameraDesiredLookAt = menuTransform;
		current = menuTransform;
		settingResetHere.transform.localPosition = new Vector3(0f,10f,0f);
	}

	public void start()
	{
		SceneManager.LoadScene ("Intermediate");
	}

	public void reset()
	{
		PlayerPrefs.DeleteAll ();
	}

	public void wapasLeAA()
	{
		pad.GetComponent<Image> ().sprite = unpressedPad;
		bloke.GetComponent<Image> ().sprite = unpressedBloke;
		powerup.GetComponent<Image> ().sprite = unpressedPowerUp;
		specials.GetComponent<Image> ().sprite = unpressedSpecials;
		extras.GetComponent<Image> ().sprite = unpressedExtras;
	}

	private void loadBlokes(int objectNumber)
	{
		bloke1.GetComponent<Renderer> ().material = youdidthistoher.Instance.blokeMaterials [objectNumber * 3];
		bloke2.GetComponent<Renderer> ().material = youdidthistoher.Instance.blokeMaterials [objectNumber * 3 + 1];
		bloke3.GetComponent<Renderer> ().material = youdidthistoher.Instance.blokeMaterials [objectNumber * 3 + 2];
	}

	private void loadExtras(int objectNumber)
	{
	/*	if (objectNumber < 4) {
			print ("gd");
			ground.GetComponent<Renderer> ().material = youdidthistoher.Instance.extraMaterials [objectNumber];
			g1.GetComponent<Renderer> ().material = youdidthistoher.Instance.extraMaterials [objectNumber];
		} else {
			print ("wl");
			w1.GetComponent<Renderer> ().material = youdidthistoher.Instance.extraMaterials [objectNumber];
			w2.GetComponent<Renderer> ().material = youdidthistoher.Instance.extraMaterials [objectNumber];
			w3.GetComponent<Renderer> ().material = youdidthistoher.Instance.extraMaterials [objectNumber];
			w4.GetComponent<Renderer> ().material = youdidthistoher.Instance.extraMaterials [objectNumber];
			w5.GetComponent<Renderer> ().material = youdidthistoher.Instance.extraMaterials [objectNumber];
			w6.GetComponent<Renderer> ().material = youdidthistoher.Instance.extraMaterials [objectNumber];
			w7.GetComponent<Renderer> ().material = youdidthistoher.Instance.extraMaterials [objectNumber];
			w8.GetComponent<Renderer> ().material = youdidthistoher.Instance.extraMaterials [objectNumber];
		}
*/	}

	public void pads()
	{
		storeButtonContainer.GetComponent<RectTransform> ().localPosition = Vector3.zero;
		isOnceClicked=-1;
		wapasLeAA ();
		pad.GetComponent<Image> ().sprite = pressedPad;
		storeFor = "p";
		curPrice = youdidthistoher.Instance.padPriceDisplay;
		currentRollyElements = NO_OF_PADS;
		sampleFalse ();
		playerSample.SetActive (true);
		thumbnails = youdidthistoher.Instance.pads;
		currentAvailability = youdidthistoher.Instance.skinAvailabilityPad;
		txt = (TextAsset)Resources.Load ("TextAssets/pad_descriptions",typeof(TextAsset));
	//	print (curPrice);
		LoadIt ();
	//	print (curPrice);
		parser (txt.text,NO_OF_PADS+1);
	}

	public void blokes()
	{
		storeButtonContainer.GetComponent<RectTransform> ().localPosition = Vector3.zero;
		isOnceClicked=-1;
		wapasLeAA ();
		bloke.GetComponent<Image> ().sprite = pressedBloke;
		storeFor = "b";
		curPrice = youdidthistoher.Instance.blokePriceDisplay;
		currentRollyElements = NO_OF_BLOKES;
		sampleFalse ();
		thumbnails = youdidthistoher.Instance.blokes;
		blokeSample.SetActive (true);
		blokeSample.GetComponent<Image> ().sprite = thumbnails[youdidthistoher.Instance.currentSkinIndexBloke];
		currentAvailability = youdidthistoher.Instance.skinAvailabilityBloke;
		txt = (TextAsset)Resources.Load ("TextAssets/bloke_descriptions",typeof(TextAsset));
	//	print (curPrice);
		LoadIt ();
	//	print (curPrice);
		parser (txt.text,NO_OF_BLOKES+1);
	}

	public void Extras()
	{
		storeButtonContainer.GetComponent<RectTransform> ().localPosition = Vector3.zero;
		isOnceClicked=-1;
		wapasLeAA ();
		extras.GetComponent<Image> ().sprite = pressedExtras;
		storeFor = "e";
		curPrice = youdidthistoher.Instance.extraPriceDisplay;
		currentRollyElements = NO_OF_EXTRAS;
		sampleFalse ();
		thumbnails = youdidthistoher.Instance.extras;
		currentAvailability = youdidthistoher.Instance.skinAvailabilityGround;
		txt = (TextAsset)Resources.Load ("TextAssets/extra_descriptions",typeof(TextAsset));
//		print (curPrice);
		LoadIt ();
	//	print (curPrice);
		parser (txt.text,NO_OF_EXTRAS+1);
	}

	public void powerups()
	{
		storeButtonContainer.GetComponent<RectTransform> ().localPosition = Vector3.zero;
		isOnceClicked=-1;
		wapasLeAA ();
		powerup.GetComponent<Image> ().sprite = pressedPowerUp;
		storeFor = "pu";
		curPrice = youdidthistoher.Instance.powerUpPriceDisplay;
		currentRollyElements = NO_OF_POWER_UPS;
		sampleFalse ();
		thumbnails = youdidthistoher.Instance.powerUps;
		powerUpCount.SetActive (true);
		txt = (TextAsset)Resources.Load ("TextAssets/powerup_descriptions",typeof(TextAsset));
		currentAvailability = 1;
		for (int i = 1; i < NO_OF_POWER_UPS; i++) {
			currentAvailability = currentAvailability >> 2;
		}
		LoadIt ();
		parser (txt.text,NO_OF_POWER_UPS+1);
	}

	public void special()
	{
		storeButtonContainer.GetComponent<RectTransform> ().localPosition = new Vector3(90,0,0);
		foreach (Transform t in storeButtonContainer.transform) {
			Destroy (t.gameObject);
		}
		storeFor = "s";
		//storeButtonContainer.transform.GetChild (0).localScale = new Vector3 (2f,2f,2f);
		wapasLeAA ();
		specials.GetComponent<Image> ().sprite = pressedSpecials;
		sampleFalse ();
		MCD.SetActive (true);
		Drunk.SetActive (true);
		if (youdidthistoher.Instance.MCDActive == 1)
			MCD.GetComponent<Image> ().sprite = pressedMCD;
		if (youdidthistoher.Instance.DrunkActive == 1)
			Drunk.GetComponent<Image> ().sprite = pressedDrunk;
		txt = (TextAsset)Resources.Load ("TextAssets/MCD_descriptions",typeof(TextAsset));
		parser (txt.text,1);
		description.text = parsed [0];
		specials.transform.GetChild (0).gameObject.SetActive (true);
	}

	private void sampleFalse ()
	{
		specials.transform.GetChild (0).gameObject.SetActive (false);
		MCD.SetActive (false);
		Drunk.SetActive (false);
		playerSample.SetActive (false);
		blokeSample.SetActive (false);
		powerUpCount.SetActive (false);
	}

	public void MCDDabaya()
	{
		if (youdidthistoher.Instance.skinAvailabilityMCD == 0) {
			MOD = 0;
			specialActive ();
		}
		else {
			youdidthistoher.Instance.MCDActive = (youdidthistoher.Instance.MCDActive==1)?0:1;
			if (youdidthistoher.Instance.MCDActive == 1) {
				MCD.GetComponent<Image> ().sprite = pressedMCD;
				Drunk.GetComponent<Image> ().sprite = unpressedDrunk;
				youdidthistoher.Instance.DrunkActive = 0;
			}
			else
				MCD.GetComponent<Image> ().sprite = unpressedMCD;
			youdidthistoher.Instance.Save ();
		}
	}

	public void DrunkDabaya()
	{
		if (youdidthistoher.Instance.skinAvailabilityDrunk == 0) {
			MOD = 1;
			specialActive ();
		}
		else {
			youdidthistoher.Instance.DrunkActive = (youdidthistoher.Instance.DrunkActive==1)?0:1;
			if (youdidthistoher.Instance.DrunkActive == 1) {
				Drunk.GetComponent<Image> ().sprite = pressedDrunk;
				MCD.GetComponent<Image> ().sprite = unpressedMCD;
				youdidthistoher.Instance.MCDActive = 0;
			}
			else
				Drunk.GetComponent<Image> ().sprite = unpressedDrunk;
			youdidthistoher.Instance.Save ();
		}
	}

	public void joystick()
	{	
		PlayerPrefs.SetInt ("InputType",0);
		joyStickButton.GetComponent<Image> ().sprite = pressedJoystick;
		touchButton.GetComponent<Image> ().sprite = unpresedTouch;
		gyroButton.GetComponent<Image> ().sprite = unpressedGyro;
	}

	public void touch()
	{
		PlayerPrefs.SetInt ("InputType",1);
		joyStickButton.GetComponent<Image> ().sprite = unpressedJoystick;
		touchButton.GetComponent<Image> ().sprite = pressedTouch;
		gyroButton.GetComponent<Image> ().sprite = unpressedGyro;
	}

	public void gyro()
	{	
		PlayerPrefs.SetInt ("InputType",2);
		joyStickButton.GetComponent<Image> ().sprite = unpressedJoystick;
		touchButton.GetComponent<Image> ().sprite = unpresedTouch;
		gyroButton.GetComponent<Image> ().sprite = pressedGyro;
	}

	public void dynamic()
	{	/*
		camerareset ();
		youdidthistoher.Instance.currentCameraMode = 0;
		youdidthistoher.Instance.Save ();
		Dynamic.GetComponent<Image> ().sprite = pressedDynamic;
*/  topdown ();
	}

	public void topdown()
	{
		camerareset ();
		youdidthistoher.Instance.currentCameraMode = 1;
		youdidthistoher.Instance.Save ();
		TopDown.GetComponent<Image> ().sprite = pressedTopDown;
	}

	public void firstperson()
	{
		camerareset ();
		youdidthistoher.Instance.currentCameraMode = 2;
		youdidthistoher.Instance.Save ();
		FirstPerson.GetComponent<Image> ().sprite = pressedFirstPerson;
	}

	private void camerareset ()
	{
		FirstPerson.GetComponent<Image> ().sprite = unpressedFirstPerson;
		TopDown.GetComponent<Image> ().sprite = unpressedTopDown;
		Dynamic.GetComponent<Image> ().sprite = unpressedDynamic;
	}

	public void confirmedBuy()
	{
		confirmBuy.SetActive (false);
		switch (storeFor) {
		case "p":
			buyPad ();
			break;
		case "b":
			buyBloke ();
			break;
		case "pu":
			buyPowerUp ();
			break;
		case "s":
			deniedBuy ();
			if (MOD == 1) {
				//buySpecialDrunk ();
//				IAPmanager.Instance.BuyDrunk ();
			} else {
				//	buySpecialMCD ();
	//			IAPmanager.Instance.BuyMCD();
			}
			break;
		case "e":
			buyExtra();
			break;
		}
	}

	public void specialActive()
	{
		specialPanel.SetActive (true);
	}

	public void specialYes()
	{
		confirmBuy.SetActive (true);
		specialPanel.SetActive (false);
	}

	public void deniedBuy()
	{
		confirmBuy.SetActive (false);
		specialPanel.SetActive (false);
	}

	private void buyPad()
	{
		int cost = 100;
		if (youdidthistoher.Instance.currency >= cost) {
			youdidthistoher.Instance.currency -= cost;
			youdidthistoher.Instance.skinAvailabilityPad += 1 << currentBuyNum;
			youdidthistoher.Instance.Save ();
			currencyText.text = youdidthistoher.Instance.currency.ToString ();
			storeButtonContainer.transform.GetChild (currentBuyNum).GetChild (0).gameObject.SetActive (false);
			youdidthistoher.Instance.currentSkinIndexPad = currentBuyNum;
			Loader (currentBuyNum);
		} else {
			//insufficient funds
			coinPurchasePanel.SetActive(true);
			mastIdea.SetActive (false);
			playerSample.SetActive (false);
		}
	}

	private void buyBloke()
	{
		int cost = 50;
		if (youdidthistoher.Instance.currency >= cost) {
			youdidthistoher.Instance.currency -= cost;
			youdidthistoher.Instance.skinAvailabilityBloke += 1 << currentBuyNum;
			youdidthistoher.Instance.Save ();
			currencyText.text = youdidthistoher.Instance.currency.ToString ();
			storeButtonContainer.transform.GetChild (currentBuyNum).GetChild (0).gameObject.SetActive (false);
			youdidthistoher.Instance.currentSkinIndexBloke = currentBuyNum;
			Loader (currentBuyNum);
			loadBlokes (currentBuyNum);
		}else {
			//insufficient funds
			coinPurchasePanel.SetActive(true);
			mastIdea.SetActive (false);
		}
	}

	private void buyExtra()
	{
		int cost = 150;
		if (youdidthistoher.Instance.currency >= cost) {
			youdidthistoher.Instance.currency -= cost;
			youdidthistoher.Instance.skinAvailabilityGround += 1 << currentBuyNum;
			if (currentBuyNum < 4)
				youdidthistoher.Instance.currentGround = currentBuyNum;
			else
//				youdidthistoher.Instance.currentWall = currentBuyNum;
				print("placed");
			youdidthistoher.Instance.Save ();
			currencyText.text = youdidthistoher.Instance.currency.ToString ();
			storeButtonContainer.transform.GetChild (currentBuyNum).GetChild (0).gameObject.SetActive (false);
			Loader (currentBuyNum);
			loadExtras (currentBuyNum);
		}else {
			//insufficient funds
			coinPurchasePanel.SetActive(true);
			mastIdea.SetActive (false);
		}
	}

	private void buyPowerUp()
	{
		int cost = 30;
		if (youdidthistoher.Instance.currency >= cost) {
			youdidthistoher.Instance.currency -= cost;
			youdidthistoher.Instance.powerUpArray[currentBuyNum] += 1;
			youdidthistoher.Instance.Save ();
			currencyText.text = youdidthistoher.Instance.currency.ToString ();
			Loader (currentBuyNum);
		}else {
			//insufficient funds
			coinPurchasePanel.SetActive(true);
			mastIdea.SetActive (false);
		}
		deniedBuy ();			//bencho bina iske chal hi nahi raha
	}

	public void buySpecialMCD()
	{
	//	int cost = 1500;
//		if (youdidthistoher.Instance.currency >= cost) {
//			youdidthistoher.Instance.currency -= cost;
			youdidthistoher.Instance.skinAvailabilityMCD += 1 << currentBuyNum;
			currencyText.text = youdidthistoher.Instance.currency.ToString ();
			MCD.GetComponent<Image> ().sprite = pressedMCD;
			Drunk.GetComponent<Image> ().sprite = unpressedDrunk;
			youdidthistoher.Instance.DrunkActive = 0;
			youdidthistoher.Instance.MCDActive = 1;
			youdidthistoher.Instance.Save ();
//		} else {
			//insufficient funds
//			coinPurchasePanel.SetActive(true);
//			MCD.SetActive (false);
//			Drunk.SetActive (false);
//			mastIdea.SetActive (false);
//		}
	}

	public void buySpecialDrunk()
	{
	//	int cost = 1500;
	//	if (youdidthistoher.Instance.currency >= cost) {
	//		youdidthistoher.Instance.currency -= cost;
			youdidthistoher.Instance.skinAvailabilityDrunk += 1 << currentBuyNum;
			currencyText.text = youdidthistoher.Instance.currency.ToString ();
			Drunk.GetComponent<Image> ().sprite = pressedDrunk;
			MCD.GetComponent<Image> ().sprite = unpressedMCD;
			youdidthistoher.Instance.DrunkActive = 1;
			youdidthistoher.Instance.MCDActive = 0;
			youdidthistoher.Instance.Save ();
	//	} else {
			//insufficient funds
	//		coinPurchasePanel.SetActive(true);
	//		MCD.SetActive (false);
	//		Drunk.SetActive (false);
	//		mastIdea.SetActive (false);
	//	}
	}


	public void nahiKhareeda()
	{
		coinPurchasePanel.SetActive (false);
		mastIdea.SetActive (true);
		if (storeFor == "s") {
			MCD.SetActive (true);
			Drunk.SetActive (true);
		} else if (storeFor == "p") {
			playerSample.SetActive (true);
		}
	}

	public void exit()
	{
		quitSure.SetActive (true);
	}
		

	public void rateKarRahaHaiInsan()
	{
		rating.SetActive (false);
		exitButton.SetActive (true);
		youdidthistoher.Instance.hasRatedGame = 1;
		//Load Here
		Application.OpenURL ("market://details?id=com.BizzareGames.PongManiac");

		//
		youdidthistoher.Instance.currency+=100;
		youdidthistoher.Instance.Save ();
	}

	public void baadMeinRateKarega()
	{
		rating.SetActive (false);
		exitButton.SetActive (true);
		youdidthistoher.Instance.gameOpenCount++;
	}

	public void kabhiNahiRateKarega()
	{
		rating.SetActive (false);
		exitButton.SetActive (true);
		youdidthistoher.Instance.hasRatedGame = 1;
	}

	public void chalja()
	{
//		AdManager.Instance.ShowVideo ();
	}

	public void purchaseCoin()
	{
		coinPurchasePanel.SetActive (false);
		coinKhareedneWalaStore.SetActive (true);
		coinKhareedneWalaStore.transform.GetChild (3).GetChild (0).GetComponent<Text> ().text = youdidthistoher.Instance.currency.ToString ();
	}

	public void inAppPurchaseIdhar1()
	{	//IAPmanager.Instance.Buy499Coins ();
		coinKhareedneWalaStore.SetActive (false);
		mastIdea.SetActive (true);
		if (storeFor == "sp") {
			MCD.SetActive (true);
			Drunk.SetActive (true);
		}	
		currencyText.text = youdidthistoher.Instance.currency.ToString ();
	}

	public void inAppPurchaseIdhar2()
	{	
		//IAPmanager.Instance.Buy999Coins ();
		coinKhareedneWalaStore.SetActive (false);
		mastIdea.SetActive (true);
		if (storeFor == "sp") {
			MCD.SetActive (true);
			Drunk.SetActive (true);
		}
		currencyText.text = youdidthistoher.Instance.currency.ToString ();
	}

	public void inAppPurchaseIdhar3()
		{	
		//IAPmanager.Instance.Buy2999Coins ();
		coinKhareedneWalaStore.SetActive (false);
		mastIdea.SetActive (true);
		if (storeFor == "sp") {
			MCD.SetActive (true);
			Drunk.SetActive (true);
		}
		currencyText.text = youdidthistoher.Instance.currency.ToString ();
	}

	public void watchAdForCoin()
	{
		//AdManager.Instance.ShowRewardedVideo (1);
		coinPurchasePanel.SetActive (false);
		mastIdea.SetActive (true);
		currencyText.text = youdidthistoher.Instance.currency.ToString ();
	}

	public void purchaseBack()
	{
		coinKhareedneWalaStore.SetActive (false);
		mastIdea.SetActive (true);
		if (storeFor == "p")
			playerSample.SetActive (true);
		else if (storeFor == "b")
			blokeSample.SetActive (true);
	}

	public void coinButton()
	{
		coinKhareedneWalaStore.SetActive (true);
		playerSample.SetActive (false);
		coinKhareedneWalaStore.transform.GetChild (3).GetChild (0).GetComponent<Text> ().text = youdidthistoher.Instance.currency.ToString ();
	}

	public void sondBajega()
	{
//		youdidthistoher.Instance.MenuSound ();
//		if (youdidthistoher.Instance.soundOn == 1)
//			sond.GetComponent<Image> ().sprite = pressedSound;
//		else
//			sond.GetComponent<Image> ().sprite = unpressedSound;
	}


	public void haha()
	{
		quitSure.SetActive (false);
	}

	public void ermm()
	{
		Application.Quit(); 
	}
}