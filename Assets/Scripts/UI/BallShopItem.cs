
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BallShopItem : MonoBehaviour {

    public Image ShopImage;
    public Text ItemCost;
    public Text EquipTxt;
    public Text EquippedTxt;
    public GameObject uglyGem;
    public Image priceHolder;
    public Button button;
    public GameObject confirmPopup;
    public GameObject redirectPoorPopup;

    public Sprite buttonYellow;
    public Sprite buttonGreen;

    [Header("BALL")]
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
        } else if (PlayerController.player.gems >= itemObject.costGems && !hasItem) {
            StartCoroutine(confirmedBuy());
        }
        else {
            StartCoroutine(redirectPoor());
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
    public IEnumerator redirectPoor() {
        GameObject redir = Instantiate(redirectPoorPopup, ShopPopup.Instance.gameObject.transform);
        while (redir.GetComponent<ConfirmBallPopup>().result == "") {
            yield return null;
        }
        if (redir.GetComponent<ConfirmBallPopup>().result == "ok") {
            ShopPopup.Instance.SwitchToShopHCTab();
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
        priceHolder.sprite = buttonYellow;
        Color newCol = new Color();
        if (ColorUtility.TryParseHtmlString("#C8C8C8", out newCol))
            priceHolder.color = newCol;
        ShopPopup.EquipedBall = this;
    }

    public void uiEquip() {
        button.interactable = true;
        uglyGem.SetActive(false);
        EquippedTxt.gameObject.SetActive(false);
        EquipTxt.gameObject.SetActive(true);
        priceHolder.sprite = buttonYellow;
        Color newCol = new Color();
        if (ColorUtility.TryParseHtmlString("#FFFFFF", out newCol))
            priceHolder.color = newCol;
        ItemCost.gameObject.SetActive(false);
    }

    public void uiBuy() {
        button.interactable = true;
        uglyGem.SetActive(true);
        EquippedTxt.gameObject.SetActive(false);
        EquipTxt.gameObject.SetActive(false);
        priceHolder.sprite = buttonGreen;
        Color newCol = new Color();
        if (ColorUtility.TryParseHtmlString("#FFFFFF", out newCol))
            priceHolder.color = newCol;
        ItemCost.gameObject.SetActive(true);
    }
}
