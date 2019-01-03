using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Transform LevelListParent;

    [SerializeField]
    private LevelUI levelUiElementPrefab;


    public Text gems;

    public SpriteState sprState = new SpriteState();

    [Header("Popups")]
    public GameObject SettingsPrefab;
    public GameObject ShopPrefab;

    void Start() {
        //Load data
        AllLevelsData.allLevels = DataController.LoadLevels();

        // debugText.text = "allLevels: " + AllLevelsData.allLevels.Count + "\n path: " + DataController.levelfilePath + "\n jsonDataExtracted: " + DataController.AjsonData;
        //Create level buttons
        if (PlayerController.player != null) {
            for (int i = 0; i < AllLevelsData.allLevels.Count; i++) {
                var lvl = Instantiate(levelUiElementPrefab);
                lvl.LevelNumber = i;
                lvl.transform.parent = LevelListParent.transform;
                if (i <= PlayerController.Instance.starsPerLvl.Count) {
                    lvl.buttonComponent.interactable = true;
                    lvl.StarsNumber = (i < PlayerController.Instance.starsPerLvl.Count) ? PlayerController.Instance.starsPerLvl[i] : 0;
                }
            }
        }
    }

    void Update() {
        // gems.text = "Gems >>> " +  PlayerController.player.gems;
        //debugText.text = "allLevels: " + AllLevelsData.allLevels.Count + "      path: " + DataController.levelfilePath;
    }

    public void getGems() {
        AdmobController.Instance.ShowGemsRevardVideo();
    }

    public void Share() {
        ShareController.Instance.ShareScreenshotWithText("check out this balls");
    }

    public void OpenSettings() {
        Instantiate(SettingsPrefab, transform);
    }

    public void OpenShop() {
        Instantiate(ShopPrefab, transform);
    }
   
}
