using UnityEngine;

public class LevelController : SceneSingleton<LevelController> {

    public static int blockScoreMin = 20;
    private static int blockScoreCoefficientStep = 10;

    public static int ThirdStarScore = 9;

    public static int LevelStarsAmount;
    public static int StarAmount;
    public static int levelScore;
    public static int blockscore = blockScoreMin ;

    public LevelData currentLevel;
    public static string SpecialBall;

    public static bool isGameOver; 
    public static bool isGamePaused;

    void Awake() {
        currentLevel = AllLevelsData.GetCurrentLevel();
        isGameOver = false;
        isGamePaused = false;
        Debug.Log(">>>> lvl > " + AllLevelsData.GetCurrentLevel());

        ResumeGame();

        ThirdStarScore = currentLevel.GetBlocksAmount() * 85; // this is where the fuckyou were doing that
    }

    public static int IncreseScore() {
        blockscore += blockScoreCoefficientStep;

        levelScore += blockscore;

        return blockscore;
    }

    public static void ResetScoreCoefficient() {
        blockscore = blockScoreMin;
    }

    public static void ResetScore() {
        blockscore = blockScoreMin;
        levelScore = 0;
    }

    public static void PauseGame() {
        isGamePaused = true;
        //Debug.Log("! Game is paused");
    }

    public static void ResumeGame() {
        isGamePaused = false;
        //Debug.Log("! Game is not paused");
    }

    public static bool IsGameStopped() {
        return isGameOver || isGamePaused;
    }
}