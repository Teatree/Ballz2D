using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class DataController {
    private static string levelsFileName = "levels.json";
    private static string playerFileName = "playerInfo.json";

    public static string AjsonData;
    private const string CASHE_FOLDER = "LocalStorage";

#if UNITY_EDITOR
    public static string levelfilePath = Path.Combine(Application.dataPath + "/StreamingAssets", levelsFileName);
    public static string playerfilePath = Path.Combine(Application.dataPath + "/StreamingAssets", playerFileName);

#elif UNITY_IOS
    private static string levelfilePath = Path.Combine (Application.persistentDataPath  + "/Raw", levelsFileName);
    private static string playerfilePath = Path.Combine (Application.persistentDataPath  + "/Raw", playerFileName);
 
#elif UNITY_ANDROID
    public static string levelfilePath = Path.Combine (Application.streamingAssetsPath + "/", levelsFileName);
    public static string playerfilePath = Path.Combine (Application.streamingAssetsPath + "/", playerFileName);
    //public static string levelfilePath = Path.Combine ("jar:file://" + Application.streamingAssetsPath  + "!/assets/", levelsFileName);
    //public static string playerfilePath = Path.Combine ("jar:file://" + Application.streamingAssetsPath  + "!/assets/", playerFileName);
 
#endif

    //--------- Player Info ----------
    public static PlayerInfo LoadPlayer() {
        if (File.Exists(playerfilePath)) {
            string jsonData = File.ReadAllText(playerfilePath);
            PlayerInfo pi = JsonUtility.FromJson<PlayerInfo>(jsonData);
            //Debug.Log(">>> PlayerInfo > " + pi);
            return pi;
        }
        else {
            Debug.LogError("Cannot find the file " + playerfilePath);
            return new PlayerInfo();
        }
    }

    protected static string ReadString(string fileName) {
        string path = Path.Combine("jar:file://" + Application.dataPath + "!/assets/", fileName);

        if (!File.Exists(path)) {
            Debug.LogError("Cannot find the file " + levelfilePath);
            AjsonData = "<color=#a52a2aff>CANNOT FIND PAAAATH: " + path + "</color>";
            return null;
        }
        AjsonData = "<color=#a52a2aff>ERROR PATH:  " + path + "</color>";
        using (var stream = new StreamReader(path)) {
            string result = stream.ReadToEnd();
            stream.Close();
            return result;
        }
    }

    public static void SavePlayer(PlayerInfo pi) {
        string jsonData = JsonUtility.ToJson(pi);
        File.WriteAllText(playerfilePath, jsonData);
    }

    //------------Load Levels --------------------
    public static List<LevelData> LoadLevels() {
        List<LevelData> lvlsData = new List<LevelData>();
        string jsonData = "";

#if UNITY_EDITOR || UNITY_IOS
        if (File.Exists(levelfilePath)) {
            jsonData = File.ReadAllText(levelfilePath);
        }
#elif UNITY_ANDROID
            //WWW reader = new WWW(ReadString);
            //while (!reader.isDone) {
            //}
            jsonData = ReadString(levelsFileName);
#endif
        if (jsonData != null) {
            string separ = "Level_?";
            string[] lvls = System.Text.RegularExpressions.Regex.Split(jsonData, separ);

            for (int i = 1; i < lvls.Length; i++) {

                string editedLvl = "{ \"Rows" + lvls[i].Substring(1, lvls[i].Length - 7);
                if (editedLvl.LastIndexOf("]") == editedLvl.Length - 1) {
                    editedLvl += "}";
                }
                else {
                    editedLvl += "]}";
                }
                RowData[] rows = JsonHelper.FromJson<RowData>(editedLvl);
                LevelData lvlData = new LevelData();
                System.Array.Reverse(rows);

                foreach (RowData row in rows) {
                    if (!row.IsEmpty()) {
                        lvlData.rows.Add(row);
                    }
                }

                foreach (RowData row in rows) {
                    if (row.IsEmpty()) {
                        lvlData.emptyRowsCount++;
                    }
                    else {
                        break;
                    }
                }

                lvlsData.Add(lvlData);
            }
            return lvlsData;
        }
        else {
            //Debug.LogError("Cannot find the file " + levelfilePath);
            //AjsonData = "<color=#a52a2aff>JSON IS NULL</color>";
            return lvlsData;
        }
    }

}

[System.Serializable]
public class PlayerInfo {
    public List<int> starsPerLvl = new List<int>();
}

[System.Serializable]
public class LevelData {
    public int emptyRowsCount;
    public List<RowData> rows = new List<RowData>();

    public int GetBlocksAmount() {
        int amount = 0;
        for (int i = 0; i < rows.Count; i++) {
            List<CellData> cells = rows[i].GetCells();
            for (int j = 0; j < cells.Count; j++) {
                if (cells[j] != null && cells[j].isCollidableBlock()) {
                    amount++;
                }
            }
        }
        return amount;
    }
}

[System.Serializable]
public class RowData {
    public string o;
    public string col1;
    public string col2;
    public string col3;
    public string col4;
    public string col5;
    public string col6;
    public string col7;
    public string col8;
    public string col9;
    public string col10;
    public string col11;
    public string col12;
    public string col13;
    public string col14;
    public string col15;

    public List<CellData> GetCells() {
        List<CellData> cells = new List<CellData>();
        cells.Add(new CellData(col2));
        cells.Add(new CellData(col3));
        cells.Add(new CellData(col4));
        cells.Add(new CellData(col5));
        cells.Add(new CellData(col6));
        cells.Add(new CellData(col7));
        cells.Add(new CellData(col8));
        cells.Add(new CellData(col9));
        cells.Add(new CellData(col10));
        cells.Add(new CellData(col11));
        cells.Add(new CellData(col12));
        cells.Add(new CellData(col13));
        cells.Add(new CellData(col14));

        return cells;
    }

    public bool IsEmpty() {
        return col2 != null && (col2 == "" || col2 == "x") &&
                col3 != null && (col3 == "" || col3 == "x") &&
                col4 != null && (col4 == "" || col4 == "x") &&
                col5 != null && (col5 == "" || col5 == "x") &&
                col6 != null && (col6 == "" || col6 == "x") &&
                col7 != null && (col7 == "" || col7 == "x") &&
                col8 != null && (col8 == "" || col8 == "x") &&
                col9 != null && (col9 == "" || col9 == "x") &&
                col10 != null && (col10 == "" || col10 == "x") &&
                col11 != null && (col11 == "" || col11 == "x") &&
                col12 != null && (col12 == "" || col12 == "x") &&
                col13 != null && (col13 == "" || col13 == "x") &&
                col14 != null && (col14 == "" || col14 == "x");
    }
}

public class CellData {
    public string type;
    public int life;

    public CellData(string stringVal) {
        if (stringVal.Length > 2) {
            type = stringVal.Substring(0, 2);
            life = int.Parse(stringVal.Substring(2, stringVal.Length - 2));
        }
        else {
            type = stringVal;
        }
    }

    public bool isCollidableBlock() {
        return type != null && type != "" && type != "LV" && type != "LH" && type != "LC" && type != "FF" && type != "★★" && type != "os";
    }
}

public static class JsonHelper {
    public static T[] FromJson<T>(string json) {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Rows;
    }

    public static string ToJson<T>(T[] array) {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Rows = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint) {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Rows = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T> {
        public T[] Rows;
    }
}
