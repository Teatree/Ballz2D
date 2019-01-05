using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPopup<T> : SceneSingleton<IPopup<T>> {

    public virtual void OnClick_Close() {
        Destroy(gameObject, 0.2f); // kill self // yes there is a timeer 0.2f, it might fix some thigns on the mobile side, not sure, remove when there is a propper fade
    }

    public void OnClick_Home() {
        GameUIController.Instance.HandleHomeButton();
    }

    public void OnClick_AdsForGems() {
        AdmobController.Instance.ShowGemsRevardVideo();
    }
}
