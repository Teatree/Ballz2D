using System.Collections.Generic;
using UnityEngine;

public class SettingsPoup : IPopup<SettingsPoup> {

    public void RateTheGame() {
        #if UNITY_ANDROID
                Application.OpenURL("market://details?id=com.FearlessDoodlez.BirckTheBalls");
        #elif UNITY_IPHONE
                        Application.OpenURL("itms-apps://itunes.apple.com/app/idYOUR_APP_ID");
        #endif
    }

    public void PrivacyPolicy() {
        Application.OpenURL("https://docs.google.com/document/d/1nq-UQ8j5vF09rLokYYZFsxljIyPJczRbdyQgEo43SBQ/edit?usp=sharing");
    }

    public void ResetProgress() {
        PlayerController.player.gems = 400;
        PlayerController.player.completedLvls = new List<CompletedLevel>();
        PlayerController.starsPerLvl = new Dictionary<int, int>();
        PlayerController.player.stars = 0;

        PlayerController.player.specialBallImageName = "knob";
        PlayerController.player.specialBallName = "Knob";
        LevelController.SpecialBall = "knob";
        PlayerController.player.items = new List<ItemData>();
        ItemData i = new ItemData();
        i.costGems = 10;
        i.name = "Knob";
        i.amount = 0;
        i.enabled = true;
        PlayerController.player.items.Add(i);

        PlayerController.player.progressTowardsNextStarBox = 0;
        PlayerController.player.numStarBoxesOpened = 0;
        AllLevelsData.CurrentLevelIndex = 0;
        LevelController.ResetScore();
        DataController.SavePlayer(PlayerController.player);
        SceneController.sceneController.ReloadMenu();

    }
}
