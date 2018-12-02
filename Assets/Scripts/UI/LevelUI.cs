using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour {

    public int LevelNumber;
    public bool IsCompleted;
    public int Stars;

    [Header("Shit")]
    public Text levelNumText;

    private void Start() {
        levelNumText.text = (LevelNumber+1).ToString();
    }

    public void StartGameAtLevel() {
        AllLevelsData.CurrentLevelIndex = LevelNumber;

        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
}
