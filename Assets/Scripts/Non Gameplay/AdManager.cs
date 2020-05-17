/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;


using GoogleMobileAds;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour {

	public static AdManager Instance { set; get; }

	private BannerView bannerView;
	private InterstitialAd interstitial;
	public int bannerCount=1;


	private int RVAdType = 0; 
	private int result;
	//RVAdType==0    toMultiplyCoins
	//RVAdType==1	 toAddCoins

	private void Start(){

		Instance = this;
		DontDestroyOnLoad (gameObject);

		#if UNITY_EDITOR
//		print("Testing is on");
		#endif
		this.RequestInterstitial ();
		this.RequestBanner ();

	}

	public void ShowVideo(){
		if (Advertisement.IsReady()) {
			Advertisement.Show();
		}
		print ("Showing");

	}

	public void ShowRewardedVideo(int AdType){
		RVAdType = AdType;
		if (Advertisement.IsReady()) {
			Advertisement.Show("rewardedVideo",new ShowOptions(){resultCallback=HandleAdResult});
		}

	}



	private void HandleAdResult(ShowResult result){
		switch (result) {
		case ShowResult.Skipped:
			break;
		case ShowResult.Failed:
			break;
		case ShowResult.Finished:
			if (RVAdType == 0) {
				youdidthistoher.Instance.currency += GameObject.Find ("GameManager").GetComponent<GameManager> ().coinCount;
			} else if (RVAdType == 1) {
				youdidthistoher.Instance.currency += 30;
			}
			youdidthistoher.Instance.Save ();
			break;
		default :
			break;
		}
	}


	// Returns an ad request with custom ad targeting.
	private AdRequest CreateAdRequest()
	{

		return new AdRequest.Builder()
					.Build();
	
	}

	private void RequestBanner()
	{
		
		#if UNITY_EDITOR
		string adUnitId = "unused";
		#elif UNITY_ANDROID
		string adUnitId = "ca-app-pub-4698696782574262/9586417439";
		#else
		string adUnitId = "unexpected_platform";
		#endif

		this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

		// Load a banner ad.
		this.bannerView.LoadAd(this.CreateAdRequest());
		this.bannerView.Hide ();
	}

	private void RequestInterstitial()
	{
		#if UNITY_EDITOR
		string adUnitId = "unused";
		#elif UNITY_ANDROID
		string adUnitId = "ca-app-pub-4698696782574262/5016617032";
		#else
		string adUnitId = "unexpected_platform";
		#endif

		// Create an interstitial.
		this.interstitial = new InterstitialAd (adUnitId);

		// Load an interstitial ad.
		this.interstitial.LoadAd (this.CreateAdRequest ());
	}
	
	public void ShowInterstitial()
	{
		if (this.interstitial.IsLoaded ()) {
			this.interstitial.Show ();
		} else {
			MonoBehaviour.print ("Interstitial is not ready yet");
		}

	}

	public void ShowBanner(){
		bannerView.Show ();
	}
	
	public void HideBanner(){
		bannerView.Hide ();
	}

}
*/