﻿using System.Collections.Generic;
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
        FB.ShareLink(new System.Uri("https://play.google.com/store/apps/details?id=com.FearlessDoodlez.BirckTheBalls"), "Check it out!",
            "This game has balls!",
//            new System.Uri("https://raw.githubusercontent.com/fearlessdoodlez/balls/master/bunny.png")
            null);
    }

    #region Inviting
    public void FacebookGameRequest() {
        FB.AppRequest("Hey! Come and play this awesome game!", title: "Ball vs Bricks");
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