using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxPopup : IPopup<BoxPopup> {

    public ItemObject itemToReceive;

    public Image itemImage;
    public GameObject itemText;
    public Text itemName;

    [Header("Buttonz")]
    public GameObject panelBalls;
    public GameObject panelDefault;

    [Header("Fade In Elements")]
    public Image Box;
    public Image Bg;

    void Start () {
        Debug.Log(itemToReceive);
        StartCoroutine(FadeIn());

        itemImage.sprite = itemToReceive.shopImage;
        itemImage.SetNativeSize();
        itemName.text = itemToReceive.name;

        if (itemToReceive.amount <= 0) {
            itemText.SetActive(false);
            panelBalls.SetActive(true);
            panelDefault.SetActive(false);

        }
        else {
            itemText.SetActive(true);
            itemText.GetComponent<Text>().text = itemToReceive.amount.ToString();
            panelBalls.SetActive(false);
            panelDefault.SetActive(true);
        }
    }

    public IEnumerator FadeIn() {
        while (Box.color.a < 1) {
            Box.color = new Color(Box.color.r, Box.color.g, Box.color.b, Box.color.a + Time.deltaTime*3);
            Bg.color = new Color(Bg.color.r, Bg.color.g, Bg.color.b, Bg.color.a + Time.deltaTime * 3);
            yield return null;
        }
    }

    public void OnClick_Equip() {
        ItemsController.getItem(itemToReceive, true);
        ItemsController.EquipSpecialBall(itemToReceive);
    }

    public override void OnClick_Close() {
        ItemsController.getItem(itemToReceive, true);
        base.OnClick_Close();
    }
}
