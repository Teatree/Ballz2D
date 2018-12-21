using UnityEngine;
using UnityEngine.UI;

public class LightningPowerup : SceneSingleton<LightningPowerup> {

    public GameObject Lightning;
    public GameObject Button;
    public GameObject Text;

    private Button _buttonComponent;
    private int CostGems;

    public void Start() {
        _buttonComponent = Button.transform.GetComponent<Button>();
        UpdateVisual();
    }
    public void ShootLightning() {
        if (GameController.Gems >= CostGems) {
            PlayAni();
            foreach (Block b in GridController.blocksSpawned) {
                if (b._type.isCollidable) {
                    b.GetLightningDamage();
                }
            }
            GameController.Gems -= CostGems;
            CostGems += 100;
        }
        UpdateVisual();
    }

    public void DisableButton() {
        if (_buttonComponent != null) {
            _buttonComponent.interactable = false;
        }
    }

    public void EnableButton() {
        if (_buttonComponent != null && GameController.Gems >= CostGems) {
           
            _buttonComponent.interactable = true;
        }
    }

    public void UpdateVisual() {
        if (_buttonComponent != null) {
            Text.GetComponent<Text>().text = CostGems > 0 ? "" + CostGems : "";
        }

        if (GameController.Gems < CostGems) {
            DisableButton();
        }
    }

    private void PlayAni() {

    }
}
