using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class DataController {
    private static string levelsFileName = "levels.json";

    public static List<LevelData> LoadLevels() {
        List<LevelData> lvlsData = new List<LevelData>();
        string filePath = Path.Combine(Application.streamingAssetsPath, levelsFileName);
        if (File.Exists(filePath)) {
            string jsonData = File.ReadAllText(filePath);
            string separ = "Level_?";
            string[] lvls = System.Text.RegularExpressions.Regex.Split(jsonData, separ);

            for (int i = 1; i < lvls.Length; i++) {

                string editedLvl = "{ \"Rows" + lvls[i].Substring(1, lvls[i].Length - 7);
                if (editedLvl.LastIndexOf("]") == editedLvl.Length-1) {
                    editedLvl += "}";
                }
                else {
                    editedLvl += "]}";
                }
                Debug.Log("editedLvl " + editedLvl);
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
        else 
        {
            Debug.LogError("Cannot find the file " + filePath);
            return lvlsData;
        }
    }
}

[System.Serializable]
public class LevelData {
    //public int levelNumber;
    //public int levelStars;
    //public bool copleted; 
    public int emptyRowsCount;
    public List<RowData> rows = new List<RowData>();
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

