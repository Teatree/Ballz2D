using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : SceneSingleton<UIController> {

    public Transform LevelListParent;

    [SerializeField]
    private LevelUI levelUiElementPrefab;
    public GameObject arrowPrefab;

    public Text gems;
    public Text starsText;

    public SpriteState sprState = new SpriteState();

    [Header("Popups")]
    public GameObject SettingsPrefab;
    public GameObject ShopPrefab;
    public GameObject BoxPopupPrefab;
    public GameObject waitForItPrefab;

    //Daily boxes
    public GameObject BoxDayButton;
    public GameObject BoxDayWaitButton;

    public GameObject BoxAdButton;
    public GameObject BoxAdArrow;

    private Button boxAdButtonCmp;
    public Scrollbar lvlSlider;

    string url = "https://cdn.shrm.org/image/upload/c_crop,h_365,w_851,x_0,y_0/w_auto:100:2100,q_auto,f_auto/v1/Compensation/pay-hands_qd26uc";
    Texture2D img;

    void Start() {
        DateTime dt = PlayerController.player == null || PlayerController.player.giveBoxAt == null || PlayerController.player.giveBoxAt == "" ? DateTime.Now : DateTime.Parse(PlayerController.player.giveBoxAt);

        if (dt < DateTime.Now) {
            // if Player came logged in after box was supposed to be claiemd
            ShowDayBoxButton();
        }
        else {
            ShowDayBoxWaitButton();
        }

        // Debug.Log(">>>> Menu.initScene > " + SceneController.initScene);
        if (SceneController.initScene == "GameScene") {
            SceneController.initScene = "";
            SceneController.sceneController.UnloadMenu();
            SceneController.sceneController.LoadGame();
        }
        else {
            // Load data
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
                    }
                    else {
                        lvlIndex -= 2;
                        lvl.LevelNumber = i + lvlIndex;
                        lvl.StarsNumber = (i + lvlIndex < PlayerController.starsPerLvl.Count) ? PlayerController.starsPerLvl[i + lvlIndex] : 0;
                        lvl.buttonComponent.interactable = i + lvlIndex <= PlayerController.starsPerLvl.Count ? true : false;
                        lvl.UpdateButtonVisuals(lvl.StarsNumber);
                    }
                    var arrow = Instantiate(arrowPrefab, lvl.transform);
                    arrow.transform.SetParent(lvl.transform);
                    // arrow.transform.localScale = new Vector3(15, 15, 1);

                    if (arrowIndex % 8 == 0 || arrowIndex % 8 == 1 || arrowIndex % 8 == 2) {
                        arrow.transform.localPosition = new Vector3(-85, 0);
                        arrow.transform.localRotation = Quaternion.Euler(0, 0, 180);
                    }
                    else
                    if (arrowIndex % 8 == 3 || arrowIndex % 8 == 4) {
                        arrow.transform.localPosition = new Vector3(0, 85);
                        arrow.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    }
                    else {
                        arrow.transform.localPosition = new Vector3(85, 0);
                    }
                    arrowIndex++;
                    lvlSlider.value = 0;
                }
            }

            SetStarsAmountText();
        }

        InitAdbox();

        //if (PlayerController.player.adBoxOpenedCount < UnityAddsController.Instance.AdBoxOpenLimit) {
        //    BoxAdArrow.SetActive(true);
        //    BoxAdArrow.GetComponent<Animation>().Play();
        //}

        //if (DateTime.Parse(PlayerController.player.giveBoxAt) < // next time for box){

        //    }
    }

    private void SetStarsAmountText() {
        int allStars = 0;
        if (PlayerController.starsPerLvl != null)
            foreach (int lvlInd in PlayerController.starsPerLvl.Keys) {
                allStars += PlayerController.starsPerLvl[lvlInd];
            }
        starsText.text = PlayerController.player != null ? "" + allStars : "0";
    }

    void Update() {
        gems.text = PlayerController.player != null ? PlayerController.player.gems.ToString() : "0";

        // gems.text = "Gems >>> " +  PlayerController.player.gems;
        //debugText.text = "allLevels: " + AllLevelsData.allLevels.Count + "      path: " + DataController.levelfilePath;

        if (PlayerController.player.adBoxOpenedCount < UnityAddsController.Instance.AdBoxOpenLimit) {
            SetEnabledAdBox(true);
        }
        else {
            SetEnabledAdBox(false);
        }
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

    public void OpenWaitPopup() {
        GameObject waitPopup = Instantiate(waitForItPrefab, transform);
        BoxDayWaitButton.transform.GetComponent<DailyBoxTimer>().popupTimer = waitPopup.GetComponent<WaitForBoxPopup>().timerText;
    }

    public void OpenShop() {
        Instantiate(ShopPrefab, transform);
    }

    public void OpenBoxOpen() {
        var Box = Instantiate(BoxPopupPrefab, transform);
        Box.GetComponent<BoxPopup>().itemToReceive = BoxOpener.Instance.GetBoxContents_BoxAd();

        AnalyticsController.Instance.LogBoxesOpenedEvent("Day / Ad Box", Box.GetComponent<BoxPopup>().itemToReceive.name, Box.GetComponent<BoxPopup>().itemToReceive.amount);
    }

    public void OpenDayBox() {

        OpenBoxOpen();
        ShowDayBoxWaitButton();

        DateTime dt = PlayerController.player == null || PlayerController.player.giveBoxAt == null || PlayerController.player.giveBoxAt == "" ? DateTime.Now : DateTime.Parse(PlayerController.player.giveBoxAt);
        NotificationController.Instance.ScheduleBoxNotification(dt);

    }

    public void SetEnabledAdBox(bool b) {
        boxAdButtonCmp = BoxAdButton.GetComponent<Button>();
        boxAdButtonCmp.interactable = b;
        if (b == false) {
            BoxAdArrow.SetActive(b);
        }
    }

    public void InitAdbox() {
        DateTime dt = PlayerController.player == null || PlayerController.player.adBoxOpenedDate == null || PlayerController.player.adBoxOpenedDate == "" ? DateTime.Now : DateTime.Parse(PlayerController.player.adBoxOpenedDate);

        if (DateTime.Now.Date == dt.Date) {
            if (PlayerController.player.adBoxOpenedCount < UnityAddsController.Instance.AdBoxOpenLimit) {
                BoxAdArrow.SetActive(true);
                BoxAdArrow.GetComponent<Animation>().Play();
                SetEnabledAdBox(true);
            } else {
                SetEnabledAdBox(false);
            }
        } else {
            SetEnabledAdBox(true);
            PlayerController.player.adBoxOpenedCount = 0;
        }

        if (UnityAddsController.AdsLoaded == false) {
            SetEnabledAdBox(false);
        }
    }

    public void ShowAdBox() {
        UnityAddsController.Instance.ShowBoxAd();
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

    public void FacebookShare() {
        FacebookController.Instance.FacebookShare();
    }
}
