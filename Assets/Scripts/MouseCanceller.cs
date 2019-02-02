using UnityEngine;

public class MouseCanceller : MonoBehaviour {

    void OnMouseOver() {
        //If your mouse hovers over the GameObject with the script attached, output this message
        Input.ResetInputAxes();
        BallLauncher.Instance.HideGhosts();
    }
}