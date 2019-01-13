using System.Collections.Generic;

public static class AllLevelsData {
    public static List<LevelData> allLevels = new List<LevelData>();


    public static int CurrentLevelIndex = 0;

    public static LevelData GetCurrentLevel() {
        if (allLevels.Count > 0) {
            return allLevels[CurrentLevelIndex];
        } else {
            allLevels = DataController.LoadLevels();
            return allLevels[CurrentLevelIndex];
        }
    }
}
