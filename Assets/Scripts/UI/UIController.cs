using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Transform LevelListParent;

    [SerializeField]
    private LevelUI levelUiElementPrefab;

    public GameObject TestPrefab;

    public Text debugText;

    public SpriteState sprState = new SpriteState();

    void Start() {
        //Load data
        AllLevelsData.allLevels = DataController.LoadLevels();
        AllLevelsData.playerInfo = DataController.LoadPlayer();

        Debug.Log("playerInfo > " + AllLevelsData.playerInfo.ToString());
        Debug.Log("allLevels > " + AllLevelsData.allLevels.ToString());

        // debugText.text = "allLevels: " + AllLevelsData.allLevels.Count + "\n path: " + DataController.levelfilePath + "\n jsonDataExtracted: " + DataController.AjsonData;
        //Create level buttons
        for (int i = 0; i < AllLevelsData.allLevels.Count; i++) {
            var lvl = Instantiate(levelUiElementPrefab);
            lvl.LevelNumber = i;
            lvl.transform.parent = LevelListParent.transform;
            if (i <= AllLevelsData.playerInfo.starsPerLvl.Count) {
                lvl.buttonComponent.interactable = true;
                lvl.StarsNumber = (i < AllLevelsData.playerInfo.starsPerLvl.Count) ? AllLevelsData.playerInfo.starsPerLvl[i] : 0;
            }
        }

        //for (int i = 0; i < AllLevelsData.allLevels.Count; i++) {
        //    GameObject lvl = (GameObject)Instantiate(TestPrefab.gameObject, LevelListParent.transform);
        //    RectTransform mainRT = lvl.GetComponent<RectTransform>();
        //    mainRT.SetParent(LevelListParent.transform);

        //    //lvl.LevelNumber = i;
        //    //lvl.transform.position = new Vector3(0,0,120);
        //    //lvl.transform.localScale = new Vector3(5,5,5);
        //    //lvl.GetComponent<Image>().color = Color.red;

        //    DontDestroyOnLoad(lvl);
        //    //if (i <= AllLevelsData.playerInfo.starsPerLvl.Count) {
        //    //    //lvl.buttonComponent.interactable = true;
        //    //    //lvl.StarsNumber = (i < AllLevelsData.playerInfo.starsPerLvl.Count) ? AllLevelsData.playerInfo.starsPerLvl[i] : 0;
        //    //}
        //}
    }

    void Update() {
        //debugText.text = "allLevels: " + AllLevelsData.allLevels.Count + "      path: " + DataController.levelfilePath;
    }
}
