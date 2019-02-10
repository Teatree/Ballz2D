﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SceneSingleton<PlayerController> {
    public static PlayerData player;
    public static Dictionary<int, int> starsPerLvl;
    public static int progressTowardsStarBox;

    public List<BoxObject> starBoxes;

    private void Start() {

        if (player == null) {
            player = DataController.LoadPlayer() != null ? DataController.LoadPlayer() : new PlayerData();
            DateTime dt = DateTime.Now;
            player.lastLogin = dt.ToString("yyyy-MM-dd HH:mm");

            SetupDailyBoxStuff();

            starsPerLvl = new Dictionary<int, int>();
            if (player.completedLvls != null && player.completedLvls.Count > 0) {
                foreach (CompletedLevel lvl in player.completedLvls) {
                    starsPerLvl.Add(lvl.number, lvl.stars);
                }
            }
        }
    }

    private static void SetupDailyBoxStuff() {
        if (player.giveBoxAt != null && player.giveBoxAt != "") {
            DateTime getBoxAt = DateTime.ParseExact(player.giveBoxAt, "yyyy-MM-dd HH:mm", null);
            if (getBoxAt < DateTime.Now) {
                UIController.Instance.ShowDayBoxButton();
            }
            else {
                //set timer 
                DayBoxTimer.Instance.SetCountDownTo(getBoxAt);
                UIController.Instance.ShowDayBoxWaitButton();
            }
        }
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
    }

    private static void SavePlayer() {
        List<CompletedLevel> lvls = new List<CompletedLevel>();
            foreach (KeyValuePair<int, int> lvl in starsPerLvl) {
                lvls.Add(new CompletedLevel(lvl.Key, lvl.Value));
            }
        player.completedLvls = lvls;
        DataController.SavePlayer(player);
    }

    void OnApplicationQuit() {
        SavePlayer();
        //List<CompletedLevel> lvls = new List<CompletedLevel>();
        //foreach (KeyValuePair<int, int> lvl in starsPerLvl) {
        //    lvls.Add(new CompletedLevel(lvl.Key, lvl.Value));
        //}
        //player.completedLvls = lvls;
        //DataController.SavePlayer(player);
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
