using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Preview : IPopup<Preview> {

    public Text LevelText;

	void Start () {
        LevelText.text = "Level: " + (AllLevelsData.CurrentLevelIndex+1).ToString();
    }
	
	void Update () {
		
	}

    
    public override void OnClick_Close() {
        GameController.ResumeGame();
        base.OnClick_Close();
    } 

}
