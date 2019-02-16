using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : SceneSingleton<UIController> {

    public Transform LevelListParent;

    [SerializeField]
    private LevelUI levelUiElementPrefab;
    public GameObject arrowPrefab;

    public Text gems;
    public Text stars;

    public SpriteState sprState = new SpriteState();

    [Header("Popups")]
    public GameObject SettingsPrefab;
    public GameObject ShopPrefab;
    public GameObject BoxPopupPrefab;

    //Daily boxes
    public GameObject waitForItPrefab;
    public GameObject BoxDayButton;
    public GameObject BoxDayWaitButton;

    public GameObject BoxAdButton;
    private Button boxAdButtonCmp;
    public Scrollbar lvlSlider;

    void Start() {
        Debug.Log(">>>> Menu.initScene > " + SceneController.initScene);
        if (SceneController.initScene == "GameScene") {
            SceneController.initScene = "";
            SceneController.sceneController.UnloadMenu();
            SceneController.sceneController.LoadGame();
        }
        else {
            //Load data
            AllLevelsData.allLevels = DataController.LoadLevels();

            // debugText.text = "allLevels: " + AllLevelsData.allLevels.Count + "\n path: " + DataController.levelfilePath + "\n jsonDataExtracted: " + DataController.AjsonData;

            //Create level buttons
            
            if (PlayerController.player != null) {
                int arrowIndex = 0;
                int lvlIndex = 4;
                for (int i = 0; i < AllLevelsData.allLevels.Count; i++) {
                    var lvl = Instantiate(levelUiElementPrefab, LevelListParent);
                    if (i % 8 == 4) {
                        lvlIndex = 5;
                    }
                    if (i % 8 < 4) {
                        lvl.LevelNumber = i;
                        lvl.StarsNumber = (i < PlayerController.starsPerLvl.Count) ? PlayerController.starsPerLvl[i] : 0;
                        lvl.buttonComponent.interactable = i <= PlayerController.starsPerLvl.Count ? true : false;
                        lvl.UpdateButtonVisuals(lvl.StarsNumber);
                    } else {
                        lvlIndex-=2;
                        Debug.Log(">>> i + lvlIndex > " + i +" > " + lvlIndex);
                        lvl.LevelNumber = i + lvlIndex;
                        lvl.StarsNumber = (i + lvlIndex < PlayerController.starsPerLvl.Count) ? PlayerController.starsPerLvl[i+ lvlIndex] : 0;
                        lvl.buttonComponent.interactable = i + lvlIndex <= PlayerController.starsPerLvl.Count ? true : false;
                        lvl.UpdateButtonVisuals(lvl.StarsNumber);         
                    }
                    var arrow = Instantiate(arrowPrefab, lvl.transform);
                    arrow.transform.localScale = new Vector3(15, 15, 1);

                    if (arrowIndex % 8 == 0 || arrowIndex % 8 == 1 || arrowIndex % 8 == 2) {
                        arrow.transform.position = new Vector3(-45, lvl.transform.position.y);
                        arrow.transform.localRotation = Quaternion.Euler(0, 0, 180);
                    } else 
                    if (arrowIndex % 8 == 3 || arrowIndex % 8 == 4) {
                        arrow.transform.position = new Vector3(10, lvl.transform.position.y + 55);
                        arrow.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    } else {
                        arrow.transform.position = new Vector3(65, lvl.transform.position.y);
                    }
                    arrowIndex++;
                    lvlSlider.value = 0;
                }
            }

            stars.text = PlayerController.player != null ? PlayerController.player.GetStarsAmount().ToString() : "0";
        }
    }

    void Update() {
        gems.text = PlayerController.player != null ? PlayerController.player.gems.ToString() : "0";

        // gems.text = "Gems >>> " +  PlayerController.player.gems;
        //debugText.text = "allLevels: " + AllLevelsData.allLevels.Count + "      path: " + DataController.levelfilePath;
    }

    public void getGems() {
        // AdmobController.Instance.ShowGemsrewardVideo();
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

    public void GetDayBox() {
        DayBoxTimer.Instance.SetCountDownTo(DateTime.Now.AddHours(6));
        PlayerController.player.giveBoxAt = DateTime.Now.AddHours(6).ToString("yyyy-MM-dd HH:mm");
        OpenBoxOpen();
    }
    public void SetEnabledAdBox(bool b) {
        boxAdButtonCmp = BoxAdButton.GetComponent<Button>();
        boxAdButtonCmp.interactable = b;
    }

    public void ShowAdBox() {
        AdmobController.Instance.ShowBoxrewardVideo();
    }

    public void showWaitForItPopup() {
        Debug.Log(">>>> Wait for it");
        Instantiate(waitForItPrefab, transform);
    }

    public void ShowDayBoxButton() {
        BoxDayButton.SetActive(true);
        BoxDayWaitButton.SetActive(false);
    }

    public void ShowDayBoxWaitButton() {
        BoxDayButton.SetActive(false);
        BoxDayWaitButton.SetActive(true);
    }

}
