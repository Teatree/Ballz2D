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
        Button.SetActive(true);
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
        if (PlayerController.player.gems >= CostGems) {
            PlayerController.player.gems -= CostGems;
            ShootLightning();
        }
        else {
            GameUIController.Instance.ShowShop();
        }
    }
}
