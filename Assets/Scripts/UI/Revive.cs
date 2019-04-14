using System;
using UnityEngine;
using UnityEngine.UI;

public class Revive : IPopup<Revive> {

    public GameObject laserLine1;
    public GameObject laserLine2;
    public GameObject laserLine3;
    public GameObject adsButton;
    public Text HCAmount;

    public static int RowToDestroyIndex;
    public static float RowToDestroyPosition;

    private float line1Y;
    private float line2Y;
    private float line3Y;

    public int CostGems;

    // Ads Stuff
    public int MoreHCReviveOpenLimit = 3;

    public static bool available = false;

    public void Start() {
        UpdateButtsButtonState();

        //HCAmount.text = PlayerController.player != null ? PlayerController.player.gems.ToString() : "0";

        laserLine1 = GameUIController.Instance.laserLine1;
        laserLine2 = GameUIController.Instance.laserLine2;
        laserLine3 = GameUIController.Instance.laserLine3;

        line1Y = GridController.Instance.grid.GetChild(GridController.Instance.grid.childCount - 3).transform.position.y;
        line2Y = GridController.Instance.grid.GetChild(GridController.Instance.grid.childCount - 2).transform.position.y;
        line3Y = GridController.Instance.grid.GetChild(GridController.Instance.grid.childCount - 1).transform.position.y;

        laserLine1.transform.position = new Vector3(-3.15f, line1Y);
        laserLine2.transform.position = new Vector3(-3.15f, line2Y);
        laserLine3.transform.position = new Vector3(-3.15f, line3Y);
    }

    public void Update() {
        HCAmount.text = PlayerController.player != null ? PlayerController.player.gems.ToString() : "0";
    }

    public void UpdateButtsButtonState() {
        DateTime dt = PlayerController.player == null || PlayerController.player.MoreHCReviveOpenedDate == null || PlayerController.player.MoreHCReviveOpenedDate == "" ? DateTime.Now : DateTime.Parse(PlayerController.player.MoreHCReviveOpenedDate);

        if (DateTime.Now.Date == dt) {
            if (PlayerController.player.MoreHCReviveCount < MoreHCReviveOpenLimit) {
                adsButton.SetActive(true);
            }
            else {
                adsButton.SetActive(false);
            }
        }
        else {
            PlayerController.player.MoreHCReviveOpenedDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
            adsButton.SetActive(true);
        }
    }

    public void GetRevive() {
        Debug.Log(LevelController.isGameOver);
        if (LevelController.isGameOver && PlayerController.player.gems >= CostGems) {
            OnClick_Close();
            DestroyBottomLines();
            PlayerController.player.gems -= CostGems;
            available = false;
        }
        else {
            GameUIController.Instance.ShowRedirectPoorRevive();
        }
    }

    public void ShowThemAdsForHC() {
        UnityAddsController.Instance.ShowMoreHCReviveAd();
    }

    public void DestroyBottomLines() {
        ShootLasers(laserLine1.transform.GetComponent<LineRenderer>(), line1Y);
        ShootLasers(laserLine2.transform.GetComponent<LineRenderer>(), line2Y);
        ShootLasers(laserLine3.transform.GetComponent<LineRenderer>(), line3Y);
        foreach (Block b in GridController.blocksSpawned) {
            if (!b.destroyed && (b.row == RowToDestroyIndex || b.row == RowToDestroyIndex + 1 || b.row == RowToDestroyIndex + 2)) {
                b._behaviour.OnDestroy();
            }
        }
        GameUIController.Instance.UpdateScore(LevelController.levelScore);
        Warning.Instance.StopAndDestroyWarnings();
    }

    private void ShootLasers(LineRenderer laserLine, float y) {

        Color c = laserLine.material.color;
        c.a = 1f;
        laserLine.material.color = c;

        laserLine.positionCount = 2;
        laserLine.SetPosition(0, new Vector2(3.15f, y));
        laserLine.SetPosition(1, new Vector2(-3.15f, y));

        GameUIController.Instance.FadingLasers(laserLine);
    }

    public void OnClick_ShowShop() {
        GameUIController.Instance.ShowShop();
    }

    public void OnClick_ShowGameOver() {
        Revive.available = false;

        GameUIController.Instance.ShowGameOver();
    }
}
