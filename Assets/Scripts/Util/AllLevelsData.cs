using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AllLevelsData {
    public static List<LevelData> allLevels = new List<LevelData>();

    public static int CurrentLevelIndex;

    public static LevelData GetCurrentLevel() {
        return allLevels[CurrentLevelIndex];
    }
}
