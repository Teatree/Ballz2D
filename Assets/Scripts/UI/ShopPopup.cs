﻿public class ShopPopup : IPopup<Pause> {


    public void BuyGems100() {
        //Purchaser.purchaser.BuyGems100();
        PlayerController.player.gems += 100;
    }


    public void BuyNoAds() {
        Purchaser.purchaser.BuyNoAds();
    }
}