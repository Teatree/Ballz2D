﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DataController: MonoBehaviour {
    private static string levelsFileName = "levels.json";
    public static List<LevelData> allLevels = new List<LevelData>();
   
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

            List<LevelData> lvlsData = new List<LevelData>();
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
}

public class CellData {
    public string type;
    public int life;

    public CellData(string stringVal) {
        if (stringVal.Length > 2) {
            Debug.Log("stringVal: " + stringVal);
            type = stringVal.Substring(0, 2);
            life = int.Parse(stringVal.Substring(2, stringVal.Length - 2));
            Debug.Log("life: "  + life );
            Debug.Log("type: " + type);
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

