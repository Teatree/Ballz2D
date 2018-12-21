﻿using UnityEngine;

public class GameController : MonoBehaviour {

    public static int levelScoreCoefficientMin = 20;

    public static int SecondStarScore = 13000;
    public static int ThirdStarScore = 25000;

    public static int levelScore;
    public static int levelScoreCoefficient = levelScoreCoefficientMin ;

    public LevelData currentLevel;
    public static int Gems = 10000;

    public static bool isGameOver; 
    public static bool isGamePaused;

    void Awake() {
        currentLevel = AllLevelsData.GetCurrentLevel();
    }

    public static int IncreseScore() {
        int scoreEred = levelScoreCoefficient;
        levelScore += levelScoreCoefficient;

        return scoreEred;
    }

    public static void ResetScoreCoefficient() {
        levelScoreCoefficient = levelScoreCoefficientMin;
    }

    public static void ResetScore() {
        levelScoreCoefficient = levelScoreCoefficientMin;
        levelScore = 0;
    }

    //public static void GameOver() {
    //    isGameOver = true;
    //    Debug.Log("!!! The game is over");
    //}

    public static void PauseGame() {
        isGamePaused = true;
        Debug.Log("! Game is paused");
    }

    public static void ResumeGame() {
        isGamePaused = false;
        Debug.Log("! Game is not paused");
    }

    public static bool IsGameStopped() {
        return isGameOver || isGamePaused;
    }
}