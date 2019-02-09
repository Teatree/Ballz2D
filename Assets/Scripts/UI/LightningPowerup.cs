using UnityEngine;
using UnityEngine.UI;

public class LightningPowerup : SceneSingleton<LightningPowerup> {

    public GameObject Lightning;
    public GameObject Button;
    public Text AmountText;
    public GameObject playArea;
    public GameObject HC_cost;

    public static ItemData idata;

    public int CostGems;

    public void Start() {
        UpdateVisual();
    }

    private void ShootLightning() {
        PlayAni();
        foreach (Block b in GridController.blocksSpawned) {
            if (b._type.isCollidable) {
                b.GetLightningDamage();
            }
        }
        UpdateVisual();
    }

    public void DisableButton() {
        Button.SetActive(false);
    }

    public void EnableButton() {
        int has = hasLightnigItem();
        HC_cost.SetActive(has < 0);
        AmountText.text = has >= 0 ? "x" + PlayerController.player.items[has].amount.ToString() : "";
        Button.SetActive(true);
    }

    private int hasLightnigItem() {
        int has = -1;
        for (int i = 0; i < PlayerController.player.items.Count; i++) {
            if (PlayerController.player.items[i].name.Equals("Booster Lightning")) {
                has = i;
            }
        }

        return has;
    }

    public void UpdateVisual() {
        if (PlayerController.player.gems < CostGems) {
            DisableButton();
        }
    }

    private void PlayAni() {
        GameObject inst = Instantiate(Lightning);
        inst.transform.SetParent(transform.parent.transform.parent, false);
        inst.SetActive(true);
        Destroy(inst, 0.3f);
    }

    public void OnClick_Lightning() {
        int hasItem = hasLightnigItem();
    
        if (hasItem >= 0) {
            ShootLightning();

            if (PlayerController.player.items[hasItem].amount > 1) {
                PlayerController.player.items[hasItem].amount--;
            }
            else {
                PlayerController.player.items.RemoveAt(hasItem);
            }
        }
        else if (PlayerController.player.gems >= CostGems) {
            PlayerController.player.gems -= CostGems;
            ShootLightning();
        }
        else {
            GameUIController.Instance.ShowShop();
        }
        EnableButton();
    }
}
