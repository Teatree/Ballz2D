using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayServicesUI : SceneSingleton<PlayServicesUI> {
    
	void Start () {
		
	}

    public void Unlock1Achievement () {
        PlayServicesController.Instance.UnlockAchievement(GPGSIds.achievement_started);

    }

    public void ShowAchievements() {
        PlayServicesController.Instance.ShowAchievemntsUI();
    }

    public void ShowLeaderboard() {
        PlayServicesController.Instance.ShowLeaderboardsUI();
    }
}
