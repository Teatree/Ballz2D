using UnityEngine;

public class LightningPowerup : SceneSingleton<LightningPowerup> {

    public GameObject Lightning;
    public GameObject Button;
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
        HC_cost.SetActive(hasLightnigItem() < 0);
        Button.SetActive(true);
    }

    private int hasLightnigItem() {
        int has = -1;
        for (int i = 0; i < PlayerController.player.items.Count; i++) {
            if (PlayerController.player.items[i].name.Equals("Lightning")) {
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
        Debug.Log(">>> hasItem > " + hasItem);
    
        if (hasItem >= 0) {
            ShootLightning();

            if (PlayerController.player.items[hasItem].amount > 1) {
                PlayerController.player.items[hasItem].amount--;
                Debug.Log(">>> use  > " + PlayerController.player.items[hasItem].amount);
            }
            else {
                Debug.Log(">>> " + hasItem);
                PlayerController.player.items.RemoveAt(hasItem);
                Debug.Log(">>> " + PlayerController.player.items);
            }
        }
        else if (PlayerController.player.gems >= CostGems) {
            Debug.Log(">>> buy > ");
            PlayerController.player.gems -= CostGems;
            ShootLightning();
        }
        else {
            GameUIController.Instance.ShowShop();
        }
        Debug.Log(">>> " + PlayerController.player.items);
        EnableButton();
    }
}
