using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using GoogleMobileAds.Api;

public class AdmobController : MonoBehaviour {

#if UNITY_ANDROID
    string appId = "ca-app-pub-4809397092315700~2101070495";
    string bannerId = "ca-app-pub-4809397092315700/7133905324"; //Sizes https://developers.google.com/admob/unity/banner#banner_sizes
    string interstitialId = "ca-app-pub-4809397092315700/3492498628";
    string reardVideoId = "ca-app-pub-4809397092315700/9646843432";
#elif UNITY_IPHONE
     string appId = "ca-app-pub-4809397092315700~2101070495";
    string bannerId = "ca-app-pub-4809397092315700/7133905324";
    string interstitialId = "ca-app-pub-4809397092315700/3492498628";
    string reardVideoId = "ca-app-pub-4809397092315700/9646843432";
#else
    string appId = "ca-app-pub-4809397092315700~2101070495";
    string bannerId = "ca-app-pub-4809397092315700/7133905324";
    string interstitialId = "ca-app-pub-4809397092315700/3492498628";
    string reardVideoId = "ca-app-pub-4809397092315700/9646843432";
#endif


    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardBasedVideoAd rewardGemsVideo;

    public void Start() {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

        this.RequestBanner();
        InitGemRewardVideo();
    }

    private void RequestBanner() {
        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Top);
        AdRequest request = GetTestRequest();

        // Load the banner with the request.
        bannerView.LoadAd(request);
    }


    private void RequestInterstitial() {

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(interstitialId);
        // Create an empty ad request.
        AdRequest request = GetTestRequest();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    private void RequestRewardVideo() {
        // Create an empty ad request.
        this.rewardGemsVideo = RewardBasedVideoAd.Instance;
        AdRequest request = GetTestRequest();
        // Load the rewarded video ad with the request.
        this.rewardGemsVideo.LoadAd(request, reardVideoId);
    }


    public void HandleInterstitialClosed(object sender, EventArgs args) {
        MonoBehaviour.print("HandleInterstitialClosed event received");
    }

    private static AdRequest GetTestRequest() {
        // Create an empty ad request.
        return new AdRequest.Builder()
            .AddTestDevice("9457F77F86541E4EA84BF4A7CF6D83A6")
            .Build();
    }

    //REvard!!!!

    public void ShowGemsRevardVideo() {
        if (rewardGemsVideo.IsLoaded()) {
            rewardGemsVideo.Show();
        } else {
            GameController.Gems = 404;
        }
    }

    private void InitGemRewardVideo() {
        RequestRewardVideo();
        // Get singleton reward based video ad reference
        rewardGemsVideo.OnAdRewarded += HandleRewardGemsRewarded;
        rewardGemsVideo.OnAdClosed += HandleRewardGemsClosed;
    }

    public void HandleRewardGemsRewarded(object sender, Reward args) {
        GameController.Gems += 30;

    }

    public void HandleRewardGemsClosed(object sender, EventArgs args) {
        this.RequestRewardVideo();
    }

}
