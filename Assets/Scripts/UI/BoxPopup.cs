using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxPopup : IPopup<BoxPopup> {

    public ItemObject itemToReceive;

    public Image itemImage;
    public GameObject itemText;
    public Text itemName;

	void Start () {
        Debug.Log(itemToReceive);

        itemImage.sprite = itemToReceive.shopImage;
        itemImage.SetNativeSize();
        itemName.text = itemToReceive.name;

        if (itemToReceive.amount <= 0) {
            itemText.SetActive(false);
        }
        else {
            itemText.SetActive(true);
            itemText.GetComponent<Text>().text = itemToReceive.amount.ToString();
        }
    }
	
    public void OnClick_Equip() {
        ItemsController.EquipSpecialBall(itemToReceive);
    }

    public override void OnClick_Close() {
        ItemsController.getItem(itemToReceive, true);
        base.OnClick_Close();
    }
}
