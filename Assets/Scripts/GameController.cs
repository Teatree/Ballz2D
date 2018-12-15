using UnityEngine;

public class GameController : MonoBehaviour {

    public static int levelScoreCoefficientMin = 10;
    public static int levelScoreCoefficientStep = 10;

    public static int levelScore;
    public static int levelScoreCoefficient = levelScoreCoefficientMin ;

    public LevelData currentLevel;

    public static bool isGameOver; 
    public static bool isGamePaused; 

    void Awake() {
        currentLevel = AllLevelsData.GetCurrentLevel();
    }

    public static void IncreseScore() {       
        levelScore += levelScoreCoefficient;
        levelScoreCoefficient += levelScoreCoefficientStep;
    }

    public static void ResetScoreCoefficient() {
        levelScoreCoefficient = levelScoreCoefficientMin;
    }

    public static void ResetScore() {
        levelScoreCoefficient = levelScoreCoefficientMin;
        levelScore = 0;
    }

    public static void GameOver() {
        isGameOver = true;
        Debug.Log("!!! The game is over");
    }

    public static void PauseGame() {
        isGamePaused = true;
        Debug.Log("!!! The game is over");
    }

    public static void GameWarning() {
        Debug.Log(">>> You're going to loose");
    }

    public static bool IsGameStopped() {
        return isGameOver || isGamePaused;
    }
}