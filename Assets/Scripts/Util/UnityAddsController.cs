﻿
using System;
using System.Collections;
using UnityEngine;

using UnityEngine.Monetization;


public class UnityAddsController : SceneSingleton<UnityAddsController> {
    private const int extraGems = 40;
    private string gameId = "3108568";
    private string boxAd = "BoxRewarded";
    private string moreHCAd = "MoreHCRewarded";
    private string moreBallsAd = "MoreBallsRewarded";

    public Sprite iconSprte;
    public static bool AdsLoaded;
    
    //intersticials
    private string enterActionPhaseAfterRestartAd = "EnterActionPhaseAfterRestart";
    private string enterActionPhaseFromMainMenuAd = "EnterActionPhaseFromMainMenu";

    public int AdBoxOpenLimit = 3;

    void Start() {
        if (Monetization.isSupported) {
            Monetization.Initialize(gameId, false);
        }

        AdsLoaded = Application.internetReachability != NetworkReachability.NotReachable;
        //AdsLoaded = false;
        if (!AdsLoaded) {
            UIController.Instance.SetEnabledAdBox(false);
        }
    }

    //private void Update() {
    //    if (Application.internetReachability == NetworkReachability.NotReachable) {
    //        hasInternetAccess = false;
    //    } else {
    //        hasInternetAccess = true;
    //    }
    //}

    public void ShowMoreHCReviveAd() {
        ShowAdCallbacks options = new ShowAdCallbacks();
        options.finishCallback = HandleMoreHCGiveNastya;
        ShowAdPlacementContent ad = Monetization.GetPlacementContent(moreHCAd) as ShowAdPlacementContent;
        ad.Show(options);

        PlayerController.player.MoreHCReviveCount++;
        ((Revive)Revive.Instance).UpdateButtsButtonState();

        AnalyticsController.Instance.LogIncentivizedAdWatchedEvent("More HC AD");
    }

    public void ShowBoxAd() {
        DateTime dt = PlayerController.player == null || PlayerController.player.adBoxOpenedDate == null || PlayerController.player.adBoxOpenedDate == "" ? DateTime.MinValue : DateTime.Parse(PlayerController.player.adBoxOpenedDate);

        if (DateTime.Now.Date == dt.Date) {
            // if Player came logged in after box was supposed to be claiemd

            // Not exceding limit
            if(PlayerController.player.adBoxOpenedCount < AdBoxOpenLimit && AdsLoaded) {
                ShowAdCallbacks options = new ShowAdCallbacks();
                options.finishCallback = HandleOpenAdBoxNastya;
                ShowAdPlacementContent ad = Monetization.GetPlacementContent(boxAd) as ShowAdPlacementContent;
                ad.Show(options);

                PlayerController.player.adBoxOpenedCount++;
                PlayerController.player.adBoxOpenedDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
                AnalyticsController.Instance.LogIncentivizedAdWatchedEvent("Box AD");
            }
            else {
                UIController.Instance.SetEnabledAdBox(false);
            }
        }
        else {
            PlayerController.player.adBoxOpenedDate = DateTime.Now.Date.ToString("yyyy-MM-dd");

            ShowAdCallbacks options = new ShowAdCallbacks();
            options.finishCallback = HandleOpenAdBoxNastya;
            ShowAdPlacementContent ad = Monetization.GetPlacementContent(boxAd) as ShowAdPlacementContent;
            ad.Show(options);

            PlayerController.player.adBoxOpenedCount = 1;

            AnalyticsController.Instance.LogIncentivizedAdWatchedEvent("Box AD");
        }
        // set the dat of ad shown
        // Add counter to ad shown
    }

    void HandleOpenAdBoxNastya(ShowResult result) {
        if (result == ShowResult.Finished) {
            Debug.LogWarning(" REWARD!");
            UIController.Instance.OpenBoxOpen();
        }
        else if (result == ShowResult.Skipped) {
            Debug.LogWarning("The player skipped the video - DO NOT REWARD!");
        }
        else if (result == ShowResult.Failed) {
            Debug.LogError("Video failed to show");
        }
    }

    void HandleMoreHCGiveNastya(ShowResult result) {
        if (result == ShowResult.Finished) {
            Debug.LogWarning(" REWARD!");
            GameUIController.Instance.ShowItemReceived(extraGems);
            PlayerController.player.gems += extraGems;
        }
        else if (result == ShowResult.Skipped) {
            Debug.LogWarning("The player skipped the video - DO NOT REWARD!");
        }
        else if (result == ShowResult.Failed) {
            Debug.LogError("Video failed to show");
        }
    }

    public void ShowMoreBallsReviveAd() {
        ShowAdCallbacks options = new ShowAdCallbacks();
        options.finishCallback = HandleMoreBalls;
        ShowAdPlacementContent ad = Monetization.GetPlacementContent(moreBallsAd) as ShowAdPlacementContent;
        ad.Show(options);

        AnalyticsController.Instance.LogIncentivizedAdWatchedEvent("More Balls AD");
    }

    void HandleMoreBalls(ShowResult result) {
        if (result == ShowResult.Finished) {
            Debug.LogWarning(" REWARD!");
            int ballsAmount = MoreBallsPowerup.Instance.getRandomBallsAmount();
            GameUIController.Instance.ShowItemReceived(ballsAmount, iconSprte);
            MoreBallsPowerup.Instance.GetMoreBalls(ballsAmount);
            BallLauncher.ExtraAdBalls = ballsAmount;
            BallLauncher.Instance.SetBallsUIText();
        }
        else if (result == ShowResult.Skipped) {
            Debug.LogWarning("The player skipped the video - DO NOT REWARD!");
        }
        else if (result == ShowResult.Failed) {
            Debug.LogError("Video failed to show");
        }
    }

    public void ShowEnterActionPhaseAfterRestartAd() {
        ShowAdCallbacks options = new ShowAdCallbacks();
        options.finishCallback = HandleLoadMenu;
        
        ShowAdPlacementContent ad = Monetization.GetPlacementContent(enterActionPhaseAfterRestartAd) as ShowAdPlacementContent;
        ad.Show(options);
    }

    public void ShowEnterActionPhaseFromMainMenuAd() {
        ShowAdCallbacks options = new ShowAdCallbacks();
        options.finishCallback = HandleLoadGame;

        ShowAdPlacementContent ad = Monetization.GetPlacementContent(enterActionPhaseFromMainMenuAd) as ShowAdPlacementContent;
        ad.Show(options);
    }

    void HandleLoadMenu (ShowResult result) {
        if (result == ShowResult.Finished) {
            SceneController.sceneController.LoadMenu();
        }
        else if (result == ShowResult.Skipped) {
            SceneController.sceneController.LoadMenu();
        }
        else if (result == ShowResult.Failed) {
            SceneController.sceneController.LoadMenu();
        }
    }

    void HandleLoadGame(ShowResult result) {
        if (result == ShowResult.Finished) {
            SceneController.sceneController.LoadGame();
        }
        else if (result == ShowResult.Skipped) {
            SceneController.sceneController.LoadGame();
        }
        else if (result == ShowResult.Failed) {
            SceneController.sceneController.LoadGame();
        }
    }
    
}
