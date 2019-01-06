using UnityEngine;

public class LightningPowerup : SceneSingleton<LightningPowerup> {

    public GameObject Lightning;
    public GameObject Button;

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

    }

    public void OnClick_Lightning() {
        Debug.Log(">>> Lightning cost > " + PlayerController.player.gems);
        if (PlayerController.player.gems >= CostGems) {
            PlayerController.player.gems -= CostGems;
            ShootLightning();
        }
        else {
            GameUIController.Instance.ShowShop();
        }
    }
}
