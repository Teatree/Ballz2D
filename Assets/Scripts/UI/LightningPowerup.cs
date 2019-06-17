using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LightningPowerup : SceneSingleton<LightningPowerup> {

    public Camera cam;

    public Image Lightning;
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
        //inst.transform.SetParent(transform.parent.transform.parent, false);

        cam.GetComponent<Shaker>().TriggerShake();

        Lightning.gameObject.SetActive(true);
        Lightning.color = Color.yellow;
        StartCoroutine(Fade(Lightning));
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
            AnalyticsController.Instance.LogLevelBoostsUsedEvent("Level " + AllLevelsData.CurrentLevelIndex, "Lightning", BallLauncher.Instance.shotCount);
        }
        else if (PlayerController.player.gems >= CostGems) {
            PlayerController.player.gems -= CostGems;

            AnalyticsController.Instance.LogSpendCreditsEvent("Lightning", "Power Up", CostGems);

            ShootLightning();
        }
        else {
            GameUIController.Instance.ShowRedirectPoor();
        }
        EnableButton();
    }

    protected IEnumerator Fade(Image img) {
        yield return new WaitForSeconds(0.1f);
        img.gameObject.SetActive(false);
    }

    
}
