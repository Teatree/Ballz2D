﻿using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Transform LevelListParent;

    [SerializeField]
    private LevelUI levelUiElementPrefab;


    public Text gems; 

    public SpriteState sprState = new SpriteState();

    [Header("Popups")]
    public GameObject SettingsPrefab;

    void Start() {
        //Load data
        AllLevelsData.allLevels = DataController.LoadLevels();

        // debugText.text = "allLevels: " + AllLevelsData.allLevels.Count + "\n path: " + DataController.levelfilePath + "\n jsonDataExtracted: " + DataController.AjsonData;
        //Create level buttons
        for (int i = 0; i < AllLevelsData.allLevels.Count; i++) {
            var lvl = Instantiate(levelUiElementPrefab);
            lvl.LevelNumber = i;
            lvl.transform.parent = LevelListParent.transform;
            if (i <= PlayerController.player.starsPerLvl.Count) {
                lvl.buttonComponent.interactable = true;
                lvl.StarsNumber = (i < PlayerController.player.starsPerLvl.Count) ? PlayerController.player.starsPerLvl[i] : 0;
            }
        }
    }

    void Update() {
       // gems.text = "Gems >>> " +  PlayerController.player.gems;
        //debugText.text = "allLevels: " + AllLevelsData.allLevels.Count + "      path: " + DataController.levelfilePath;
    }

    public void getGems() {
       // AdmobController.admob.ShowGemsRevardVideo();
    }

    public void OpenSettings() {
        Instantiate(SettingsPrefab, transform);
    }
}
