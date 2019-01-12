using UnityEngine;
using UnityEngine.UI;

public class ShopPopup : IPopup<Pause> {
    public Text gems100; 

    public void Start() {
        Debug.Log(">>>> " + Purchaser.purchaser.GetLocalPrice("100_gems"));
        gems100.text = Purchaser.purchaser.GetLocalPrice("100_gems");
    }
        public void BuyGems100() {
        // gems100.GetComponentInChildren<Text>().text = Purchaser.purchaser.GetLocalPrice("100_gems");
        Purchaser.purchaser.BuyGems100();
    }

    public void BuyNoAds() {
        Purchaser.purchaser.BuyNoAds();
    }

    public override void OnClick_Close() {
        LevelController.ResumeGame();
        base.OnClick_Close();
    }

}
