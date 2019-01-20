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
        Social.ReportProgress(id, 100, success => { });
    }

    public void ShowAchievemntsUI() {
        Social.ShowAchievementsUI();
    }
    #endregion

    #region leaderboard
    public void PublishScoreToLeaderBoard( long score) {
        Social.ReportScore(score, GPGSIds.leaderboard_ballsy_leaders, success => { });
    }

    public void ShowLeaderboardsUI() {
        Social.ShowAchievementsUI();
    }
    #endregion
}
