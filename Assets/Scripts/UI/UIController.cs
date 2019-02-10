using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : SceneSingleton<UIController> {

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

    //Daily boxes
    public GameObject waitForItPrefab;
    public GameObject BoxDayButton;
    public GameObject BoxDayWaitButton;

    public GameObject BoxAdButton;
    private Button boxAdButtonCmp; 
    public Scrollbar lvlSlider;

    void Start() {
        //Load data
        AllLevelsData.allLevels = DataController.LoadLevels();

        // debugText.text = "allLevels: " + AllLevelsData.allLevels.Count + "\n path: " + DataController.levelfilePath + "\n jsonDataExtracted: " + DataController.AjsonData;

        //Create level buttons
        if (PlayerController.player != null) {
            for (int i = 0; i < AllLevelsData.allLevels.Count; i++) {
                var lvl = Instantiate(levelUiElementPrefab, LevelListParent);
                lvl.LevelNumber = i;
       
                lvl.StarsNumber = (i < PlayerController.starsPerLvl.Count) ? PlayerController.starsPerLvl[i] : 0;
                lvl.buttonComponent.interactable = i <= PlayerController.starsPerLvl.Count ? true : false;
                lvl.UpdateButtonVisuals(lvl.StarsNumber);

                lvlSlider.value = 0;
            }
        }

        stars.text = PlayerController.player != null ? PlayerController.player.GetStarsAmount().ToString() : "0";
    }

    void Update() {
        gems.text = PlayerController.player != null ?  PlayerController.player.gems.ToString() : "0";

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
