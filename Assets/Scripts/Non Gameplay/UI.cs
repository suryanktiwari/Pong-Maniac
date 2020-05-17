using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

	private const int NO_OF_ROWS = 4;
	private const int NO_OF_COLUMNS = 5;
	private const int NO_OF_PAGES = 20;
	private const int POWERUP_COUNT = 7;
	private const int GROUND_COUNT = 5;
	private const int BLOKES_COUNT = 5;
	private const int SPECIAL_COUNT = 4;
	private const int PAD_COUNT = 8;
	private const float gameSelectionWaitTime = 3.5f;
	private const float purchaseWaitTime = 1.5f;
	private const int maxDescriptionSizeParsed=15;
	private int MCD_COST=600;
	private int DRUNK_COST=600;
	private int GROUND_COST=youdidthistoher.Instance.extraPriceDisplay;
	private int BLOCK_COST=youdidthistoher.Instance.blokePriceDisplay;
	private int PAD_COST=youdidthistoher.Instance.padPriceDisplay;
	private int POWERUP_COST=youdidthistoher.Instance.powerUpPriceDisplay;
	private const int RATE_US_FREQUENCY = 4;
	private const float RATE_US_DELAY = 0f;

	private Vector3 checkMarkHomePos = new Vector3 (80f, 70f, 0f);

	private bool blockMovement,firstBlock, levelSelected, purchaseModeOn;
	private int levelCount, levelsUnlocked;
	private int lastSwitch=1;
	private int currentActiveStore=0;
	private int[] boughtInStores =  new int[5];
	private int[] selectedInStores = new int[5];
	private GameObject[] currentCheck = new GameObject[5];

	private string[] parsed, helpParsed;

	private TextAsset txt, helpTxt;

	public Text  descriptionText, itemDescriptionText, countText;

	public GameObject mainPanel, gameSelectionPanel, levelSelectionPanel, settingsPanel, aboutPanel, helpPanel, shopPanel, patt, ratingAnimatable, gameSelectionAnimatable, mainAnimatable, aboutSound, notConnectedPanel;
    public GameObject privacyPanel;
    public GameObject LevelPage, LevelButton, LevelRow, StoreButton, helpTitleText ,helpDescriptionText, helpImage, helpPowerUp, helpBlocks, helpOthers, ratingPanel, coinPanel, firstShopVisit;
	public GameObject GroundsPanel, BlokesPanel, PowerupsPanel, PadsPanel, SpecialPanel;
	public GameObject soundButton, cameraButton, effectsButton;
	public GameObject practiceButton, endlessButton, campaignButton, forwardCampaignButton, backwardCampaignButton, mcdButton, drunkButton;
	public GameObject bckGameSelection, bckLevelSelection;			//back buttons that need to be disabled
	public GameObject confirmBuy, shopMoneyAmount, intermediateMoneyAmount, coinPurchaseActivator;
	public GameObject checkMarkBlock, checkMarkGround, checkMarkPad;

	public Sprite[] powerUpSprites = new Sprite[POWERUP_COUNT];
	public Sprite[] groundSprites = new Sprite[GROUND_COUNT];
	public Sprite[] blokeSprites = new Sprite[BLOKES_COUNT];
	public Sprite[] padSprites = new Sprite[PAD_COUNT];
	public Sprite[] helpSprites;

	public AudioSource themeSource, effectsSource;
	public AudioClip loadingSoundEffect;

	private GameObject[] row = new GameObject[NO_OF_ROWS];
	private GameObject[,] levels = new GameObject[NO_OF_ROWS,NO_OF_COLUMNS];
	private GameObject prevButton;
	private int prevItemNo, curHelpSel, GameType;
	private bool canAskForRating=true, selected=false;

	void Awake () 
	{
		helpTxt = (TextAsset)Resources.Load ("Text/help",typeof(TextAsset));
		string content = helpTxt.text;
		helpParsed = content.Split ('\n');
		if (youdidthistoher.Instance.helpOn) {
			mainPanel.SetActive (false);
			helpPanel.SetActive (true);
			hPowerup ();
	//		AdManager.Instance.HideBanner ();
		}
		else if (youdidthistoher.Instance.startGame == true) {
			mainPanel.SetActive (false);
			gameSelectionPanel.SetActive (true);
			showAd ();
		}
		Time.timeScale = 1f;
		string isLoginOnce  = PlayerPrefs.GetString ("_isLoginOnce");
		if (isLoginOnce != "True") {
			PlayerPrefs.SetString ("_isLoginOnce", "True");
			cameraButtonController (2);	//fps button active
		} else {
			cameraButtonController (youdidthistoher.Instance.currentCameraMode);
		}
		parsed = new string[maxDescriptionSizeParsed];			
		firstBlock=levelSelected=false;
		levelsUnlocked = youdidthistoher.Instance.campaignLevelReached;
		levelCount = (NO_OF_ROWS*NO_OF_COLUMNS)*(levelsUnlocked/(NO_OF_ROWS*NO_OF_COLUMNS))+1;
//		print (levelCount);
		makeAPage (LevelPage.transform.position);									//Campaign levels
		if (levelsUnlocked % 20 == 0) {
			previousPageLevel ();
			blockMovement = true;
		}



		if (youdidthistoher.Instance.backgroundMusic == 0) {
			soundButton.transform.GetChild (1).gameObject.SetActive (true);
			themeSource.enabled = false;
		}

		if(youdidthistoher.Instance.effectsSound==0){
			effectsButton.transform.GetChild (1).gameObject.SetActive (true);
			effectsSource.enabled = false;
		}

		boughtInStores[1]= youdidthistoher.Instance.skinAvailabilityPad;
		boughtInStores[3]= youdidthistoher.Instance.skinAvailabilityBloke;
		boughtInStores[4]= youdidthistoher.Instance.skinAvailabilityGround;
		currentCheck [1] = checkMarkPad;
		currentCheck [3] = checkMarkBlock;
		currentCheck [4] = checkMarkGround;
		selectedInStores[1] = youdidthistoher.Instance.currentSkinIndexPad;
		selectedInStores[3] = youdidthistoher.Instance.currentSkinIndexBloke;
		selectedInStores[4] = youdidthistoher.Instance.currentGround;
		currentActiveStore = 4;
		shopInstantiator ("Text/grounds",GROUND_COUNT,GroundsPanel,groundSprites);
		currentActiveStore = 3;
		shopInstantiator ("Text/blokes",BLOKES_COUNT,BlokesPanel,blokeSprites);
		currentActiveStore = 1;
		shopInstantiator ("Text/pads",PAD_COUNT,PadsPanel,padSprites);
		currentActiveStore = 0;
		shopInstantiator ("Text/powerups",POWERUP_COUNT,PowerupsPanel,powerUpSprites);

	}

	void Start()
	{
		shopStarter ();
        if (!PlayerPrefs.HasKey("privacyPolicy"))
        {
            privacyPanel.SetActive(true);
        }
        /*
                if (youdidthistoher.Instance.hasRatedGame == 0) {
                    if (youdidthistoher.Instance.gameOpenCount % RATE_US_FREQUENCY == 0) 
                    {
                        Invoke ("rateUs", RATE_US_DELAY);
                    }
                }
        */
    }

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (notConnectedPanel.activeInHierarchy) {
				closeNotConnectedPanel ();
			}
			else if (mainPanel.activeInHierarchy) {
				Application.Quit ();
			}
			else if (coinPanel.activeInHierarchy) {
				coinPanel.SetActive (false);
				shopPanel.SetActive (false);
				gameSelectionPanel.SetActive (false);
				mainAnimatable.GetComponent<UIAnimController> ().PanelActive ();
			}
			else if (levelSelectionPanel.activeInHierarchy) {
				levelSelectionPanel.SetActive (false);
				gameSelectionAnimatable.GetComponent<UIAnimController> ().PanelActive ();
	//			AdManager.Instance.ShowBanner ();
			} else if (gameSelectionPanel.activeInHierarchy) {
				gameSelectionAnimatable.GetComponent<UIAnimController> ().PanelInactive ();
				mainAnimatable.GetComponent<UIAnimController> ().PanelActive ();
			} else if (helpPanel.activeInHierarchy) {
				helpPanel.SetActive (false);
				mainAnimatable.GetComponent<UIAnimController> ().PanelActive ();
			} else if (shopPanel.activeInHierarchy) {
				shopPanel.SetActive (false);
				mainAnimatable.GetComponent<UIAnimController> ().PanelActive ();
			}
			else if (aboutPanel.activeInHierarchy) {
				aboutPanel.SetActive (false);
				mainAnimatable.GetComponent<UIAnimController> ().PanelActive ();
			}
			else if (settingsPanel.activeInHierarchy) {
				settingsPanel.SetActive (false);
				mainAnimatable.GetComponent<UIAnimController> ().PanelActive ();
			}
			else if (ratingPanel.activeInHierarchy) {
				ratingPanel.SetActive (false);
				mainAnimatable.GetComponent<UIAnimController> ().PanelActive ();
			}


		}
	}


    public void PrivacyPolicy()
    {
        Application.OpenURL("https://esteev.github.io/privacy_policy_pm.html");
    }

    public void UnityPrivacyPolicy()
    {
        Application.OpenURL("https://unity3d.com/legal/privacy-policy");
    }

    public void AcceptPrivacyPolicy()
    {
        PlayerPrefs.SetInt("privacyPolicy", 1);
        privacyPanel.SetActive(false);
    }

    int price(int storeActive)
	{
		switch (storeActive) 
		{
		case 0:
			return POWERUP_COST;
		case 1:
			return PAD_COST;
		case 3:
			return BLOCK_COST;
		case 4:
			return GROUND_COST;
		default:
			return -1;
		}
	}

	void shopInstantiator(string address, int count, GameObject parentGameObject, Sprite[] images)
	{
		txt = (TextAsset)Resources.Load (address,typeof(TextAsset));
		parser (txt.text);
//		RectTransform rowRectTransform = StoreButton.GetComponent<RectTransform>();
//		RectTransform containerRectTransform = parentGameObject.GetComponent<RectTransform>();

//		float width = containerRectTransform.rect.width / NO_OF_COLUMNS;
//		float ratio = width / rowRectTransform.rect.width;
//		float height = rowRectTransform.rect.height * ratio;
		int i = 1;
		for (int j = 0; j < count; j++) {
			GameObject temp = GameObject.Instantiate (StoreButton, parentGameObject.transform.position, Quaternion.identity);
			temp.transform.position = parentGameObject.transform.position;
			temp.transform.SetParent (parentGameObject.transform.GetChild(0).transform);
			temp.GetComponent<Image> ().sprite = images [j];
		//	print (j+"  "+images [j]);
			int tempItemNo = j;
			temp.gameObject.GetComponent<Button> ().onClick.AddListener (() => StoreItem (temp, tempItemNo));
			if ((boughtInStores [currentActiveStore] & 1 << tempItemNo) == 1 << tempItemNo && currentActiveStore != 0) {
				//if already purchased
				temp.transform.GetChild (0).gameObject.SetActive (false);
				if (tempItemNo == selectedInStores [currentActiveStore]) {

					currentCheck[currentActiveStore].transform.SetParent (temp.transform);
					currentCheck[currentActiveStore].transform.localPosition = checkMarkHomePos;
				}
			} 
			else {
				temp.transform.GetChild (0).GetComponent<Text> ().text = price(currentActiveStore).ToString();		//enter prices here
			}
		
/*
			float offsetRatio = 0.1f;
			RectTransform rectTransform = temp.GetComponent<RectTransform> ();

			float x = -containerRectTransform.rect.width / 2 + width * (j) + width * offsetRatio;
			float y = containerRectTransform.rect.height / 2 - height * (i + 1) + height * offsetRatio;
			rectTransform.offsetMin = new Vector2 (x, y);

			x = rectTransform.offsetMin.x + width - 2 * width * offsetRatio;
			y = rectTransform.offsetMin.y + height - 2 * height * offsetRatio;
			rectTransform.offsetMax = new Vector2 (x, y);

			rectTransform.localScale = new Vector3 (1f, 1f, 1f);
*/		}
	}

	private void parser(string str)
	{
		//	for (int i = 0; i < 10; i++)
		//		parsed [i] = null;
		parsed=str.Split ("\n"[0]);
		descriptionText.text = parsed [0];
		itemDescriptionText.text = parsed[1];
	}

	private void StoreItem(int itemNo)
	{
	//	print (itemNo+" "+(1 << itemNo));
		descriptionText.text = parsed [itemNo + 1];
	}

	private void StoreItem(GameObject button, int itemNo)
	{
		if (button == prevButton && purchaseModeOn &&!selected) {
			//purchase happens

			switch (currentActiveStore) {
			case 0:
				//powerups
				if (youdidthistoher.Instance.currency >= POWERUP_COST) {
					confirmBuy.transform.parent.GetComponent<Animation> ().Play ("boughtSuccessfully");
					youdidthistoher.Instance.currency -= POWERUP_COST;
					intermediateMoneyAmount.GetComponent<Text>().text = shopMoneyAmount.GetComponent<Text> ().text = youdidthistoher.Instance.currency.ToString ();
					youdidthistoher.Instance.powerUpArray [itemNo] += 1;
					youdidthistoher.Instance.Save ();
					countText.text = "In Pocket: " + youdidthistoher.Instance.powerUpArray [itemNo];
				} else {
					//insufficient funds
					coinPurchaseActivator.GetComponent<UIAnimController>().PanelActive();
				}
				break;
			case 1:
				//pads	
				if (youdidthistoher.Instance.currency >= PAD_COST && (youdidthistoher.Instance.skinAvailabilityPad & 1 << itemNo) != 1 << itemNo) {
					confirmBuy.transform.parent.GetComponent<Animation> ().Play ("boughtSuccessfully");
					currentCheck[currentActiveStore].transform.SetParent (button.transform);
					currentCheck[currentActiveStore].transform.localPosition = checkMarkHomePos;
					youdidthistoher.Instance.currency -= PAD_COST;
					intermediateMoneyAmount.GetComponent<Text>().text = shopMoneyAmount.GetComponent<Text> ().text = youdidthistoher.Instance.currency.ToString();
					youdidthistoher.Instance.skinAvailabilityPad += 1 << itemNo;
					youdidthistoher.Instance.currentSkinIndexPad = itemNo;
					youdidthistoher.Instance.Save ();
					button.transform.GetChild (0).gameObject.SetActive (false);
				} else {	
					//insufficient funds
					coinPurchaseActivator.GetComponent<UIAnimController>().PanelActive();
				}
				break;
			case 2:
				//specials
				//separate functions written
				break;
			case 3:
				//blokes
				if (youdidthistoher.Instance.currency >= BLOCK_COST && (youdidthistoher.Instance.skinAvailabilityBloke & 1 << itemNo) != 1 << itemNo) {
					confirmBuy.transform.parent.GetComponent<Animation> ().Play ("boughtSuccessfully");
					currentCheck[currentActiveStore].transform.SetParent (button.transform);
					currentCheck[currentActiveStore].transform.localPosition = checkMarkHomePos;
					youdidthistoher.Instance.currency -= BLOCK_COST;
					intermediateMoneyAmount.GetComponent<Text>().text = shopMoneyAmount.GetComponent<Text> ().text = youdidthistoher.Instance.currency.ToString();
					youdidthistoher.Instance.skinAvailabilityBloke += 1 << itemNo;
					youdidthistoher.Instance.currentSkinIndexBloke = itemNo;
					youdidthistoher.Instance.Save ();
					print("bought");
					button.transform.GetChild (0).gameObject.SetActive (false);
				}else {
					//insufficient funds
					coinPurchaseActivator.GetComponent<UIAnimController>().PanelActive();
				}
				break;
			case 4:
				//grounds
				if (youdidthistoher.Instance.currency >= GROUND_COST && (youdidthistoher.Instance.skinAvailabilityGround & 1 << itemNo) != 1 << itemNo) {
					confirmBuy.transform.parent.GetComponent<Animation> ().Play ("boughtSuccessfully");
					currentCheck[currentActiveStore].transform.SetParent (button.transform);
					currentCheck[currentActiveStore].transform.localPosition = checkMarkHomePos;
					youdidthistoher.Instance.currency -= GROUND_COST;
					intermediateMoneyAmount.GetComponent<Text>().text = shopMoneyAmount.GetComponent<Text> ().text = youdidthistoher.Instance.currency.ToString();
					youdidthistoher.Instance.skinAvailabilityGround += 1 << itemNo;
					youdidthistoher.Instance.currentGround = itemNo;
					youdidthistoher.Instance.Save ();
					button.transform.GetChild (0).gameObject.SetActive (false);
				}else {
					//insufficient funds
					coinPurchaseActivator.GetComponent<UIAnimController>().PanelActive();
				}
				break;
			default:
				break;
			}

		} 


		else {
			//fist click, confirmation needed or selection
			prevButton = button;
			purchaseModeOn = true;
			selected = false;
			Invoke ("purchaseReset", purchaseWaitTime);
//			print (itemNo + " " + (1 << itemNo)+" "+button);
			itemDescriptionText.text = parsed [itemNo + 2];
			switch (currentActiveStore) {
			case 0:
				//powerups
				countText.text = "In Pocket: "+youdidthistoher.Instance.powerUpArray[itemNo];
				confirmBuy.transform.parent.GetComponent<Animation> ().Play ("confirmBuy");
				break;
			case 1:
				//pads
				if ((youdidthistoher.Instance.skinAvailabilityPad & 1 << itemNo) == 1 << itemNo) {
					//runs to select the already bought materials
					youdidthistoher.Instance.currentSkinIndexPad = itemNo;
					currentCheck[currentActiveStore].transform.SetParent (button.transform);
					currentCheck[currentActiveStore].transform.localPosition = checkMarkHomePos;
					selected = true;
					print (itemNo + " selected");
					youdidthistoher.Instance.Save ();			
				} else {
					confirmBuy.transform.parent.GetComponent<Animation> ().Play ("confirmBuy");
				}
				break;
			case 2:
				//specials
				//separate functions written
				break;
			case 3:
				//blokes
				if ((youdidthistoher.Instance.skinAvailabilityBloke & 1 << itemNo) == 1 << itemNo) {
					//runs to select the already bought materials
					youdidthistoher.Instance.currentSkinIndexBloke = itemNo;
					currentCheck[currentActiveStore].transform.SetParent (button.transform);
					currentCheck[currentActiveStore].transform.localPosition = checkMarkHomePos;
					selected = true;
					youdidthistoher.Instance.Save ();			
				} else {
					confirmBuy.transform.parent.GetComponent<Animation> ().Play ("confirmBuy");
				}
				break;
			case 4:
				//grounds
				if ((youdidthistoher.Instance.skinAvailabilityGround & 1 << itemNo) == 1 << itemNo) {
					//runs to select the already bought materials
					youdidthistoher.Instance.currentGround = itemNo;
					currentCheck[currentActiveStore].transform.SetParent (button.transform);
					currentCheck[currentActiveStore].transform.localPosition = checkMarkHomePos;
					selected = true;
					youdidthistoher.Instance.Save ();			
				}else {
					confirmBuy.transform.parent.GetComponent<Animation> ().Play ("confirmBuy");
				}
				break;
			default:
				break;
			}
		}
	}


	private void LoadMenu(int level)
	{
		if (!levelSelected) 
		{
//			print ("level "+level);

			int row = (level%(NO_OF_ROWS*NO_OF_COLUMNS))/NO_OF_COLUMNS;
			if (row % NO_OF_COLUMNS == 0 && row != 0) {														//row correction
				row--;
			}
			int column = (level%(NO_OF_ROWS*NO_OF_COLUMNS))%NO_OF_COLUMNS;
			if (levelsUnlocked < ((levelCount / (NO_OF_ROWS * NO_OF_COLUMNS)) + 1) * (NO_OF_ROWS * NO_OF_COLUMNS)) {		//currentLevelPage Correction of One block ahead selection
				youdidthistoher.Instance.currentPlayingLevel = level;
				column--;
				if (column == -1) {
					column = NO_OF_COLUMNS - 1;
					row--;
					if (row == -1) {
						row = NO_OF_ROWS - 1;
					}
				}
			} else {
				youdidthistoher.Instance.currentPlayingLevel = level + 1;											//load current level
			}
//			print ("row "+row+" column "+column);
			GameObject thisButton = levels [row, column];
			if (thisButton.transform.GetChild (2).gameObject.activeInHierarchy) {
				//locked Button Click
				thisButton.GetComponent<Animation> ().Play ("lockedButtonCampaign");					//Locked Anim
			} else {
				//level Selected
				thisButton.GetComponent<Animation> ().Play ("levelButtonSelect");						//ButtonSelectAnim
				bckLevelSelection.GetComponent<Button> ().interactable = false;
				levelSelected = true;
//				print ("here");
				Invoke ("loadScene", 0.5f);
			}
		}
	}

    void makeAPage(Vector3 parentOfRows)
    {

        RectTransform rowRectTransform = LevelButton.GetComponent<RectTransform>();
        RectTransform containerRectTransform = LevelPage.GetComponent<RectTransform>();

        float width = containerRectTransform.rect.width / NO_OF_COLUMNS;
        float ratio = width / rowRectTransform.rect.width;
        float height = rowRectTransform.rect.height * ratio;

        for (int i = 0; i < NO_OF_ROWS; i++)
        {
            for (int j = 0; j < NO_OF_COLUMNS; j++)
            {
                levels[i, j] = Instantiate(LevelButton) as GameObject;
                levels[i, j].name = LevelPage.name + " item at (" + i + "," + j + ")";
                //levels[i, j].transform.parent = LevelPage.transform;
				levels [i, j].transform.SetParent (LevelPage.transform);

                levels[i, j].transform.GetChild(1).GetComponent<Text>().text = levelCount++.ToString();
                int temp = levelCount - 1;
                levels[i, j].GetComponent<Button>().onClick.AddListener(() => LoadMenu(temp));

                if (levelCount <= levelsUnlocked+1)
                {
                    levels[i, j].transform.GetChild(2).gameObject.SetActive(false);
                    blockMovement = false;

                }
                else
                {
                    levels[i, j].transform.GetChild(2).gameObject.SetActive(true);
                    blockMovement = true;
                }
                if (levelCount - 1 == levelsUnlocked)
                {                                                           //select component
                    levels[i, j].transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    levels[i, j].transform.GetChild(0).gameObject.SetActive(false);
                }

                float offsetRatio = 0.1f;
                RectTransform rectTransform = levels[i, j].GetComponent<RectTransform>();

                float x = -containerRectTransform.rect.width / 2 + width * (j) + width * offsetRatio;
                float y = containerRectTransform.rect.height / 2 - height * (i + 1) + height * offsetRatio;
                rectTransform.offsetMin = new Vector2(x, y);

                x = rectTransform.offsetMin.x + width - 2 * width * offsetRatio;
                y = rectTransform.offsetMin.y + height - 2 * height * offsetRatio;
                rectTransform.offsetMax = new Vector2(x, y);

                rectTransform.localScale = new Vector3(1f, 1f, 1f);

                levels[i, j].transform.GetChild(2).GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f) * ((1 - 2 * offsetRatio));
            }

        }
    
    }

    public void start() {

		if (youdidthistoher.Instance.hasRatedGame == 0 && youdidthistoher.Instance.gameOpenCount % RATE_US_FREQUENCY == 0 && canAskForRating) {
			{
				canAskForRating = false;
				Invoke ("rateUs", RATE_US_DELAY);
			}
		} else {
			showAd ();
			mainAnimatable.GetComponent<UIAnimController> ().PanelInactive ();
			gameSelectionAnimatable.GetComponent<UIAnimController> ().PanelActive ();
		}
	}

    public void Campaign()
    {
        youdidthistoher.Instance.gameplayType = 0;
    }

    public void Endless()
    {
		themeSource.enabled = false;
		Invoke ("playLoadingSound", 0.15f);
		campaignButton.GetComponent<Button> ().interactable = false;
		practiceButton.GetComponent<Button> ().interactable = false;
		endlessButton.GetComponent<Button> ().interactable = false;
		bckGameSelection.GetComponent<Button> ().interactable = false;
        youdidthistoher.Instance.gameplayType = 1;
		practiceButton.transform.GetChild(1).gameObject.GetComponent<ButtonBackRotator>().enabled = false;
		campaignButton.transform.GetChild(1).gameObject.GetComponent<ButtonBackRotator>().enabled = false;
		endlessButton.transform.GetChild (1).gameObject.GetComponent<ButtonBackRotator> ().rateIncrease();
		endlessButton.GetComponent<Animation> ().Play("endless");
		campaignButton.GetComponent<Animation> ().Play ("campaignRight");
		practiceButton.GetComponent<Animation> ().Play ("practiceRight");
		Invoke ("loadScene", gameSelectionWaitTime);
    }

    public void Practice()
    {
		themeSource.enabled = false;
		Invoke ("playLoadingSound", 0.15f);
		campaignButton.GetComponent<Button> ().interactable = false;
		endlessButton.GetComponent<Button> ().interactable = false;
		practiceButton.GetComponent<Button> ().interactable = false;
		bckGameSelection.GetComponent<Button> ().interactable = false;
		youdidthistoher.Instance.gameplayType = 2;
		campaignButton.transform.GetChild(1).gameObject.GetComponent<ButtonBackRotator>().enabled = false;
		endlessButton.transform.GetChild(1).gameObject.GetComponent<ButtonBackRotator>().enabled = false;
		practiceButton.transform.GetChild (1).gameObject.GetComponent<ButtonBackRotator> ().rateIncrease();
		practiceButton.GetComponent<Animation> ().Play("practice");
		campaignButton.GetComponent<Animation> ().Play ("campaignLeft");
		endlessButton.GetComponent<Animation> ().Play ("endlessLeft");
		Invoke ("loadScene", gameSelectionWaitTime);
    }

	void playLoadingSound()
	{
		effectsSource.PlayOneShot (loadingSoundEffect);
	}

	void loadScene()
	{
		youdidthistoher.Instance.startGame = true;
		SceneManager.LoadScene("Pong_Breaker");
	}

	public void quit()
	{
		Application.Quit ();
	}

	public void nextPageLevel()
	{
		if (!blockMovement) {
			if (lastSwitch == -1)
				levelCount += 20;
			lastSwitch = 1;
			for (int i = 0; i < NO_OF_ROWS; i++) {
				for (int j = 0; j < NO_OF_COLUMNS; j++) {
					levels [i, j].transform.GetChild (1).GetComponent<Text> ().text = levelCount++.ToString ();
					if (levelCount-1 == levelsUnlocked) {															//select component
						levels [i, j].transform.GetChild (0).gameObject.SetActive (true);
					} else {
						levels [i, j].transform.GetChild (0).gameObject.SetActive (false);
					}
					levels [i, j].GetComponent<Button> ().onClick.RemoveAllListeners ();
					int temp = levelCount - 1;
					levels [i, j].GetComponent<Button> ().onClick.AddListener (() => LoadMenu (temp));
					if (levelCount<=levelsUnlocked) {
						levels [i, j].transform.GetChild (2).gameObject.SetActive (false);
						blockMovement = false;
					} 
					else {
						if (levelCount-1 == levelsUnlocked)
							levels [i, j].transform.GetChild (2).gameObject.SetActive (false);	
						else
							levels [i, j].transform.GetChild (2).gameObject.SetActive (true);
						blockMovement = true;
					}
				}
			}
		}
	}

	public void previousPageLevel()
	{
		if (levelCount>NO_OF_ROWS*NO_OF_COLUMNS && youdidthistoher.Instance.campaignLevelReached>=20) {
				blockMovement = false;
				if (lastSwitch == 1)
					levelCount -= NO_OF_ROWS*NO_OF_COLUMNS;
				lastSwitch = -1;
				for (int i = NO_OF_ROWS - 1; i >= 0; i--) {
					for (int j = NO_OF_COLUMNS - 1; j >= 0; j--) {	
						levels [i, j].transform.GetChild (1).GetComponent<Text> ().text = (--levelCount).ToString ();
						if (levelCount == levelsUnlocked) {															//select component
							levels [i, j].transform.GetChild (0).gameObject.SetActive (true);
						} else {
							levels [i, j].transform.GetChild (0).gameObject.SetActive (false);
						}
						levels [i, j].GetComponent<Button> ().onClick.RemoveAllListeners ();
						int temp = levelCount - 1;
						levels [i, j].GetComponent<Button> ().onClick.AddListener (() => LoadMenu (temp));

						if (levelCount <= levelsUnlocked) {
							levels [i, j].transform.GetChild (2).gameObject.SetActive (false);
						} else {
							levels [i, j].transform.GetChild (2).gameObject.SetActive (true);
						}
					}
				}
			}
	}

	void resetShop()
	{
		PowerupsPanel.SetActive (false);
		PadsPanel.SetActive (false);
		SpecialPanel.SetActive (false);
		BlokesPanel.SetActive (false);
		GroundsPanel.SetActive (false);
	}

	public void shopStarter()
	{
		powerUps ();
		intermediateMoneyAmount.GetComponent<Text>().text = shopMoneyAmount.GetComponent<Text> ().text = youdidthistoher.Instance.currency.ToString();
	}

	public void powerUps()
	{
		resetShop ();
		PowerupsPanel.SetActive (true);
		countText.gameObject.SetActive (true);
		currentActiveStore = 0;
		txt = (TextAsset)Resources.Load ("Text/powerups",typeof(TextAsset));
		parser (txt.text);
	}

	public void pads()
	{
		resetShop ();
		PadsPanel.SetActive (true);
		countText.gameObject.SetActive (false);
		currentActiveStore = 1;
		txt = (TextAsset)Resources.Load ("Text/pads",typeof(TextAsset));
		parser (txt.text);
	}

	public void specials()
	{
		resetShop ();
		SpecialPanel.SetActive (true);
		countText.gameObject.SetActive (false);
		currentActiveStore = 2;
		if (youdidthistoher.Instance.MCDActive == 1) {
			mcdButton.transform.GetChild (0).gameObject.SetActive (true);
			mcdButton.transform.GetChild (1).gameObject.SetActive (false);
		}
		if (youdidthistoher.Instance.DrunkActive == 1) {
			drunkButton.transform.GetChild (0).gameObject.SetActive (true);
			drunkButton.transform.GetChild (1).gameObject.SetActive (false);
		}
		txt = (TextAsset)Resources.Load ("Text/specials",typeof(TextAsset));
		parser (txt.text);
	}

	public void blokes()
	{
		resetShop ();
		BlokesPanel.SetActive (true);
		countText.gameObject.SetActive (false);
		currentActiveStore = 3;
		txt = (TextAsset)Resources.Load ("Text/blokes",typeof(TextAsset));
		parser (txt.text);
	}

	public void grounds()
	{
		resetShop ();
		GroundsPanel.SetActive (true);
		countText.gameObject.SetActive (false);
		currentActiveStore = 4;
		txt = (TextAsset)Resources.Load ("Text/grounds",typeof(TextAsset));
		parser (txt.text);
	}

	public void OpenWebsite()
	{
		Debug.Log ("hello");
		Application.OpenURL ("http://www.bizarregamestudios.com");
	}

	public void OpenFacebook()
	{
		Application.OpenURL ("https://www.facebook.com/bizarregamestudios");
	}

	public void OpenInsta()
	{
		Application.OpenURL ("https://www.instagram.com/bizarregamestudios");
	}

	public void OpenPlaystore()
	{
		Application.OpenURL ("https://play.google.com/store/apps/details?id=com.BizzareGames.PongManiac&hl=en");
	}

	void resetCameraButtons()
	{
		for (int i = 0; i < 3; i++) 
		{
			cameraButton.transform.GetChild (i).transform.GetChild(0).gameObject.SetActive (false);
		}
	}

	public void cameraButtonController(int param)
	{
		// 0 = dynamic, 1= topdown, 2= fps
		resetCameraButtons ();
		cameraButton.transform.GetChild (param).transform.GetChild (0).gameObject.SetActive (true);
		youdidthistoher.Instance.currentCameraMode = param;
		youdidthistoher.Instance.Save ();
	//	print (param);
	}

	public void MCD()
	{
		if (youdidthistoher.Instance.skinAvailabilityMCD == 1) {
			if (youdidthistoher.Instance.MCDActive == 0) {
				youdidthistoher.Instance.MCDActive = 1;
				mcdButton.transform.GetChild (0).gameObject.SetActive (true);
				mcdButton.transform.GetChild (1).gameObject.SetActive (false);
				youdidthistoher.Instance.DrunkActive = 0;
				drunkButton.transform.GetChild (0).gameObject.SetActive (false);
				drunkButton.transform.GetChild (1).gameObject.SetActive (true);
			} else {
				youdidthistoher.Instance.MCDActive = 0;
				mcdButton.transform.GetChild (0).gameObject.SetActive (false);
				mcdButton.transform.GetChild (1).gameObject.SetActive (true);
			}
			youdidthistoher.Instance.Save ();
		} else {
			if (purchaseModeOn && prevButton == mcdButton) {
				if (youdidthistoher.Instance.campaignLevelReached > 20) {
					if (youdidthistoher.Instance.currency >= MCD_COST) {
						confirmBuy.transform.parent.GetComponent<Animation> ().Play ("boughtSuccessfully");
						youdidthistoher.Instance.currency -= MCD_COST;
						intermediateMoneyAmount.GetComponent<Text> ().text = shopMoneyAmount.GetComponent<Text> ().text = youdidthistoher.Instance.currency.ToString ();
						youdidthistoher.Instance.skinAvailabilityMCD = 1;
						youdidthistoher.Instance.MCDActive = 1;
						mcdButton.transform.GetChild (0).gameObject.SetActive (true);
						mcdButton.transform.GetChild (1).gameObject.SetActive (false);
						youdidthistoher.Instance.DrunkActive = 0;
						drunkButton.transform.GetChild (0).gameObject.SetActive (false);
						drunkButton.transform.GetChild (1).gameObject.SetActive (true);
						youdidthistoher.Instance.Save ();
	//					Social.ReportProgress (GPGSIds.achievement_marine_cruiser_destroyer_inbound, 100.0f, (bool success) => {
	//					});		
					} else {
						//insufficient funds
						coinPurchaseActivator.GetComponent<UIAnimController> ().PanelActive ();
					}
				} else {
					confirmBuy.transform.parent.GetComponent<Animation> ().Play ("notBefore20");
				}
			}
			else {
				prevButton = mcdButton;
				purchaseModeOn = true;
				Invoke ("purchaseReset", purchaseWaitTime);
				itemDescriptionText.text = parsed [3];
				confirmBuy.transform.parent.GetComponent<Animation> ().Play ("confirmBuy");
			}
		}
	}

	public void drunk()
	{
		if (youdidthistoher.Instance.skinAvailabilityDrunk == 1) {
			if (youdidthistoher.Instance.DrunkActive == 0) {
				youdidthistoher.Instance.DrunkActive = 1;
				drunkButton.transform.GetChild (0).gameObject.SetActive (true);
				drunkButton.transform.GetChild (1).gameObject.SetActive (false);
				youdidthistoher.Instance.MCDActive = 0;
				mcdButton.transform.GetChild (0).gameObject.SetActive (false);
				mcdButton.transform.GetChild (1).gameObject.SetActive (true);
			} else {
				youdidthistoher.Instance.DrunkActive = 0;
				drunkButton.transform.GetChild (0).gameObject.SetActive (false);
				drunkButton.transform.GetChild (1).gameObject.SetActive (true);
			}
			youdidthistoher.Instance.Save ();
		} else {
			if (purchaseModeOn && prevButton == drunkButton) {
				if (youdidthistoher.Instance.campaignLevelReached > 20) {
					if (youdidthistoher.Instance.currency >= DRUNK_COST) {
						confirmBuy.transform.parent.GetComponent<Animation> ().Play ("boughtSuccessfully");
						youdidthistoher.Instance.currency -= DRUNK_COST;
						intermediateMoneyAmount.GetComponent<Text> ().text = shopMoneyAmount.GetComponent<Text> ().text = youdidthistoher.Instance.currency.ToString ();
						youdidthistoher.Instance.skinAvailabilityDrunk = 1;
						youdidthistoher.Instance.DrunkActive = 1;
						drunkButton.transform.GetChild (0).gameObject.SetActive (true);
						drunkButton.transform.GetChild (1).gameObject.SetActive (false);
						youdidthistoher.Instance.MCDActive = 0;
						mcdButton.transform.GetChild (0).gameObject.SetActive (false);
						mcdButton.transform.GetChild (1).gameObject.SetActive (true);
						youdidthistoher.Instance.Save ();
	//					Social.ReportProgress (GPGSIds.achievement_drunk, 100.0f, (bool success) => {
	//					});		
					} else {
						//insufficient funds
						coinPurchaseActivator.GetComponent<UIAnimController> ().PanelActive ();
					}
				} else {
					confirmBuy.transform.parent.GetComponent<Animation> ().Play ("notBefore20");
				}
			} else {
				prevButton = drunkButton;
				purchaseModeOn = true;
				Invoke ("purchaseReset", purchaseWaitTime);
				itemDescriptionText.text = parsed [2];
				confirmBuy.transform.parent.GetComponent<Animation> ().Play ("confirmBuy");
			}
		}
	}

	private void purchaseReset()
	{
		purchaseModeOn = false;
		prevButton = null;
	}

	public void watchAdForMoney()
	{
		// 30 coins
	//	AdManager.Instance.ShowRewardedVideo (1);
		intermediateMoneyAmount.GetComponent<Text>().text = shopMoneyAmount.GetComponent<Text> ().text = youdidthistoher.Instance.currency.ToString ();
	}

	public void purchase1()
	{
		//INR 29.99-499 Coins
//		IAPmanager.Instance.Buy499Coins ();

		intermediateMoneyAmount.GetComponent<Text>().text = shopMoneyAmount.GetComponent<Text> ().text = youdidthistoher.Instance.currency.ToString ();
	}

	public void purchase2()
	{
		//INR 49.99-999 Coins

//		IAPmanager.Instance.Buy999Coins ();

		intermediateMoneyAmount.GetComponent<Text>().text = shopMoneyAmount.GetComponent<Text> ().text = youdidthistoher.Instance.currency.ToString ();
	}

	public void purchase3()
	{
		//INR 99.99-2999 Coins
//		IAPmanager.Instance.Buy2999Coins ();
	
		intermediateMoneyAmount.GetComponent<Text>().text = shopMoneyAmount.GetComponent<Text> ().text = youdidthistoher.Instance.currency.ToString ();
	}

	void rateUs()
	{
		ratingAnimatable.GetComponent<UIAnimController> ().PanelActive ();
		ratingAnimatable.GetComponent<UIAnimController> ().panel.GetComponent<Animation> ().Play ();
		ratingAnimatable.transform.parent.GetChild (0).GetComponent<UIAnimController> ().PanelInactive();
	}

	public void takeMeThere()
	{
		ratingAnimatable.transform.parent.GetChild (0).GetComponent<UIAnimController> ().PanelActive();
		ratingAnimatable.GetComponent<UIAnimController> ().PanelInactive ();
		youdidthistoher.Instance.hasRatedGame = 1;
		Application.OpenURL ("market://details?id=com.BizzareGames.PongManiac");
		youdidthistoher.Instance.currency+=100;
		youdidthistoher.Instance.Save ();
	}

	public void notRightNow()
	{
		ratingAnimatable.transform.parent.GetChild (0).GetComponent<UIAnimController> ().PanelActive();
		ratingAnimatable.GetComponent<UIAnimController> ().PanelInactive ();
	}

	public void dontAskAgain()
	{
		ratingAnimatable.transform.parent.GetChild (0).GetComponent<UIAnimController> ().PanelActive();
		ratingAnimatable.GetComponent<UIAnimController> ().PanelInactive ();
		youdidthistoher.Instance.hasRatedGame = 1;
		youdidthistoher.Instance.Save ();
	}

	public void soundButtonController()
	{
		if (youdidthistoher.Instance.backgroundMusic == 1) {
			youdidthistoher.Instance.backgroundMusic = 0;
			themeSource.enabled = false;
			soundButton.transform.GetChild (1).gameObject.SetActive (true);
		} else {
			youdidthistoher.Instance.backgroundMusic = 1;
			themeSource.enabled = true;
			soundButton.transform.GetChild (1).gameObject.SetActive (false);
		}
		youdidthistoher.Instance.Save ();
	}

	public void effectsButtonController()
	{
		if (youdidthistoher.Instance.effectsSound == 1) {
			youdidthistoher.Instance.effectsSound = 0;
			effectsSource.enabled = false;
			effectsButton.transform.GetChild (1).gameObject.SetActive (true);
		} else {
			youdidthistoher.Instance.effectsSound = 1;
			effectsSource.enabled = true;
			effectsButton.transform.GetChild (1).gameObject.SetActive (false);
		}
		youdidthistoher.Instance.Save ();
	}

	public void aboutEnable()
	{
		if (youdidthistoher.Instance.backgroundMusic == 1) {
			themeSource.Stop ();
			aboutSound.GetComponent<AudioSource>().enabled = (true);
		}
	}

	public void aboutDisable()
	{
		if (youdidthistoher.Instance.backgroundMusic == 1) {
			aboutSound.GetComponent<AudioSource>().enabled = (false);
			themeSource.Play ();
		}
		aboutPanel.SetActive (false);
	}

	public void hideAd()
	{
	//	AdManager.Instance.HideBanner ();
	}

	public void showAd()
	{
	//	AdManager.Instance.ShowBanner ();
	}

	void helpReset()
	{
		helpPowerUp.SetActive (false);
		helpBlocks.SetActive (false);
		helpOthers.SetActive (false);
	}

	public void hPowerup()
	{
		helpReset ();
		helpPowerUp.SetActive (true);
		//selecting big ball
		helpParse (0);
		levelSpecifier (4);
		setGameType (0);
	}

	public void hBlocks()
	{
		helpReset ();
		helpBlocks.SetActive (true);
		//selecting basic block 1
		helpParse (10);
		levelSpecifier (1);
		setGameType (0);
	}

	public void hOthers()
	{
		helpReset ();
		helpOthers.SetActive (true);
		//selecting big ball
		helpParse (17);
		levelSpecifier (1);
		setGameType (0);
	}

	public void helpParse(int no)
	{
		string[] s = helpParsed [no].Split ('*');
		helpTitleText.GetComponent<Text> ().text = s [0];
		helpDescriptionText.GetComponent<Text> ().text = s[1];
		helpImage.GetComponent<Image>().sprite = helpSprites [no];
	}

	public void levelSpecifier(int tutData)
	{
		curHelpSel = tutData;
	}

	public void setGameType(int type)
	{
		GameType = type;
	}

	public void tutorial()
	{
		youdidthistoher.Instance.helpOn = true;
		youdidthistoher.Instance.tutorialOnly = true;
		switch (GameType) {
		case 0:
			youdidthistoher.Instance.forceTutorialLevel = curHelpSel;
			youdidthistoher.Instance.currentPlayingLevel = curHelpSel;
			youdidthistoher.Instance.gameplayType = 0;
			break;
		case 1:
			youdidthistoher.Instance.firstEndlessPlay = 1;
			youdidthistoher.Instance.gameplayType = 1;
			break;
		case 2:
			youdidthistoher.Instance.firstDarkPlay = 1;
			youdidthistoher.Instance.gameplayType = 2;
			break;
		case 3:
			youdidthistoher.Instance.currentPlayingLevel = curHelpSel;
			youdidthistoher.Instance.currentPlayingLevel = 21;
			youdidthistoher.Instance.gameplayType = 0;
			youdidthistoher.Instance.MCDActive = 1;
			youdidthistoher.Instance.firstMCDPlay = 1;
			break;
		case 4:
			youdidthistoher.Instance.currentPlayingLevel = curHelpSel;
			youdidthistoher.Instance.currentPlayingLevel = 21;
			youdidthistoher.Instance.gameplayType = 0;
			youdidthistoher.Instance.DrunkActive = 1;
			youdidthistoher.Instance.firstDrunkPlay = 1;
			break;
		}
		SceneManager.LoadScene ("Pong_Breaker");
	}

	public void firstShopVisitChecker()
	{
		if (youdidthistoher.Instance.firstShopVisit == 1) {
			firstShopVisit.SetActive (true);
		}
	}

	public void firstShopVisitCompleted()
	{
		if (youdidthistoher.Instance.firstShopVisit == 1) {
			firstShopVisit.SetActive (false);
			youdidthistoher.Instance.firstShopVisit = 0;
			youdidthistoher.Instance.Save ();
		}
	}

	public void achievements()
	{
		if (Social.localUser.authenticated)
			youdidthistoher.ShowAchievementsUI ();
		else
			notConnectedPanel.SetActive (true);
	}

	public void leaderboard()
	{
		if (Social.localUser.authenticated)
			youdidthistoher.ShowLeaderboardsUI ();
		else
			notConnectedPanel.SetActive (true);
	}

	public void closeNotConnectedPanel()
	{
		notConnectedPanel.SetActive (false);
	}

	public void signIn()
	{
		youdidthistoher.Instance.SignIn ();
		notConnectedPanel.SetActive (false);
	}
}
