using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class DataController
{
    private static string levelsFileName = "levels.json";
    private static string itemsFileName = "items.json";
    private static string playerFileName = "playerInfo.json";

    public static string AjsonData;
    private const string CASHE_FOLDER = "LocalStorage";


#if UNITY_EDITOR
    public static string levelfilePathLocal = Path.Combine(Application.dataPath + "/StreamingAssets", levelsFileName);
    public static string levelfilePath = "https://raw.githubusercontent.com/fearlessdoodlez/balls/master/level_online.json";
    public static string itemsfilePath = Path.Combine(Application.dataPath + "/StreamingAssets", itemsFileName);
    //public static string playerfilePath = Path.Combine(Application.dataPath + "/StreamingAssets", playerFileName);
    public static string playerfilePath = Path.Combine(Application.persistentDataPath, playerFileName);

#elif UNITY_ANDROID
    public static string levelfilePathLocal = Path.Combine ( Application.persistentDataPath, levelsFileName);
    public static string levelfilePath = "https://raw.githubusercontent.com/fearlessdoodlez/balls/master/level_online.json";
    public static string itemsfilePath = Path.Combine ("jar:file://" + Application.dataPath + "!/assets/", itemsFileName);
    public static string playerfilePath = Path.Combine ( Application.persistentDataPath,  playerFileName);

#elif UNITY_IOS
    //private static string levelfilePath = Path.Combine (Application.f50persistentDataPath  + "/Raw", levelsFileName);
    private static string levelfilePath = "https://raw.githubusercontent.com/fearlessdoodlez/balls/master/level_online.json";
    private static string itemsfilePath = Path.Combine (Application.persistentDataPath  + "/Raw", itemsFileName);
    private static string playerfilePath = Path.Combine (Application.persistentDataPath  + "/Raw", playerFileName);
 
 
#endif
    
    //--------- Player Info ----------
    public static PlayerData LoadPlayer()
    {
        string jsonData = "";
        //if (Application.platform == RuntimePlatform.Android) {
        PlayerData pi;

        if (!File.Exists(playerfilePath)) {
            TextAsset file = Resources.Load("playerInfo") as TextAsset;
            jsonData = file.ToString();
            pi = JsonUtility.FromJson<PlayerData>(jsonData);
            pi.PlayerID = PlayerController.GenerateID();
            pi.firstLoginAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //StreamReader f = new StreamReader(playerfilePath);
            //jsonData = f.ReadToEnd();
            //f.Close();

            
            //WWW reader = new WWW(playerfilePath);
            //while (!reader.isDone) { }
            //jsonData = reader.text;
        }
        else {
            jsonData = File.ReadAllText(playerfilePath);
            pi = JsonUtility.FromJson<PlayerData>(jsonData);
            if(pi.PlayerID == null || pi.PlayerID == "") {
                pi.PlayerID = PlayerController.GenerateID();
            }
            Debug.Log(">>>> jsonData > " + jsonData);

        }
        return pi;
        // PlayerData pi = JsonUtility.FromJson<PlayerData>(jsonData);
        //   AjsonData = "<color=#a52a2aff> " + jsonData + "</color>";
    }

    public static void SavePlayer(PlayerData pi)
    {
        string jsonData = JsonUtility.ToJson(pi);
        // Debug.Log(">>> player info > " + jsonData);
        // if (Application.platform == RuntimePlatform.Android) {
        StreamWriter writer = new StreamWriter(playerfilePath, false);
        writer.WriteLine(jsonData);
        writer.Close();
        //} else {
        //    File.WriteAllText(playerfilePath, jsonData);
        //} 
    }

    //------------Items ---------------------------
    public static List<ItemData> LoadItems()
    {
        string jsonData = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW reader = new WWW(itemsfilePath);
            while (!reader.isDone) { }
            jsonData = reader.text;
        }
        else
        {
            jsonData = File.ReadAllText(itemsfilePath);
        }
        ItemData[] id = JsonHelper.FromJson<ItemData>(jsonData);
        return new List<ItemData>(id);
    }

    //------------Load Levels --------------------
    public static List<LevelData> LoadLevels()
    {
        List<LevelData> lvlsData = new List<LevelData>();
        string jsonData = "";
        bool isNetwork = Application.internetReachability != NetworkReachability.NotReachable;
        Debug.Log(" >>>> is NEtwork " + isNetwork );
        if (Application.platform == RuntimePlatform.Android) {
            if (isNetwork) {
                WWW reader = new WWW(levelfilePath);
                while (!reader.isDone) { }

                jsonData = reader.text;

                //Save the file
                StreamWriter writer = new StreamWriter(levelfilePathLocal, false);
                writer.WriteLine(jsonData);
                writer.Close();
            } else {
                if (!File.Exists(levelfilePathLocal)){
                    TextAsset file = Resources.Load("levels") as TextAsset;
                    jsonData = file.ToString();
                } else {
                    jsonData = File.ReadAllText(levelfilePathLocal);
                }
                //TextAsset file = Resources.Load("levels.json") as TextAsset;
                //jsonData = file.ToString();
                //WWW reader = new WWW(levelfilePathLocal);
                //while (!reader.isDone) { }
                //jsonData = reader.text;
            }
        }
        else {
            if (isNetwork) {

                WWW reader = new WWW(levelfilePath);
                while (!reader.isDone) { }
                jsonData = reader.text;

                Debug.Log(" >>>> levels jsonData " + jsonData);
                //Save the file
                StreamWriter writer = new StreamWriter(levelfilePathLocal, false);
                writer.WriteLine(jsonData);
                writer.Close();
            } else {
                jsonData =
               File.ReadAllText(levelfilePathLocal);
            }
        }


        if (jsonData != null)
        {
            string separ = "Level_[0-9][0-9][0-9]";
            string[] lvls = System.Text.RegularExpressions.Regex.Split(jsonData, separ);

            for (int i = 1; i < lvls.Length; i++)
            {

                string editedLvl = "{ \"Rows" + lvls[i].Substring(1, lvls[i].Length - 7);
                if (editedLvl.LastIndexOf("]") == editedLvl.Length - 1)
                {
                    editedLvl += "}";
                }
                else
                {
                    editedLvl += "]}";
                }
                RowData[] rows = JsonHelper.FromJson<RowData>(editedLvl);
                LevelData lvlData = new LevelData();
                System.Array.Reverse(rows);

                foreach (RowData row in rows)
                {
                    if (!row.IsEmpty())
                    {
                        lvlData.rows.Add(row);
                    }
                }

                foreach (RowData row in rows)
                {
                    if (row.IsEmpty())
                    {
                        lvlData.emptyRowsCount++;
                    }
                    else
                    {
                        break;
                    }
                }

                lvlsData.Add(lvlData);
            }
            return lvlsData;
        }
        else
        {
            //Debug.LogError("Cannot find the file " + levelfilePath);
            //AjsonData = "<color=#a52a2aff>JSON IS NULL</color>";
            return lvlsData;
        }
    }

}

[System.Serializable]
public class PlayerData
{
    public string PlayerID;
    public int gems;
    public int stars;
    public int progressTowardsNextStarBox; // how many stars player already gathered for the current star box
    public int numStarBoxesOpened; // how many boxs were already opened
    public string giveBoxAt;
    public string firstLoginAt;
    public bool noAds;
    public string specialBallImageName;
    public string specialBallName;
    public bool boughtStarter;
    public int adBoxOpenedCount;
    public string adBoxOpenedDate;
    public int MoreHCReviveCount;
    public string MoreHCReviveOpenedDate;
    public string lastSubDate;

    public List<CompletedLevel> completedLvls = new List<CompletedLevel>();
    public List<ItemData> items = new List<ItemData>();

    public int GetStarsAmount()
    {
        int sum = 0;
        foreach (CompletedLevel lvl in completedLvls)
        {
            sum += lvl.stars;
        }
        stars = sum;
        return sum;
    }
}

[System.Serializable]
public class ItemData
{
    public int costGems;
    public string name;
    public int amount;
    public bool enabled;

    public ItemData(ItemObject io)
    {
        this.costGems = io.costGems;
        this.name = io.name;
        this.amount = io.amount;
        this.enabled = io.enabled;
    }
    public ItemData()
    {

    }
}

[System.Serializable]
public class CompletedLevel
{
    public int number;
    public int stars;

    public CompletedLevel(int i, int j)
    {
        this.number = i;
        this.stars = j;
    }
}
[System.Serializable]
public class LevelData
{
    public int emptyRowsCount;
    public List<RowData> rows = new List<RowData>();

    public int GetBlocksAmount()
    {
        int amount = 0;
        for (int i = 0; i < rows.Count; i++)
        {
            List<CellData> cells = rows[i].GetCells();
            for (int j = 0; j < cells.Count; j++)
            {
                if (cells[j] != null && cells[j].isCollidableBlock())
                {
                    amount++;
                }
            }
        }
        return amount;
    }
}

[System.Serializable]
public class RowData
{
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

    public List<CellData> GetCells()
    {
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

    public bool IsEmpty()
    {
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

public class CellData
{
    public string type;
    public int life;

    public CellData(string stringVal)
    {
        if (stringVal.Length > 2)
        {
            type = stringVal.Substring(0, 2);
            life = int.Parse(stringVal.Substring(2, stringVal.Length - 2));
        }
        else
        {
            type = stringVal;
        }
    }

    public bool isCollidableBlock()
    {
        return type != null && type != "" && type != "LV" && type != "LH" && type != "LC" && type != "FF" && type != "★★" && type != "os";
    }
}


public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Rows;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Rows = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Rows = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Rows;
    }
}