using UnityEngine;
using UnityEngine.UI;

public class WaitForBoxPopup : MonoBehaviour {
    public Text timerText;

    void Start () {
        Debug.Log("WaitForBoxPopup");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void OnClick_Close() {
        Debug.Log("WaitForBoxPopup. Close");
        Destroy(this.gameObject); // kill self // yes there is a timeer 0.2f, it might fix some thigns on the mobile side, not sure, remove when there is a propper fade
    }
}
