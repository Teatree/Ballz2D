using UnityEngine;

public class GameController : MonoBehaviour {

    public static int levelScoreCoefficientMin = 10;
    public static int levelScoreCoefficientStep = 10;

    public static int levelScore;
    public static int levelScoreCoefficient = levelScoreCoefficientMin ;

    public LevelData currentLevel;

    public static bool isGameOver; 

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
}