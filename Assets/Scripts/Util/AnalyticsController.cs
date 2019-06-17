using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;

public class AnalyticsController : SceneSingleton<AnalyticsController> {

    // Use this for initialization
    void Awake() {
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

    IEnumerator WaitForRequest(WWW data) {
        yield return data; // Wait until the download is done
        if (data.error != null) {
            Debug.Log("There was an error sending request: " + data.error);
        }
        else {
            Debug.Log("WWW Request: " + data.text);
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

    public class JsonData {
        public string playerID = "!%*(AS";
    }

    public class GeneralTrackingData : JsonData {
        public string playerStatus = "online / offline";
    }

    public void LogPlayersOnline(string playerStatus) {
        // Custom tracker
        GeneralTrackingData generalData = new GeneralTrackingData();
        generalData.playerID = PlayerController.player.PlayerID;
        generalData.playerStatus = playerStatus;

        PostRequest(generalData, "http://5.45.69.185:80/playersonline");
    }

    #region shop
    public class HCTransactionTrackingData {
        public string playerID = "!%*(AS";
        public string contentId = "Ball_Yellow, Joker, ";
        public string contentType = "HC, Balls Booster";
        public string totalValue = "1, 23";
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

        // Custom tracker
        HCTransactionTrackingData hcTransactionData = new HCTransactionTrackingData();
        hcTransactionData.playerID = PlayerController.player.PlayerID;
        hcTransactionData.contentId = contentId;
        hcTransactionData.contentType = contentType;
        hcTransactionData.totalValue = totalValue.ToString();

        string json = JsonUtility.ToJson(hcTransactionData);

        WWW www;
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");

        // convert json string to byte
        var formData = System.Text.Encoding.UTF8.GetBytes(json);

        www = new WWW("http://5.45.69.185:80/hctransaction", formData, postHeader);
        StartCoroutine(WaitForRequest(www));
    }

    

    public class IAPTrackingData : JsonData {
        public string IAPID = "Weekend Offer";
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

        // Custom tracker
        IAPTrackingData iapData = new IAPTrackingData();
        iapData.playerID = PlayerController.player.PlayerID;
        iapData.IAPID = IAP_ID;

        PostRequest(iapData, "http://5.45.69.185:80/iap");
    }
    
    #endregion

    public class IncentivizedAdTrackingData : JsonData {
        public string ad_ID = "Box Ad";
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

        // Custom tracker
        IncentivizedAdTrackingData incentivizedAdData = new IncentivizedAdTrackingData();
        incentivizedAdData.playerID = PlayerController.player.PlayerID;
        incentivizedAdData.ad_ID = ad_ID;

        PostRequest(incentivizedAdData, "http://5.45.69.185:80/incentivizedad");
    }

    #region boxes
    public class BoxTrackingData : JsonData{
        public string boxID = "Ad Box, Star Box, etc";
        public string itemReceived = "HC, Balls Booster";
        public string itemAmount = "1, 23";
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

        // Custom tracker
        BoxTrackingData boxData = new BoxTrackingData();
        boxData.playerID = PlayerController.player.PlayerID;
        boxData.boxID = box_id;
        boxData.itemReceived = item;
        boxData.itemAmount = amount.ToString();

        PostRequest(boxData, "http://5.45.69.185:80/box");
    }
    #endregion

    #region boosters
    public class BoosterTrackingData : JsonData {
        public string boosterType = "Lightning, More Balls";
        public string levelNum = "numer of shots in string form";
        public string numberOfShots = "numer of shots in string form";
    }

    public void LogLevelBoostsUsedEvent(string lvlNum, string boostType, int shotsCount) {
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

        // Custom tracker
        BoosterTrackingData boosterData = new BoosterTrackingData();
        boosterData.playerID = PlayerController.player.PlayerID;
        boosterData.boosterType = boostType;
        boosterData.levelNum = lvlNum;
        boosterData.numberOfShots = shotsCount.ToString();

        PostRequest(boosterData, "http://5.45.69.185:80/booster");
    }
    #endregion

    #region Levels
    public class LevelTrackingData : JsonData {
        public string levelNum = "Level 1, Level 32";
        public string levelResult = "Failed, Resterted, Finished";
        public string numberOfShots = "numer of shots in string form";
    }

    public void LogLevelCompletedEvent(string levelNum, int shotsCount) {
        // Facebook analytics
        var parameters = new Dictionary<string, object>() {
            {"levelNum2", levelNum }
        };
        FB.LogAppEvent(
            "LevelCompleted",
            0,
            parameters
        );

        // Unity Analytics
        Analytics.CustomEvent("LevelCompleted", new Dictionary<string, object>
        {
            { "levelNum", levelNum }
        });


        // Custom tracker
        LevelTrackingData levelData = new LevelTrackingData();
        levelData.playerID = PlayerController.player.PlayerID;
        levelData.levelNum = levelNum;
        levelData.levelResult = "Completed";
        levelData.numberOfShots = shotsCount.ToString(); // TODO

        PostRequest(levelData, "http://5.45.69.185:80/level");
    }

    public void LogLevelFailedEvent(string levelNum, int shotsCount) {
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

        // Custom tracker
        LevelTrackingData levelData = new LevelTrackingData();
        levelData.levelNum = levelNum;
        levelData.levelResult = "Failed";
        levelData.numberOfShots = shotsCount.ToString(); // TODO

        string json = JsonUtility.ToJson(levelData);

        WWW www;
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");

        // convert json string to byte
        var formData = System.Text.Encoding.UTF8.GetBytes(json);

        www = new WWW("http://5.45.69.185:80/level", formData, postHeader);
        StartCoroutine(WaitForRequest(www));
    }

    public void LogLevelRestartedEvent(string levelNum, int shotsCount) {
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

        // Custom tracker
        LevelTrackingData levelData = new LevelTrackingData();
        levelData.levelNum = levelNum;
        levelData.levelResult = "Restarted";
        levelData.numberOfShots = shotsCount.ToString(); // TODO

        string json = JsonUtility.ToJson(levelData);

        WWW www;
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");

        // convert json string to byte
        var formData = System.Text.Encoding.UTF8.GetBytes(json);

        www = new WWW("http://5.45.69.185:80/level", formData, postHeader);
        StartCoroutine(WaitForRequest(www));
    }
    #endregion

    private void PostRequest(JsonData data, string url) {
        string json = JsonUtility.ToJson(data);

        WWW www;
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");

        // convert json string to byte
        var formData = System.Text.Encoding.UTF8.GetBytes(json);

        www = new WWW(url, formData, postHeader);
        StartCoroutine(WaitForRequest(www));
    }
}
