using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public int levelScore;

    //public int currentLevelIndex;
    public LevelData currentLevel;
    //public List<LevelData> allLevels = new List<LevelData>();

    // Use this for initialization
    void Awake() {
        //allLevels = DataController.LoadLevels();
        currentLevel = AllLevelsData.GetCurrentLevel();
    }
}