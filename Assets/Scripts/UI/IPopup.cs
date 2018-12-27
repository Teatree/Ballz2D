using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPopup<T> : SceneSingleton<IPopup<T>> {

    public virtual void OnClick_Close() {
        Destroy(gameObject); // kill self
    }

    public void OnClick_Home() {
        GameUIController.Instance.HandleHomeButton();
    }

    public void OnClick_AdsForGems() {
        AdmobController.Instance.ShowGemsRevardVideo();
    }
}
