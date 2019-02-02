using UnityEngine;
using System;
using GoogleMobileAds.Api;

public class AdmobController : SceneSingleton<AdmobController> {

#if UNITY_ANDROID
    string appId = "ca-app-pub-4809397092315700~2101070495";
    string bannerId = "ca-app-pub-4809397092315700/7133905324"; //Sizes https://developers.google.com/admob/unity/banner#banner_sizes
    string interstitialId = "ca-app-pub-4809397092315700/3492498628";
    string revardVideoId = "ca-app-pub-4809397092315700/9646843432";
    string boxRevardVideoId = "ca-app-pub-4809397092315700/9646843432";
#elif UNITY_IPHONE
     string appId = "ca-app-pub-4809397092315700~2101070495";
    string bannerId = "ca-app-pub-4809397092315700/7133905324";
    string interstitialId = "ca-app-pub-4809397092315700/3492498628";
    string revardVideoId = "ca-app-pub-4809397092315700/9646843432";
     string boxRevardVideoId = "ca-app-pub-4809397092315700/9646843432";
#else
    string appId = "ca-app-pub-4809397092315700~2101070495";
    string bannerId = "ca-app-pub-4809397092315700/7133905324";
    string interstitialId = "ca-app-pub-4809397092315700/3492498628";
    string revardVideoId = "ca-app-pub-4809397092315700/9646843432";
     string boxRevardVideoId = "ca-app-pub-4809397092315700/9646843432";
#endif


    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardBasedVideoAd rewardGemsVideo;
    private RewardBasedVideoAd rewardBoxVideo;

    public void Start() {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

        this.RequestBanner();
        
        InitGemRewardVideo();
        RequestInterstitial();
        bannerView.Show();
    }

    private static AdRequest GetTestRequest() {
        // Create an empty ad request.
        return new AdRequest.Builder()
            .AddTestDevice("9457F77F86541E4EA84BF4A7CF6D83A6")
            .Build();
    }

    private void RequestBanner() {
        bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Top);
        AdRequest request = GetTestRequest();
        bannerView.LoadAd(request);
    }

    #region Interstitial
    private void RequestInterstitial() {
        this.interstitial = new InterstitialAd(interstitialId);
        AdRequest request = GetTestRequest();
        this.interstitial.LoadAd(request);
    }

    public void ShowIterstitial() {
        if (this.interstitial.IsLoaded()) {
            this.interstitial.Show();
        }
    }
    public void HandleInterstitialClosed(object sender, EventArgs args) {
        MonoBehaviour.print("HandleInterstitialClosed event received");
    }
    #endregion

    #region revards

    private void RequestGemsRewardVideo() {
        // Create an empty ad request.
        this.rewardGemsVideo = RewardBasedVideoAd.Instance;
        AdRequest request = GetTestRequest();
        // Load the rewarded video ad with the request.
        this.rewardGemsVideo.LoadAd(request, revardVideoId);
    }

    public void ShowGemsRevardVideo() {
        if (rewardGemsVideo.IsLoaded()) {
            rewardGemsVideo.Show();
        } else {
            RequestGemsRewardVideo();
            rewardGemsVideo.Show();
        }
    }

    private void InitGemRewardVideo() {
        RequestGemsRewardVideo();
        // Get singleton reward based video ad reference
        rewardGemsVideo.OnAdRewarded += HandleRewardGemsRewarded;
        rewardGemsVideo.OnAdClosed += HandleRewardGemsClosed;
    }

    public void HandleRewardGemsRewarded(object sender, Reward args) {
        PlayerController.player.gems += 30;
    }

    public void HandleRewardGemsClosed(object sender, EventArgs args) {
        this.RequestGemsRewardVideo();
    }


    //Box 
    private void RequestBoxRewardVideo() {
        // Create an empty ad request.
        this.rewardBoxVideo = RewardBasedVideoAd.Instance;
        AdRequest request = GetTestRequest();
        // Load the rewarded video ad with the request.
        this.rewardBoxVideo.LoadAd(request, revardVideoId);
        if (rewardGemsVideo.IsLoaded()) {
            UIController.Instance.SetEnabledAdBox(false);
        } else {
            UIController.Instance.SetEnabledAdBox(true);
        }
    }

    public void ShowBoxRevardVideo() {
        if (rewardGemsVideo.IsLoaded()) {
            rewardGemsVideo.Show();
        } else {
            UIController.Instance.SetEnabledAdBox(false); 
        }
    }

    private void InitBoxRewardVideo() {
        RequestBoxRewardVideo();
        rewardGemsVideo.OnAdRewarded += HandleRewardGemsRewarded;
        rewardGemsVideo.OnAdClosed += HandleRewardGemsClosed;
    }

    public void HandleRewardBoxRewarded(object sender, Reward args) {
        UIController.Instance.OpenBoxOpen();
    }

    public void HandleRewardBoxClosed(object sender, EventArgs args) {
        this.RequestGemsRewardVideo();
    }

    ////MoreBalls
    //public void ShowBallsRevardVideo() {
    //    if (rewardGemsVideo.IsLoaded()) {
    //        rewardGemsVideo.Show();
    //    }
    //    else {

    //    }
    //}

    //private void InitBallsRewardVideo() {
    //    RequestRewardVideo();
    //    // Get singleton reward based video ad reference
    //    rewardGemsVideo.OnAdRewarded += HandleRewardBallsRewarded;
    //    rewardGemsVideo.OnAdClosed += HandleRewardBallsClosed;
    //}

    //public void HandleRewardBallsRewarded(object sender, Reward args) {
    //    BallLauncher.ExtraBalls += UnityEngine.Random.Range(1, 6);
    //}

    //public void HandleRewardBallsClosed(object sender, EventArgs args) {
    //    this.RequestRewardVideo();
    //}

    #endregion 
}
