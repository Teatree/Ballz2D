using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsPoup : IPopup<SettingsPoup> {

    public void RateTheGame() {
        #if UNITY_ANDROID
                Application.OpenURL("market://details?id=com.fd.etf.android");
        #elif UNITY_IPHONE
                        Application.OpenURL("itms-apps://itunes.apple.com/app/idYOUR_APP_ID");
        #endif
    }

    public void ResetProgress() {
        PlayerController.player.gems = 0;
        PlayerController.player.completedLvls = new List<CompletedLevel>();
        PlayerController.starsPerLvl = new Dictionary<int, int>();
        PlayerController.player.stars = 0;
        PlayerController.player.items = new List<ItemData>();
        AllLevelsData.CurrentLevelIndex = 0;
        LevelController.ResetScore();
        DataController.SavePlayer(PlayerController.player);
        SceneController.sceneController.ReloadMenu();
    }
}
