using System;
using System.Collections.Generic;

public class PlayerController : SceneSingleton<PlayerController> {
    public static PlayerData player;
    public Dictionary<int, int> starsPerLvl;

    private void Start() {

        player = DataController.LoadPlayer() != null && player == null ? DataController.LoadPlayer() : new PlayerData();
        DateTime dt = DateTime.Now;
        player.lastLogin = dt.ToString("yyyy-MM-dd");

        starsPerLvl = new Dictionary<int, int>();
        if (player.completedLvls != null && player.completedLvls.Count > 0) {
            foreach (CompletedLevel lvl in player.completedLvls) {
                starsPerLvl.Add(lvl.number, lvl.stars);
            }
        }
    }

    

    public void AddNewCompletedLevel(int lvlNum, int stars) {
        if (starsPerLvl.ContainsKey(lvlNum)) {
            starsPerLvl[lvlNum] = stars;
        } else {
            starsPerLvl.Add(lvlNum, stars);
        }
    }

    void OnApplicationQuit() {
        List<CompletedLevel> lvls = new List<CompletedLevel>();
        foreach (KeyValuePair<int, int> lvl in starsPerLvl) {
            lvls.Add(new CompletedLevel(lvl.Key, lvl.Value));
        }
        player.completedLvls = lvls;
        DataController.SavePlayer(player);
    }


}
