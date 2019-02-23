using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallShopItem : MonoBehaviour {

    public Image ShopImage;
    public Text ItemCost;

    public ItemObject itemObject;

    void Start () {
        ShopImage.sprite = itemObject.shopImage;
        ItemCost.text = itemObject.costGems.ToString();
    }

    public void OnClick_BuyBall() {
        if (ItemsController.getItem(itemObject, false)) {
            ItemsController.EquipSpecialBall(itemObject);

        } else {
            ShopPopup.Instance.SwitchToShopHCTab();
        }
    }
}
