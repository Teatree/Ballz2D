using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FacebookController : SceneSingleton<FacebookController> {

    public Text FriendsText;

    private void Awake() {
        if (!FB.IsInitialized) {
            FB.Init(() => {
                if (FB.IsInitialized)
                    FB.ActivateApp();
                else
                    Debug.LogError("Couldn't initialize");
            },
            isGameShown => {
                if (!isGameShown)
                    Time.timeScale = 0;
                else
                    Time.timeScale = 1;
            });
        }
        else
            FB.ActivateApp();
    }

    #region Login / Logout
    public void FacebookLogin() {
        var permissions = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(permissions);
    }

    public void FacebookLogout() {
        FB.LogOut();
    }
    #endregion

    public void FacebookShare() {
        FB.ShareLink(new System.Uri("https://i.kym-cdn.com/entries/icons/facebook/000/021/033/Screenshot_236.jpg"), "Check it out!",
            "It's a doggo!",
            new System.Uri("https://resocoder.com/wp-content/uploads/2017/01/logoRound512.png"));
    }

    #region Inviting
    public void FacebookGameRequest() {
        FB.AppRequest("Hey! Come and play this awesome game!", title: "Reso Coder Tutorial");
    }

    public void FacebookInvite() {
        FB.Mobile.AppInvite(new System.Uri("https://i.ytimg.com/vi/eLLZd7fW244/maxresdefault.jpg"));
    }
    #endregion

    public void GetFriendsPlayingThisGame() {
        string query = "/me/friends";
        FB.API(query, HttpMethod.GET, result => {
            var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            var friendsList = (List<object>)dictionary["data"];
            FriendsText.text = string.Empty;
            foreach (var dict in friendsList)
                FriendsText.text += ((Dictionary<string, object>)dict)["name"];
        });
    }
}