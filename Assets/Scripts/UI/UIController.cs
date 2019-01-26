using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Transform LevelListParent;

    [SerializeField]
    private LevelUI levelUiElementPrefab;

    public Text gems;
    public Text stars;

    public SpriteState sprState = new SpriteState();

    [Header("Popups")]
    public GameObject SettingsPrefab;
    public GameObject ShopPrefab;
    public GameObject BoxPopupPrefab;

    void Start() {
        //Load data
        AllLevelsData.allLevels = DataController.LoadLevels();

        // debugText.text = "allLevels: " + AllLevelsData.allLevels.Count + "\n path: " + DataController.levelfilePath + "\n jsonDataExtracted: " + DataController.AjsonData;
        //Create level buttons
        if (PlayerController.player != null) {
            for (int i = 0; i < AllLevelsData.allLevels.Count; i++) {
                var lvl = Instantiate(levelUiElementPrefab, LevelListParent);
                lvl.LevelNumber = i;
                //lvl.transform.parent = LevelListParent.transform;
                if (i <= PlayerController.starsPerLvl.Count) {
                    lvl.buttonComponent.interactable = true;
                    lvl.StarsNumber = (i < PlayerController.starsPerLvl.Count) ? PlayerController.starsPerLvl[i] : 0;
                }
            }
        }

        stars.text = PlayerController.player != null ? PlayerController.player.GetStarsAmount().ToString() : "0";    }

    void Update() {
        gems.text = PlayerController.player != null ?  PlayerController.player.gems.ToString() : "0";
        
        // gems.text = "Gems >>> " +  PlayerController.player.gems;
        //debugText.text = "allLevels: " + AllLevelsData.allLevels.Count + "      path: " + DataController.levelfilePath;
    }

    public void getGems() {
        AdmobController.Instance.ShowGemsRevardVideo();
    }

    public void Share() {
        ShareController.Instance.ShareScreenshotWithText("This game has balls! ");
    }

    public void OpenSettings() {
        Instantiate(SettingsPrefab, transform);
    }

    public void OpenShop() {
        Instantiate(ShopPrefab, transform);
    }

    public void OpenBoxOpen() {
        var Box = Instantiate(BoxPopupPrefab, transform);
        Box.GetComponent<BoxPopup>().itemToReceive = BoxOpener.Instance.GetBoxContents_BoxAd();
    }
}
