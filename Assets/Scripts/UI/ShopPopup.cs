using UnityEngine;
using UnityEngine.UI;

public class ShopPopup : IPopup<Pause> {
    public Text gems100; 
    public Text gemsCurrent;

    public Tab hcTab;

    public void Start() {
        Debug.Log(">>>> " + Purchaser.purchaser.GetLocalPrice("100_gems"));
        gems100.text = Purchaser.purchaser.GetLocalPrice("100_gems");
    }
        public void BuyGems100() {
        // gems100.GetComponentInChildren<Text>().text = Purchaser.purchaser.GetLocalPrice("100_gems");
        Purchaser.purchaser.BuyGems100();
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
}
