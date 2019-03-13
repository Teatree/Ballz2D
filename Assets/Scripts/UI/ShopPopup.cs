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

    public Tab hcTab;
    public static BallShopItem EquipedBall;

    public void Start() {
        //Debug.Log(">>>> " + Purchaser.purchaser.GetLocalPrice(Purchaser.GEMS_200));
        gems200.text = Purchaser.purchaser.GetLocalPrice(Purchaser.GEMS_200);
        gems400.text = Purchaser.purchaser.GetLocalPrice(Purchaser.GEMS_400);
        gems600.text = Purchaser.purchaser.GetLocalPrice(Purchaser.GEMS_600);
        gems1100.text = Purchaser.purchaser.GetLocalPrice(Purchaser.GEMS_1100);
        gems2300.text = Purchaser.purchaser.GetLocalPrice(Purchaser.GEMS_2300);
        gems7200.text = Purchaser.purchaser.GetLocalPrice(Purchaser.GEMS_7200);
        gems12500.text = Purchaser.purchaser.GetLocalPrice(Purchaser.GEMS_12500);
        gems30000.text = Purchaser.purchaser.GetLocalPrice(Purchaser.GEMS_30000);
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

    void Update() {
        gemsCurrent.text = PlayerController.player != null ? PlayerController.player.gems.ToString() : "0";
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

    public string getTheOfferType() {
        DateTime firstLogin = DateTime.Parse(PlayerController.player.firstLoginAt);
        var daysPlaying = DateTime.Now.Subtract(firstLogin).TotalDays;
        if (daysPlaying < 7) {
            return "STARTER";
        }
        if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday || DateTime.Now.DayOfWeek == DayOfWeek.Saturday) {
            return "WEEKEND";
        }
        return "STATIC";
    }
}
