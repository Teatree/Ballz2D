using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using admob;

public class AdmobController : MonoBehaviour {

    Admob ad;
    string appID = "ca-app-pub-4809397092315700~2101070495";
    string bannerID = "ca-app-pub-4809397092315700/7133905324";
    string interstitialID = "ca-app-pub-4809397092315700/3492498628";
    string videoID = "ca-app-pub-4809397092315700/9646843432";

    void Start() {
        Debug.Log("start unity demo-------------");
        initAdmob();
        if (ad.isInterstitialReady()) {
            ad.showInterstitial();
        }
        else {
            ad.loadInterstitial(interstitialID);
        }
        //  Admob.Instance().showBannerRelative(bannerID, AdSize.BANNER, AdPosition.TOP_CENTER);
    }

    void initAdmob() {
#if UNITY_IOS
        		 appID="ca-app-pub-3940256099942544~1458002511";
				 bannerID="ca-app-pub-3940256099942544/2934735716";
				 interstitialID="ca-app-pub-3940256099942544/4411468910";
				 videoID="ca-app-pub-3940256099942544/1712485313";
				 nativeBannerID = "ca-app-pub-3940256099942544/3986624511";
#elif UNITY_ANDROID
        appID = "ca-app-pub-4809397092315700~2101070495";
        bannerID = "ca-app-pub-4809397092315700/7133905324";
        interstitialID = "ca-app-pub-4809397092315700/3492498628";
        videoID = "ca-app-pub-4809397092315700/9646843432";
#endif
        AdProperties adProperties = new AdProperties();
        adProperties.isTesting = true;

        Admob.Instance().initSDK(appID, adProperties);
        //ad = Admob.Instance();
        //ad.bannerEventHandler += onBannerEvent;
        //ad.initSDK(appID, adProperties);//reqired,adProperties can been null
    }

    void onBannerEvent(string eventName, string msg) {
        Debug.Log("handler onAdmobBannerEvent---" + eventName + "   " + msg);
    }
}
