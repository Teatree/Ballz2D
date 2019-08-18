using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SceneSingleton<PlayerController> {
    //TODO remove someday 
    public static int hardcodedLevlsCompleted = 140;

    public static PlayerData player;
    public static Dictionary<int, int> starsPerLvl; 
    public static int progressTowardsStarBox;

    // Daily Box Stuss
    private static DateTime morning9 = DateTime.Now.Date.AddHours(9);
    private static DateTime morning13 = DateTime.Now.Date.AddHours(13);
    private static DateTime morning17 = DateTime.Now.Date.AddHours(17);
    private static DateTime morning21 = DateTime.Now.Date.AddHours(21);
    public static List<DateTime> times = new List<DateTime>();

    public List<BoxObject> starBoxes;

    private void Start() {
        times.Add(morning9);
        times.Add(morning13);
        times.Add(morning17);
        times.Add(morning21);

        PlayerDataLoad();
    }

    public static void PlayerDataLoad() {
        if (player == null) {
            player = DataController.LoadPlayer() != null ? DataController.LoadPlayer() : new PlayerData();

            starsPerLvl = new Dictionary<int, int>();
            if (hardcodedLevlsCompleted > 0 ) {
                for (int i = 0; i < hardcodedLevlsCompleted; i++) {
                    starsPerLvl.Add(i, 3);
                }
            } else 
            if (player.completedLvls != null && player.completedLvls.Count > 0) {
                foreach (CompletedLevel lvl in player.completedLvls) {
                    starsPerLvl.Add(lvl.number, lvl.stars);
                }
            }

            LevelController.SpecialBall = player.specialBallImageName;
        }
        AnalyticsController.Instance.LogPlayersOnline("online");
    }

    public static string GenerateID() {
        return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Player" + UnityEngine.Random.Range(0, 100000000) + DateTime.Now));
    }

    public void AddNewCompletedLevel(int lvlNum, int stars) {
        if (starsPerLvl.ContainsKey(lvlNum)) {
            if (starsPerLvl[lvlNum] < stars) { 
                starsPerLvl[lvlNum] = stars;
            }
        } else {
            starsPerLvl.Add(lvlNum, stars);
        }
    }

    private void OnApplicationPause(bool pause) {
        
        SavePlayer();

        if (pause) {
            AnalyticsController.Instance.LogPlayersOnline("offline");
            NotificationController.Instance.ScheduleComeback(DateTime.Now.AddHours(6));
        }
        else {
            AnalyticsController.Instance.LogPlayersOnline("online");
        }
    }

    private static void SavePlayer() {
        if (PlayerController.player != null) {
            List<CompletedLevel> lvls = new List<CompletedLevel>();

            if (starsPerLvl != null) {
                foreach (KeyValuePair<int, int> lvl in starsPerLvl) {
                    lvls.Add(new CompletedLevel(lvl.Key, lvl.Value));
                }
                player.completedLvls = lvls;
            }
            DataController.SavePlayer(player);
        }
    }

    void OnApplicationQuit() {
        AnalyticsController.Instance.LogPlayersOnline("offline");
        SavePlayer();

        //NotificationController.Instance.ScheduleBoxComeback(DateTime.Now.AddSeconds(30)/*DateTime.Now.AddHours(8)*/);
    }

    private int getAmountOfItem(string iname) {
        if (player.items != null && player.items.Count > 0) {
            ItemData idata = player.items.Find(x => x.name.Equals(iname));
            return idata != null ? idata.amount : 0;
        } else {
            return 0;
        }
    }

    public int GetAmountOfLigntnings() {
        return getAmountOfItem("Booster Lightning");
    }

    public int GetAmountOfExtraBalls() {
        return getAmountOfItem("Booster Balls");
    }
}
