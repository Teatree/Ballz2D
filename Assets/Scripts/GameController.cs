using UnityEngine;

public class GameController : MonoBehaviour {

    public static int levelScoreCoefficientMin = 10;
    public static int levelScoreCoefficientStep = 10;

    public static int levelScore;
    public static int levelScoreCoefficient = levelScoreCoefficientMin;

    //public int currentLevelIndex;
    public LevelData currentLevel;
    //public List<LevelData> allLevels = new List<LevelData>();

    // Use this for initialization
    void Awake() {
        //allLevels = DataController.LoadLevels();
        currentLevel = AllLevelsData.GetCurrentLevel();
    }

    public static void IncreseScore() {       
        levelScore += levelScoreCoefficient;
        levelScoreCoefficient += levelScoreCoefficientStep;
        Debug.Log(">>> " + levelScore);
    }

    public static void ResetScoreCoefficient() {
        levelScoreCoefficient = levelScoreCoefficientMin;
    }

    public static void ResetScore() {
        levelScoreCoefficient = levelScoreCoefficientMin;
        levelScore = 0;
    }
}