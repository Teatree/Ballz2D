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

    #region shop
    public class HCTransactionTrackingData {
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
        hcTransactionData.contentId = contentId;
        hcTransactionData.contentType = contentType;
        hcTransactionData.totalValue = totalValue.ToString();

        string json = JsonUtility.ToJson(hcTransactionData);

        WWW www;
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");

        // convert json string to byte
        var formData = System.Text.Encoding.UTF8.GetBytes(json);

        www = new WWW("http://5.45.69.185:5000/hctransaction", formData, postHeader);
        StartCoroutine(WaitForRequest(www));
    }

    public class IAPTrackingData {
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
        iapData.IAPID = IAP_ID;

        string json = JsonUtility.ToJson(iapData);

        WWW www;
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");

        // convert json string to byte
        var formData = System.Text.Encoding.UTF8.GetBytes(json);

        www = new WWW("http://5.45.69.185:5000/iap", formData, postHeader);
        StartCoroutine(WaitForRequest(www));
    }
    #endregion

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

    #region boxes
    public class BoxTrackingData {
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
        boxData.boxID = box_id;
        boxData.itemReceived = item;
        boxData.itemAmount = amount.ToString();

        string json = JsonUtility.ToJson(boxData);

        WWW www;
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");

        // convert json string to byte
        var formData = System.Text.Encoding.UTF8.GetBytes(json);

        www = new WWW("http://5.45.69.185:5000/box", formData, postHeader);
        StartCoroutine(WaitForRequest(www));

        Debug.Log("json: " + json);
    }
    #endregion

    #region boosters
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
    #endregion

    #region Levels
    public class LevelTrackingData {
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
        levelData.levelNum = levelNum;
        levelData.levelResult = "Completed";
        levelData.numberOfShots = shotsCount.ToString(); // TODO

        string json = JsonUtility.ToJson(levelData);

        WWW www;
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");

        // convert json string to byte
        var formData = System.Text.Encoding.UTF8.GetBytes(json);

        www = new WWW("http://5.45.69.185:5000/level", formData, postHeader);
        StartCoroutine(WaitForRequest(www));

        Debug.Log("json: " + json);
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

        www = new WWW("http://5.45.69.185:5000/level", formData, postHeader);
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

        www = new WWW("http://5.45.69.185:5000/level", formData, postHeader);
        StartCoroutine(WaitForRequest(www));
    }
    #endregion
}
