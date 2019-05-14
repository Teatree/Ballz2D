using UnityEngine;

public class ConfirmBallPopup :MonoBehaviour {

    public string result = "";

    public void OnClick_Close() {
        result = "no";
        gameObject.SetActive(false);
       Destroy(gameObject,2f); // kill self // yes there is a timeer 0.2f, it might fix some thigns on the mobile side, not sure, remove when there is a propper fade
    }

    public void OnClick_Ok() {
        result = "ok";
        gameObject.SetActive(false);
        Destroy(gameObject, 2f); // kill self // yes there is a timeer 0.2f, it might fix some thigns on the mobile side, not sure, remove when there is a propper fade
    }
}
