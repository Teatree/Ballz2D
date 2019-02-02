using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class PlayServicesController : SceneSingleton<PlayServicesController> {

	void Start () {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        SignIn();
	}

    void SignIn() {
        Social.localUser.Authenticate(success => { });
    }

    #region achievemnts
    public void UnlockAchievement(string id) {
        if (PlayGamesPlatform.Instance.localUser.authenticated) { 
            Social.ReportProgress(id, 100, success => { });
        }
    }

    public void ShowAchievemntsUI() {
        if (PlayGamesPlatform.Instance.localUser.authenticated) {
            Social.ShowAchievementsUI();
        } else {
            Social.localUser.Authenticate(success => { Social.ShowAchievementsUI(); });
        }
    }
    #endregion

    #region leaderboard
    public void PublishScoreToLeaderBoard( long score) {
#if UNITY_ANDROID
        if (PlayGamesPlatform.Instance.localUser.authenticated) {
            Social.ReportScore(score, GPGSIds.leaderboard_ballsy_leaders, success => { });
        }
#endif
    }

    public void ShowLeaderboardsUI() {
        if (PlayGamesPlatform.Instance.localUser.authenticated) {
            Social.ShowLeaderboardUI();
        } else {
            Social.localUser.Authenticate(success => { Social.ShowLeaderboardUI(); });
        }
    }
    #endregion
}
