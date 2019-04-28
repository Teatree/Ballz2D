
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

    public int AdBoxOpenLimit = 3;

    void Start() {
        UIController.Instance.SetEnabledAdBox(false);
        if (Monetization.isSupported && PlayerController.player.adBoxOpenedCount < AdBoxOpenLimit) {
            Monetization.Initialize(gameId, true);
        }
    }

    public void ShowMoreHCReviveAd() {
        ShowAdCallbacks options = new ShowAdCallbacks();
        options.finishCallback = HandleMoreHCGiveNastya;
        ShowAdPlacementContent ad = Monetization.GetPlacementContent(moreHCAd) as ShowAdPlacementContent;
        ad.Show(options);

        PlayerController.player.MoreHCReviveCount++;
        ((Revive)Revive.Instance).UpdateButtsButtonState();
    }

    public void ShowBoxAd() {
        DateTime dt = PlayerController.player == null || PlayerController.player.adBoxOpenedDate == null || PlayerController.player.adBoxOpenedDate == "" ? DateTime.Now : DateTime.Parse(PlayerController.player.adBoxOpenedDate);

        if (DateTime.Now.Date == dt) {
            // if Player came logged in after box was supposed to be claiemd

            // Not exceding limit
            if(PlayerController.player.adBoxOpenedCount < AdBoxOpenLimit) {
                ShowAdCallbacks options = new ShowAdCallbacks();
                options.finishCallback = HandleOpenAdBoxNastya;
                ShowAdPlacementContent ad = Monetization.GetPlacementContent(boxAd) as ShowAdPlacementContent;
                ad.Show(options);

                PlayerController.player.adBoxOpenedCount++;
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
    }

    void HandleMoreBalls(ShowResult result) {
        if (result == ShowResult.Finished) {
            Debug.LogWarning(" REWARD!");
            int ballsAmount = MoreBallsPowerup.Instance.getRandomBallsAmount();
            GameUIController.Instance.ShowItemReceived(ballsAmount);
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
}
