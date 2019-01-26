using System;
using System.Collections.Generic;

public class PlayerController : SceneSingleton<PlayerController> {
    public static PlayerData player;
    public static Dictionary<int, int> starsPerLvl;

    private void Start() {
        if (player == null) {
            player = DataController.LoadPlayer() != null ? DataController.LoadPlayer() : new PlayerData();
            DateTime dt = DateTime.Now;
            player.lastLogin = dt.ToString("yyyy-MM-dd");

            starsPerLvl = new Dictionary<int, int>();
            if (player.completedLvls != null && player.completedLvls.Count > 0) {
                foreach (CompletedLevel lvl in player.completedLvls) {
                    starsPerLvl.Add(lvl.number, lvl.stars);
                }
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

    private int getAmountOfItem(string iname) {
        if (player.items != null && player.items.Count > 0) {
            ItemData idata = player.items.Find(x => x.name.Equals(iname));
            return idata != null ? idata.amount : 0;
        } else {
            return 0;
        }
    }

    public int GetAmountOfLigntnings() {
        return getAmountOfItem("lightning");
    }

    public int GetAmountOfExtraBalls() {
        return getAmountOfItem("extraball");
    }
}
