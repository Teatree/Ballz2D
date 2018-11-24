using UnityEngine;
using System.Collections;
using System.IO;

public class DataController: MonoBehaviour {
    private static string levelsFileName = "levels.json";
    public static ArrayList allLevels = new ArrayList();
    // Use this for initialization
    void Start() {
        LoadLevels();
    }

    // Update is called once per frame
    void Update() {

    }

    public static void LoadLevels() {
        string filePath = Path.Combine(Application.dataPath, levelsFileName);
        if (File.Exists(filePath)) {
            string jsonData = File.ReadAllText(filePath);
            string separ = "Level_?";
            string[] lvls = System.Text.RegularExpressions.Regex.Split(jsonData, separ);

            ArrayList lvlsData = new ArrayList();
            for (int i = 1; i < lvls.Length; i++) {

                string editedLvl = "{ \"Rows" + lvls[1].Substring(1, lvls[1].Length - 7) + "}";
                Debug.Log(">>>>  " + editedLvl);
                RowData[] rows = JsonHelper.FromJson<RowData>(editedLvl);
                LevelData lvlData = new LevelData();
                lvlData.rows = rows;
                lvlsData.Add(lvlData);
            }
            
            Debug.Log(">>>>  " + lvlsData.ToString());
            allLevels = lvlsData;
        } else {
            Debug.LogError("Cannot find the file " + filePath);
        }
    }
}

[System.Serializable]
public class LevelData {
    //public int levelNumber;
    //public int levelStars;
    //public bool copleted; 
    public RowData[] rows;
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

    public ArrayList GetCells() {
        ArrayList cells = new ArrayList();
        cells.Add(new CellData(col1));
        return cells;
    }

    public class CellData {
        public string type;
        public int life; 

        public CellData (string stringVal) {
            if (stringVal.Length > 2) {
                type = stringVal.Substring(0, 2);
                type = stringVal.Substring(2, stringVal.Length-1);
            } else {
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

