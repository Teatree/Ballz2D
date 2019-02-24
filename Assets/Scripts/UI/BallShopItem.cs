
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BallShopItem : MonoBehaviour {

    public Image ShopImage;
    public Text ItemCost;
    public Text EquipTxt;
    public Text EquippedTxt;
    public GameObject uglyGem;
    public Button button;

    public GameObject confirmPopup;
    public ItemObject itemObject;
    bool hasItem = false;

    void Start() {
        ShopImage.sprite = itemObject.shopImage;
        ItemCost.text = itemObject.costGems.ToString();

        checkHasItem();

        if (!hasItem) {
            uiBuy();
        }
        if (PlayerController.player.specialBallName == itemObject.name) {
            uiEquiped();
        }
    }

    private void checkHasItem() {
        foreach (ItemData i in PlayerController.player.items) {
            if (i.name.Equals(itemObject.name)) {
                uiEquip();
                hasItem = true;
                break;
            }
        }
    }

    public void OnClick_BuyBall() {
        checkHasItem();
        if (hasItem) {
            equipAndUI();
        } else if (PlayerController.player.gems >= itemObject.costGems) {
            StartCoroutine(confirmedBuy());
        }
        else {
            ShopPopup.Instance.SwitchToShopHCTab();
        }

    }

    public IEnumerator confirmedBuy() {
        GameObject confirm = Instantiate(confirmPopup, ShopPopup.Instance.gameObject.transform);
        while (confirm.GetComponent<ConfirmBallPopup>().result == "") {
            yield return null;
        }
        if (confirm.GetComponent<ConfirmBallPopup>().result == "ok") {
            if (ItemsController.getItem(itemObject, false)) {
                equipAndUI();
            }

        }
      
    }

    private void equipAndUI() {
        ItemsController.EquipSpecialBall(itemObject);
        if (ShopPopup.EquipedBall != null) {
            ShopPopup.EquipedBall.uiEquip();
        }
        uiEquiped();
    }

    //public void confirmedBuy() {
    //    if (ItemsController.getItem(itemObject, false)) {
    //        ItemsController.EquipSpecialBall(itemObject);
    //        if (ShopPopup.EquipedBall != null) {
    //            ShopPopup.EquipedBall.uiEquip();
    //        }
    //        uiEquiped();
    //    }
    //    else {
    //        ShopPopup.Instance.SwitchToShopHCTab();
    //    }
    //}

    public void uiEquiped() {
        button.interactable = false;
        uglyGem.SetActive(false);
        EquippedTxt.gameObject.SetActive(true);
        EquipTxt.gameObject.SetActive(false);
        ItemCost.gameObject.SetActive(false);
        ShopPopup.EquipedBall = this;
    }

    public void uiEquip() {
        button.interactable = true;
        uglyGem.SetActive(false);
        EquippedTxt.gameObject.SetActive(false);
        EquipTxt.gameObject.SetActive(true);
        ItemCost.gameObject.SetActive(false);
    }

    public void uiBuy() {
        button.interactable = true;
        uglyGem.SetActive(true);
        EquippedTxt.gameObject.SetActive(false);
        EquipTxt.gameObject.SetActive(false);
        ItemCost.gameObject.SetActive(true);
    }
}
