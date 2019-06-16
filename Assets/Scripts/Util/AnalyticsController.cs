using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;

public class AnalyticsController : SceneSingleton<AnalyticsController> {

    // Use this for initialization
    void Awake() {

        //   StartCoroutine(Upload());
        SomeData someData = new SomeData();
        someData.o = "wut?";

        string json = JsonUtility.ToJson(someData);

        WWW www;
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");

        // convert json string to byte
        var formData = System.Text.Encoding.UTF8.GetBytes(json);

        www = new WWW("http://5.45.69.185:5000/analytics", formData, postHeader);
        StartCoroutine(WaitForRequest(www));

        Debug.Log("json: " + json);

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

    public class SomeData {
        public string o = "HELLO FROM UNITY";
    }

    //IEnumerator Upload() {
    //    SomeData someData = new SomeData();
    //    someData.o = "wut?";

    //    string json = JsonUtility.ToJson(someData);

        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));

   

        //UnityWebRequest www = UnityWebRequest.Post("http://5.45.69.185:5000/analytics", json);
        //yield return www.SendWebRequest();

        //if (www.isNetworkError || www.isHttpError) {
        //    Debug.Log(www.error);
        //}
        //else {
        //    Debug.Log("Form upload complete!");
        //}
    //}

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
