using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;

public class youdidthistoher : MonoBehaviour {

	private const int NO_OF_MATERIALS = 8;
	private const int NO_OF_MATERIALS_BLOKE = 15;
    private static youdidthistoher instance;
    public static youdidthistoher Instance { get; private set; }

    public Material[] materials; 
	public Material[] blokeMaterials;
	public Sprite[] pads;
	public Sprite[] blokes;
	public Sprite[] extras;
	public Sprite[] powerUps;

	public int currentSkinIndexPad=0;
	public int currentSkinIndexBloke = 0;
	public int currentGround = 0;
	public int currency=400;
	public int p_bigBall = 5;
	public int p_flareBall = 5;
	public int p_gun = 5;
	public int p_padLong = 5;
	public int p_magnet = 5;
	public int p_VIPBall = 5;
	public int p_multiBall = 5;
	public int skinAvailabilityPad=1;
	public int skinAvailabilityBloke=1;
	public int skinAvailabilityGround=1;
	public int MCDActive=0;
	public int DrunkActive=0;
	public int skinAvailabilityMCD = 0;
	public int skinAvailabilityDrunk = 0;
	public int hasRatedGame = 0;
	public int gameOpenCount = 1;
	public int currentPlayingLevel = 1;
	public int HighScoreEndless = 0;
	public int HighScoreDark = 0;
	public int LevelLooseCount=1;
	public int backgroundMusic = 1;
	public int effectsSound =1;
	public int gameplayType = 0;

	public int padPriceDisplay = 200;
	public int blokePriceDisplay = 150;
	public int extraPriceDisplay = 300;
	public int powerUpPriceDisplay = 30;

	public int currentCameraMode=2;		//0 for dynamic, 2 for first person
	public int[] powerUpArray;
	public int campaignLevelReached=1;
	public int totalNoOfLevels = 251;
	public string[] level;

	public int tutorialLevel = 1;
	public int forceTutorialLevel = 0;
	public int firstEndlessPlay = 1;
	public int firstDarkPlay = 1;
	public int firstMCDPlay = 1;
	public int firstDrunkPlay = 1;
	public int firstShopVisit = 1;
//	public int tutorialFinished = 0;

	public bool startGame = false, helpOn=false, tutorialOnly=false;

	void Awake () {
		Instance = this;
		DontDestroyOnLoad(gameObject);

		materials = new Material[NO_OF_MATERIALS]; 
		blokeMaterials = new Material[NO_OF_MATERIALS_BLOKE];
		powerUpArray = new int[7];
	//	PlayerPrefs.DeleteAll ();
		materials = Resources.LoadAll<Material> ("Material/Pad_Materials");
		blokeMaterials = Resources.LoadAll<Material> ("Material/Bloke_Materials");
		TextAsset txt = (TextAsset)Resources.Load ("LevelStore", typeof(TextAsset));
		string content = txt.text;
		level = content.Split ('\n');
		if (PlayerPrefs.HasKey ("Currency")) {
			//We had a previous session
			currentSkinIndexPad = PlayerPrefs.GetInt("CurrentSkinPad");
			currentCameraMode = PlayerPrefs.GetInt ("currentCameraMode");
			currentSkinIndexBloke = PlayerPrefs.GetInt("CurrentSkinBloke");
			currency = PlayerPrefs.GetInt ("Currency");
			currentGround = PlayerPrefs.GetInt ("currentGround");
			skinAvailabilityPad = PlayerPrefs.GetInt ("SkinAvailabilityPad");
			skinAvailabilityBloke = PlayerPrefs.GetInt ("SkinAvailabilityBloke");
			skinAvailabilityGround = PlayerPrefs.GetInt ("SkinAvailabilityGround");
			skinAvailabilityMCD = PlayerPrefs.GetInt ("SkinAvailabilityMCD");
			skinAvailabilityDrunk = PlayerPrefs.GetInt ("SkinAvailabilityDrunk");
			MCDActive = PlayerPrefs.GetInt ("MCDActive");
			DrunkActive = PlayerPrefs.GetInt ("DrunkActive");
			backgroundMusic = PlayerPrefs.GetInt ("backgroundMusic");
			effectsSound = PlayerPrefs.GetInt ("effectsSound");
			hasRatedGame = PlayerPrefs.GetInt ("hasRatedGame");
			gameOpenCount = PlayerPrefs.GetInt ("gameOpenCount");
			LevelLooseCount = PlayerPrefs.GetInt ("levelLooseCount");
			powerUpArray [0] = PlayerPrefs.GetInt ("p_bigBall");
			powerUpArray [1] = PlayerPrefs.GetInt ("p_flareBall");
			powerUpArray [2] = PlayerPrefs.GetInt ("p_gun");
			powerUpArray [3] = PlayerPrefs.GetInt ("p_padLong");
			powerUpArray [4] = PlayerPrefs.GetInt ("p_magnet");
			powerUpArray [5] = PlayerPrefs.GetInt ("p_VIPBall");
			powerUpArray [6] = PlayerPrefs.GetInt ("p_multiBall");
			currentPlayingLevel = PlayerPrefs.GetInt ("currentPlayingLevel");
			campaignLevelReached = PlayerPrefs.GetInt ("campaignLevelReached");
			HighScoreEndless = PlayerPrefs.GetInt ("HighScoreEndless");
			HighScoreDark = PlayerPrefs.GetInt ("HighScoreDark");
			gameplayType = PlayerPrefs.GetInt ("gameplayType");
			tutorialLevel = PlayerPrefs.GetInt ("tutorialLevel");
			firstEndlessPlay = PlayerPrefs.GetInt ("firstEndlessPlay");
			firstDarkPlay = PlayerPrefs.GetInt ("firstDarkPlay");
			firstMCDPlay = PlayerPrefs.GetInt ("firstMCDPlay");
			firstDrunkPlay = PlayerPrefs.GetInt ("firstDrunkPlay");
			firstShopVisit = PlayerPrefs.GetInt ("firstShopVisit");
	//		tutorialFinished = PlayerPrefs.GetInt ("tutorialFinished");
			gameOpenCount++;

				
			if (!(PlayerPrefs.HasKey ("levelsUpdated"))) {
				PlayerPrefs.DeleteAll ();							// game reset in second update if already on some phone, 10 active devices
				firstRun();
			}

			if(!(PlayerPrefs.HasKey("easingLevel")))
			{
				PlayerPrefs.DeleteAll ();
				firstRun();
			}

            if(!PlayerPrefs.HasKey("thirdLevelChange"))
            {
                PlayerPrefs.DeleteAll();
                firstRun();
            }

		} else{
			//pehli baar chalega bencho
			firstRun();
		}

	//	PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
	//	PlayGamesPlatform.InitializeInstance(config);
	//	PlayGamesPlatform.Activate();
	//	SignIn();

	}
		
	void firstRun()
	{
		for(int i=0;i<7;i++)
			powerUpArray[i]=4;
		Save();
		PlayerPrefs.SetInt ("levelsUpdated", 1);
		PlayerPrefs.SetInt ("easingLevel", 1);
        PlayerPrefs.SetInt("thirdLevelChange", 1);
        for (int i = 1; i<=level.Length; i++) {
			PlayerPrefs.SetString ("level" + i, level [i - 1]);
		}
	}

	public void Save()
	{
		PlayerPrefs.SetInt ("CurrentSkinPad", currentSkinIndexPad);
		PlayerPrefs.SetInt ("CurrentSkinBloke", currentSkinIndexBloke);
		PlayerPrefs.SetInt ("Currency", currency);
		PlayerPrefs.SetInt ("currentGround", currentGround);
		PlayerPrefs.SetInt ("currentCameraMode", currentCameraMode);
		PlayerPrefs.SetInt ("SkinAvailabilityPad", skinAvailabilityPad);
		PlayerPrefs.SetInt ("SkinAvailabilityBloke", skinAvailabilityBloke);
		PlayerPrefs.SetInt ("SkinAvailabilityGround", skinAvailabilityGround);
		PlayerPrefs.SetInt ("SkinAvailabilityMCD", skinAvailabilityMCD);
		PlayerPrefs.SetInt ("SkinAvailabilityDrunk", skinAvailabilityDrunk);
		PlayerPrefs.SetInt ("p_bigBall", powerUpArray [0]);
		PlayerPrefs.SetInt ("p_flareBall", powerUpArray [1]);
		PlayerPrefs.SetInt ("p_gun", powerUpArray [2]);
		PlayerPrefs.SetInt ("p_padLong", powerUpArray [3]);
		PlayerPrefs.SetInt ("p_magnet", powerUpArray [4]);
		PlayerPrefs.SetInt ("p_VIPBall", powerUpArray [5]);
		PlayerPrefs.SetInt ("p_multiBall", powerUpArray [6]);
		PlayerPrefs.SetInt ("MCDActive", MCDActive);
		PlayerPrefs.SetInt ("DrunkActive", DrunkActive);
		PlayerPrefs.SetInt ("backgroundMusic", backgroundMusic);
		PlayerPrefs.SetInt ("effectsSound", effectsSound);
		PlayerPrefs.SetInt ("HighScoreEndless", HighScoreEndless);
		PlayerPrefs.SetInt ("HighScoreDark", HighScoreDark);
		PlayerPrefs.SetInt ("hasRatedGame", hasRatedGame);
		PlayerPrefs.SetInt ("gameOpenCount", gameOpenCount);
		PlayerPrefs.SetInt ("levelLooseCount", LevelLooseCount);
		PlayerPrefs.SetInt ("currentPlayingLevel", currentPlayingLevel);
		PlayerPrefs.SetInt ("campaignLevelReached", campaignLevelReached);
		PlayerPrefs.SetInt ("gameplayType", gameplayType);
		PlayerPrefs.SetInt ("tutorialLevel", tutorialLevel);
		PlayerPrefs.SetInt ("firstEndlessPlay", firstEndlessPlay);
		PlayerPrefs.SetInt ("firstDarkPlay", firstDarkPlay);
		PlayerPrefs.SetInt ("firstMCDPlay", firstMCDPlay);
		PlayerPrefs.SetInt ("firstDrunkPlay", firstDrunkPlay);
		PlayerPrefs.SetInt ("firstShopVisit", firstShopVisit);
	//	PlayerPrefs.SetInt ("tutorialFinished", tutorialFinished);

	//	print ("currentPlayingLevel "+currentPlayingLevel);
	//	print ("campaignLevelReached "+campaignLevelReached);
	}

	public void SignIn()
	{
		if (!Social.localUser.authenticated) {
			Social.localUser.Authenticate ((bool success) => {

				if (success) {
					Debug.Log (Social.localUser.userName);
				} else
					Debug.Log ("LOL");
			});
		}
	}

	#region Achievements
	public static void UnlockAchievement(string id)
	{
		Social.ReportProgress(id, 100, success => { });
	}

	public static void IncrementAchievement(string id, int stepsToIncrement)
	{
//		PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncrement, success => { });
	}

	public static void ShowAchievementsUI()
	{
		Social.ShowAchievementsUI();
	}
	#endregion /Achievements

	#region Leaderboards
	public static void AddScoreToLeaderboard(string leaderboardId, long score)
	{
		Social.ReportScore(score, leaderboardId, success => { });
	}

	public static void ShowLeaderboardsUI()
	{
		Social.ShowLeaderboardUI();
	}
	#endregion /Leaderboards

/*
	public void backgroundSound()
	{
		if (youdidthistoher.Instance.backgroundMusic == 1) {
			AudioListener.volume = 0f;
			backgroundMusic = 0;
		} else {
			AudioListener.volume = 1f;
			backgroundMusic = 1;
		}
		Save ();
	}
*/
}