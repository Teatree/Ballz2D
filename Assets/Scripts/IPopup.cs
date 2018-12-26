using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPopup<T> : SceneSingleton<IPopup<T>> {

    public void OnClick_Close() {
        Destroy(gameObject); // kill self
    }
}
