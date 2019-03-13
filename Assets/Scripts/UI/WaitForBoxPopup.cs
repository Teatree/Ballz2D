using UnityEngine;
using UnityEngine.UI;

public class WaitForBoxPopup : IPopup<Pause> {
    public Text timerText;

    void Start () {
        Debug.Log("WaitForBoxPopup");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
