using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmBallPopup : IPopup<ConfirmBallPopup> {

    public string result = "";

    public override void OnClick_Close() {
        result = "no";
        Destroy(gameObject, 0.2f); // kill self // yes there is a timeer 0.2f, it might fix some thigns on the mobile side, not sure, remove when there is a propper fade
    }

    public void OnClick_Ok() {
        result = "ok";
        Destroy(gameObject, 0.2f); // kill self // yes there is a timeer 0.2f, it might fix some thigns on the mobile side, not sure, remove when there is a propper fade
    }
}
