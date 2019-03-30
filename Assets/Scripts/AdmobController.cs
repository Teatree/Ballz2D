using UnityEngine;
using System;
using GoogleMobileAds.Api;

public class AdmobController : SceneSingleton<AdmobController> {

#if UNITY_ANDROID
    string appId = "ca-app-pub-4809397092315700~2101070495";
    string bannerId = "ca-app-pub-4809397092315700/7133905324"; //Sizes https://developers.google.com/admob/unity/banner#banner_sizes
    string interstitialId = "ca-app-pub-4809397092315700/3492498628";
    string rewardVideoId = "ca-app-pub-4809397092315700/9646843432";
    string boxrewardVideoId = "ca-app-pub-4809397092315700/4309780879";
    string ballsrewardVideoId = "ca-app-pub-4809397092315700~2101070495";
#elif UNITY_IPHONE
     string appId = "ca-app-pub-4809397092315700~2101070495";
    string bannerId = "ca-app-pub-4809397092315700/7133905324";
    string interstitialId = "ca-app-pub-4809397092315700/3492498628";
    string rewardVideoId = "ca-app-pub-4809397092315700/9646843432";
     string boxrewardVideoId = "ca-app-pub-4809397092315700/9646843432";
        string ballsrewardVideoId = "ca-app-pub-4809397092315700~2101070495";
#else
    string appId = "ca-app-pub-4809397092315700~2101070495";
    string bannerId = "ca-app-pub-4809397092315700/7133905324";
    string interstitialId = "ca-app-pub-4809397092315700/3492498628";
    string rewardVideoId = "ca-app-pub-4809397092315700/9646843432";
     string boxrewardVideoId = "ca-app-pub-4809397092315700/9646843432";
        string ballsrewardVideoId = "ca-app-pub-4809397092315700~2101070495";
#endif


    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardBasedVideoAd rewardGemsVideo;
    private RewardBasedVideoAd rewardBoxVideo;
    private RewardBasedVideoAd rewardBallsVideo;

    public void Start() {
        MobileAds.Initialize(appId);

        this.RequestBanner();
        //InitGemRewardVideo();
        //InitBoxRewardVideo();
        //if (!PlayerController.player.noAds) {
        //    RequestInterstitial();
        //    bannerView.Show();
        //}
    }

    private static AdRequest GetTestRequest() {
        //.AddTestDevice("F3BF2A3E2B31B3411BC09B6435FC8160")
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
        if (!PlayerController.player.noAds && this.interstitial.IsLoaded()) {
            this.interstitial.Show();
        }
    }
    public void HandleInterstitialClosed(object sender, EventArgs args) {
        MonoBehaviour.print("HandleInterstitialClosed event received");
    }
    #endregion

    #region rewards

    private void RequestGemsRewardVideo() {
        this.rewardGemsVideo = RewardBasedVideoAd.Instance;
        AdRequest request = GetTestRequest();
        this.rewardGemsVideo.LoadAd(request, rewardVideoId);
    }

    public void ShowGemsrewardVideo() {
        if (rewardGemsVideo.IsLoaded()) {
            rewardGemsVideo.Show();
        }
        else {
            RequestGemsRewardVideo();
            rewardGemsVideo.Show();
        }
    }

    private void InitGemRewardVideo() {
        RequestGemsRewardVideo();
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
        this.rewardBoxVideo = RewardBasedVideoAd.Instance;
        AdRequest request = GetTestRequest();
        this.rewardBoxVideo.LoadAd(request, boxrewardVideoId);
        if (UIController.Instance != null)
            if (rewardBoxVideo.IsLoaded()) {
                UIController.Instance.SetEnabledAdBox(true);
            }
            else {
                UIController.Instance.SetEnabledAdBox(false);
            }
    }

    public void ShowBoxrewardVideo() {
        if (rewardBoxVideo.IsLoaded()) {
            Debug.Log(">>>> show box reward");
            rewardBoxVideo.Show();
        }
        else {
            UIController.Instance.SetEnabledAdBox(false);
        }
    }

    private void InitBoxRewardVideo() {
        RequestBoxRewardVideo();
        rewardBoxVideo.OnAdRewarded += HandleRewardBoxRewarded;
        rewardBoxVideo.OnAdClosed += HandleRewardBoxClosed;
    }

    public void HandleRewardBoxRewarded(object sender, Reward args) {
        UIController.Instance.OpenBoxOpen();
        UIController.Instance.SetEnabledAdBox(false);
    }

    public void HandleRewardBoxClosed(object sender, EventArgs args) {
        this.RequestBoxRewardVideo();
    }

    ////MoreBalls
    private void RequestBallsRewardVideo() {
        this.rewardBallsVideo = RewardBasedVideoAd.Instance;
        AdRequest request = GetTestRequest();
        this.rewardBoxVideo.LoadAd(request, ballsrewardVideoId);
        //if (UIController.Instance != null)
        //    if (rewardBallsVideo.IsLoaded()) {
        //        UIController.Instance.SetEnabledAdBox(true);
        //    }
        //    else {
        //        UIController.Instance.SetEnabledAdBox(false);
        //    }
    }

    public void ShowBallsrewardVideo() {
        if (Application.platform == RuntimePlatform.Android) {
            if (rewardBallsVideo.IsLoaded()) {
                rewardBallsVideo.Show();
            }
        }
        else {
            MoreBallsPowerup.Instance.GetMoreBalls(10);
        }
    }

    private void InitBallsRewardVideo() {
        RequestBallsRewardVideo();
        // Get singleton reward based video ad reference
        rewardBallsVideo.OnAdRewarded += HandleRewardBallsRewarded;
        rewardBallsVideo.OnAdClosed += HandleRewardBallsClosed;
    }

    public void HandleRewardBallsRewarded(object sender, Reward args) {
        MoreBallsPowerup.Instance.GetMoreBalls(10);
    }

    public void HandleRewardBallsClosed(object sender, EventArgs args) {
        this.RequestBallsRewardVideo();
    }

    #endregion
}
