using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPoup : IPopup<SettingsPoup> {

    public void RateTheGame() {
        #if UNITY_ANDROID
                Application.OpenURL("market://details?id=com.fd.etf.android");
        #elif UNITY_IPHONE
                        Application.OpenURL("itms-apps://itunes.apple.com/app/idYOUR_APP_ID");
        #endif
    }
}
