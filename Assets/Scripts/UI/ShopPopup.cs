using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopPopup : IPopup<Pause> {
    public Text gems200;
    public Text gems400;
    public Text gems600;
    public Text gems1100;
    public Text gems2300;
    public Text gems7200;
    public Text gems12500;
    public Text gems30000;
    public Text gemsCurrent;
    public Text noAds;

    public Tab hcTab;
    public static BallShopItem EquipedBall;

    [Header("Offers")]
    public GameObject offerTab;
    public GameObject weekendOffer;
    public GameObject starterOffer;
    public GameObject staticOffer;
    public GameObject emptyOffer;
    [Header("Offer timers")]
    public GameObject timerGo;

    private Text timer;

    private int _countdownDay;
    private int _countdownHour;
    private int _countdownMinute;
    private int _countdownSeconds;
    private DateTime _countdown;

    public void Start() {
        Debug.Log(">>>> " + Purchaser.purchaser.GetLocalPrice(Purchaser.GEMS_200));
        gems200.text = Purchaser.purchaser.GetLocalPrice(Purchaser.GEMS_200);
        gems400.text = Purchaser.purchaser.GetLocalPrice(Purchaser.GEMS_400);
        gems600.text = Purchaser.purchaser.GetLocalPrice(Purchaser.GEMS_600);
        gems1100.text = Purchaser.purchaser.GetLocalPrice(Purchaser.GEMS_1100);
        gems2300.text = Purchaser.purchaser.GetLocalPrice(Purchaser.GEMS_2300);
        gems7200.text = Purchaser.purchaser.GetLocalPrice(Purchaser.GEMS_7200);
        gems12500.text = Purchaser.purchaser.GetLocalPrice(Purchaser.GEMS_12500);
        gems30000.text = Purchaser.purchaser.GetLocalPrice(Purchaser.GEMS_30000);
        noAds.text = Purchaser.purchaser.GetLocalPrice(Purchaser.NO_ADS);

        SetUpOffers(getTheOfferType());
    }

    void Update() {
        gemsCurrent.text = PlayerController.player != null ? PlayerController.player.gems.ToString() : "0";

        if (offerTab.activeSelf == true) {
            // ifs
            _countdownDay = _countdown.Day - DateTime.Now.Day - 1;
            _countdownHour = 24 - DateTime.Now.Hour;
            _countdownMinute = 59 - DateTime.Now.Minute;
            _countdownSeconds = 59 - DateTime.Now.Second;

            if (_countdownDay < 1) {
                timerGo.GetComponent<Text>().text = "Offer Ends in: " + string.Format("{0:00}h {1:00}m {2:00}s", _countdownHour, _countdownMinute, _countdownSeconds);
            }
            else {
                timerGo.GetComponent<Text>().text = "Offer Ends in: " + string.Format("{0}d {1:00}h {2:00}m", _countdownDay, _countdownHour, _countdownMinute);
            }
            // calc
        }
    }

    void SetUpOffers(string offerToShow) {
        switch (offerToShow) {
            case "pack_weekend": {
                    Debug.Log("weekend");
                    weekendOffer.SetActive(true);
                    starterOffer.SetActive(false);
                    staticOffer.SetActive(false);
                    emptyOffer.SetActive(false);

                    DateTime today = DateTime.Today;
                    // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
                    int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
                    DateTime nextMonday = today.AddDays(daysUntilMonday);

                    Debug.Log("next monday " + nextMonday);
                    _countdown = nextMonday;
                    break;
                }
            case "pack_starter": {
                    Debug.Log("starter");
                    weekendOffer.SetActive(false);
                    starterOffer.SetActive(true);
                    staticOffer.SetActive(false);
                    emptyOffer.SetActive(false);
                    break;
                }
            case "pack_static": {
                    Debug.Log("static");
                    weekendOffer.SetActive(false);
                    starterOffer.SetActive(false);
                    staticOffer.SetActive(true);
                    emptyOffer.SetActive(false);
                    break;
                }
            default: {
                    Debug.Log("n");
                    weekendOffer.SetActive(false);
                    starterOffer.SetActive(false);
                    staticOffer.SetActive(false);
                    emptyOffer.SetActive(true);
                    break;
                }
        }
    }

    public void BuyGems200() {
        Purchaser.purchaser.BuyGems200();
    }

    public void BuyGems400() {
        Purchaser.purchaser.BuyGems400();
    }

    public void BuyGems600() {
        Purchaser.purchaser.BuyGems600();
    }

    public void BuyGems1100() {
        Purchaser.purchaser.BuyGems1100();
    }

    public void BuyGems2300() {
        Purchaser.purchaser.BuyGems2300();
    }

    public void BuyGems7200() {
        Purchaser.purchaser.BuyGems7200();
    }

    public void BuyGems12500() {
        Purchaser.purchaser.BuyGems12500();
    }

    public void BuyGems30000() {
        Purchaser.purchaser.BuyGems30000();
    }

    public void BuyNoAds() {
       Purchaser.purchaser.BuyNoAds();
    }

    public override void OnClick_Close() {
        LevelController.ResumeGame();
        base.OnClick_Close();
    }

    public override void SwitchToShopHCTab() {
        hcTab.SetContentActive();
    }

    public void BuyShopOffer(ShopOffer sh) {
        // buy by product id
    //    Purchaser.purchaser.buyOffer(sh);

        if(sh.id == "pack_starter") {
            PlayerController.player.boughtStarter = true;
        }
    }

    public string getTheOfferType() {
        DateTime firstLogin = DateTime.Parse(PlayerController.player.firstLoginAt);
        var daysPlaying = DateTime.Now.Subtract(firstLogin).TotalDays;
        if (daysPlaying < 3 && !PlayerController.player.boughtStarter || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) { // must see it if he logged in not on Tuesday
            return "pack_starter";
        }
        if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday || DateTime.Now.DayOfWeek == DayOfWeek.Saturday) {
            return "pack_weekend";
        }
        if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday || DateTime.Now.DayOfWeek == DayOfWeek.Friday) {
            return "pack_static";
        }
        return "EMPTY";
    }

    // Buying
}
