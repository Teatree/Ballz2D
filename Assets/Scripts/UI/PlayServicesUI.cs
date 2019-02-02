﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayServicesUI : SceneSingleton<PlayServicesUI> {
    
	void Start () {
		
	}

    public void Unlock1Achievement () {
#if UNITY_ANDROID
        PlayServicesController.Instance.UnlockAchievement(GPGSIds.achievement_started);
#endif
    }

    public void ShowAchievements() {
#if UNITY_ANDROID
        PlayServicesController.Instance.ShowAchievemntsUI();
#endif
    }

    public void ShowLeaderboard() {
#if UNITY_ANDROID
        PlayServicesController.Instance.ShowLeaderboardsUI();
#endif
    }
}
