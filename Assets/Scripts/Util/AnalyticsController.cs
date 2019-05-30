﻿using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsController : SceneSingleton<AnalyticsController> {

    // Use this for initialization
    void Awake () {

        if (FB.IsInitialized) {
            FB.ActivateApp();
        }
        else {
            //Handle FB.Init
            FB.Init(() => {
                FB.ActivateApp();
            });
        }

    }

    private void OnApplicationPause(bool pause) {

        if (!pause) {
            //app resume
            if (FB.IsInitialized) {
                FB.ActivateApp();
            }
            else {
                //Handle FB.Init
                FB.Init(() => {
                    FB.ActivateApp();
                });
            }
        }
    }

    public void LogSpendCreditsEvent(string contentId, string contentType, double totalValue) {
        var parameters = new Dictionary<string, object>();
        parameters[AppEventParameterName.ContentID] = contentId;
        parameters[AppEventParameterName.ContentType] = contentType;
        FB.LogAppEvent(
            AppEventName.SpentCredits,
            (float)totalValue,
            parameters
        );

        Analytics.CustomEvent("HC_Spent", new Dictionary<string, object>
        {
            { "contentId", contentId},
            { "contentType", contentType},
            { "totalValue", totalValue}
        });
    }

    public void LogIAPEvent(string IAP_ID) {
        var parameters = new Dictionary<string, object>();
        parameters["iap"] = IAP_ID;
        FB.LogAppEvent(
            "IAPClicked",
            0,
            parameters
        );

        Analytics.CustomEvent("IAPClicked", new Dictionary<string, object>
        {
            { "IAP_ID", IAP_ID}
        });
    }

    public void LogIncentivizedAdWatchedEvent(string ad_ID) {
        var parameters = new Dictionary<string, object>();
        parameters["ad_id"] = ad_ID;
        FB.LogAppEvent(
            "IncentiziedVideoWatched",
            0,
            parameters
        );

        Analytics.CustomEvent("IncentiziedVideoWatched", new Dictionary<string, object>
        {
            { "ad_ID", ad_ID}
        });
    }

    public void LogBoxesOpenedEvent(string box_id, string item, int amount) {
        var parameters = new Dictionary<string, object>();
        parameters["box_id"] = box_id;
        parameters["item"] = item;
        parameters["amount"] = amount;
        FB.LogAppEvent(
            "BoxesOpened",
            0,
            parameters
        );

        Analytics.CustomEvent("BoxesOpened", new Dictionary<string, object>
        {
            { "box_ID", box_id},
            { "item", item },
            { "amount", amount}
        });
    }

    #region Levels
    public void LogLevelShotsEvent(string lvlNum, int shotsCount) {
        var parameters = new Dictionary<string, object>();
        parameters["lvlNum"] = lvlNum;
        parameters["shotsCount"] = shotsCount;
        FB.LogAppEvent(
            "LevelShotsRequired",
            0,
            parameters
        );

        Analytics.CustomEvent("LevelShotsRequired", new Dictionary<string, object>
        {
            { "levelNum", lvlNum },
            { "shotCount", shotsCount }
        });
    }

    public void LogLevelBoostsUsedEvent(string lvlNum, string boostType) {
        var parameters = new Dictionary<string, object>();
        parameters["lvlNum"] = lvlNum;
        parameters["boostType"] = boostType;
        FB.LogAppEvent(
            "LevelBoostUsedEvent",
            0,
            parameters
        );

        Analytics.CustomEvent("LevelBoostUsedEvent", new Dictionary<string, object>
        {
            { "levelNum", lvlNum },
            { "boostType", boostType }
        });
    }

    public void LogLevelCompletedEvent(string levelNum) {
        var parameters = new Dictionary<string, object>() {
            {"levelNum2", levelNum }
        };
        FB.LogAppEvent(
            "LevelCompleted",
            0,
            parameters
        );

        Analytics.CustomEvent("LevelCompleted", new Dictionary<string, object>
        {
            { "levelNum", levelNum }
        });
    }

    public void LogLevelFailedEvent(string levelNum) {
        var parameters = new Dictionary<string, object>();
        parameters["levelNum"] = levelNum;
        FB.LogAppEvent(
            "LevelFailed",
            0,
            parameters
        );

        Analytics.CustomEvent("LevelFailed", new Dictionary<string, object>
        {
            { "levelNum", levelNum }
        });
    }

    public void LogLevelRestartedEvent(string levelNum) {
        var parameters = new Dictionary<string, object>();
        parameters["levelNum"] = levelNum;
        FB.LogAppEvent(
            "LevelRestarted",
            0,
            parameters
        );

        Analytics.CustomEvent("LevelRestarted", new Dictionary<string, object>
        {
            { "levelNum", levelNum }
        });
    }
    #endregion
}