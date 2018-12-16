using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIController : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void Shop() {
        Debug.LogWarning("Shop!");
    }

    public void Pause() {
        Debug.LogWarning("Pause!");
        GameController.PauseGame();
    }
}
