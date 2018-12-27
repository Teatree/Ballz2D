﻿using UnityEngine;

public class Revive : IPopup<Revive> {

    public GameObject laserLine1;
    public GameObject laserLine2;
    public GameObject laserLine3;

    public static int RowToDestroyIndex;
    public static float RowToDestroyPosition;


    private float line1Y;
    private float line2Y;
    private float line3Y;

    public int CostGems;

    public static bool available = false;

    public void Start() {

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

    public void GetRevive() {
        Debug.Log(GameController.isGameOver);
        if (GameController.isGameOver && GameController.Gems >= CostGems) {
            DestroyBottomLines();
            GameController.Gems -= CostGems;
            available = false;
        } else {
            GameUIController.Instance.ShowShop();
        }
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
        GameUIController.Instance.UpdateScore(GameController.levelScore);
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


    public void OnClick_ShowGameOver() {
        GameUIController.Instance.HandleGameOver();
    }
}