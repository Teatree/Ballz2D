using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : SceneSingleton<UIController> {

    //public Transform LevelListParent;

    //[SerializeField]
    //private LevelUI levelUiElementPrefab;
    //public GameObject arrowPrefab;

    public Text gems;
    public Text tempPlyIDTest;
    //public Text starsText;

    public SpriteState sprState = new SpriteState();

    [Header("Popups")]
    public GameObject SettingsPrefab;
    public GameObject ShopPrefab;
    public GameObject BoxPopupPrefab;
    public GameObject waitForItPrefab;

    public GameObject SubscriptionsPrefab;
    public GameObject SubReceivedPopupPrefab;

    //Daily boxes
    public GameObject BoxDayButton;
    public GameObject BoxDayWaitButton;

    public GameObject BoxAdButton;
    public GameObject BoxAdArrow;

    [Header("Item Received Feedback")]
    public GameObject ItemReceived;

    private Button boxAdButtonCmp;
    public Scrollbar lvlSlider;

    public static bool advertiseSubs;

    void Start() {
        DateTime dt = PlayerController.player == null || PlayerController.player.giveBoxAt == null || PlayerController.player.giveBoxAt == "" ? DateTime.Now : DateTime.Parse(PlayerController.player.giveBoxAt);

        if (dt < DateTime.Now) {
            // if Player came logged in after box was supposed to be claiemd
            ShowDayBoxButton();
        }
        else {
            ShowDayBoxWaitButton();
        }

      
        InitAdbox();
      
        if (Purchaser.giveSubsStuff) {
            Purchaser.giveSubsStuff = false;
            OpenSubsBonus();
        }
        if (advertiseSubs && !PlayerController.player.isSubscribed) {
            advertiseSubs = false;
            OpenSubscriptionsOnStart();
        }
    }

    void Update() {
        tempPlyIDTest.text = PlayerController.player.PlayerID;

        gems.text = PlayerController.player != null ? PlayerController.player.gems.ToString() : "0";

        // gems.text = "Gems >>> " +  PlayerController.player.gems;
        //debugText.text = "allLevels: " + AllLevelsData.allLevels.Count + "      path: " + DataController.levelfilePath;

       // Debug.Log(">>> PlayerController.player.adBoxOpenedCount > " + PlayerController.player.adBoxOpenedCount);
       // Debug.Log(">>> UnityAddsController.Instance.AdBoxOpenLimit > " + UnityAddsController.Instance.AdBoxOpenLimit);
        if (PlayerController.player != null && PlayerController.player.adBoxOpenedCount < UnityAddsController.Instance.AdBoxOpenLimit) {
            SetEnabledAdBox(true);
        } else {
            SetEnabledAdBox(false);
        }

        if (Application.platform == RuntimePlatform.Android) {
            if (Input.GetKey(KeyCode.Escape)) {
                Application.Quit();
                return;
            }
        }
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

    public void OpenSubscriptions()
    {
        if (PlayerController.player.isSubscribed == false) {
            Instantiate(SubscriptionsPrefab, transform);
        }
        else {
            Instantiate(SubReceivedPopupPrefab, transform);
        }
    }

    public void OpenSubsBonus() {
        Instantiate(SubReceivedPopupPrefab, transform);
    }

    public void OpenSubscriptionsOnStart() {
        GameObject s = Instantiate(SubscriptionsPrefab, transform);
        SubscriptionPopup sp = s.transform.GetComponent<SubscriptionPopup>();
        sp.isOnStart = true;
        sp.InitPopup();
    }

    public void OpenBoxOpen() {
        var Box = Instantiate(BoxPopupPrefab, transform);
        Box.GetComponent<BoxPopup>().itemToReceive = BoxOpener.Instance.GetBoxContents_BoxAd();

        AnalyticsController.Instance.LogBoxesOpenedEvent("Day / Ad Box", Box.GetComponent<BoxPopup>().itemToReceive.name, Box.GetComponent<BoxPopup>().itemToReceive.amount);
    }

    public void OpenDayBox() {

        OpenBoxOpen();
        ShowDayBoxWaitButton();
        
        if (PlayerController.player != null && PlayerController.player.giveBoxAt != null && PlayerController.player.giveBoxAt != "")
        {
            DateTime dt = DateTime.Parse(PlayerController.player.giveBoxAt);
            NotificationController.Instance.ScheduleBoxNotification(dt);
            Debug.Log(">>>> Notify givebox at ? " + dt);
        }
    }

    public void SetEnabledAdBox(bool b) {
        boxAdButtonCmp = BoxAdButton.GetComponent<Button>();
        boxAdButtonCmp.interactable = b;
        if (b == false) {
            BoxAdArrow.SetActive(b);
        }
    }

    public void Show2ItemReceived(int amount1, int amount2, Sprite iconSprite1, Sprite iconSprite2) { // could add different items in future maybe?
        var itemFeedback = Instantiate(ItemReceived, transform);
        itemFeedback.transform.GetComponent<ItemReceivedFeedback>().setAmount2(amount1, amount2);
        itemFeedback.transform.GetComponent<ItemReceivedFeedback>().setIcon2(iconSprite1, iconSprite2);
        itemFeedback.transform.GetComponent<ItemReceivedFeedback>().SetTwo(true);
    }

    public void InitAdbox() {
        DateTime dt = PlayerController.player == null || PlayerController.player.adBoxOpenedDate == null || PlayerController.player.adBoxOpenedDate == "" ? DateTime.MinValue : DateTime.Parse(PlayerController.player.adBoxOpenedDate);

        Debug.Log(">>>> dt.Date > " + dt.Date);
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

        if (!UnityAddsController.AdsLoaded) {
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
