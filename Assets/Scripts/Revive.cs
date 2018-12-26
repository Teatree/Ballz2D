using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Revive : IPopup<Revive> {

    public GameObject laserLine1;
    public GameObject laserLine2;
    public GameObject laserLine3;
    public GameObject button;

    public static int RowToDestroyIndex;
    public static float RowToDestroyPosition;

    public int CostGems;

    public static bool available = false;

    public void Start() {
        available = true;
    }

    public void GetRevive() {
        Debug.Log(GameController.isGameOver);
        if (GameController.isGameOver && GameController.Gems >= CostGems && available) {
            DestroyBottomLines();
            GameController.Gems -= CostGems;
            available = false;
            DisableButton();
        }
    }

    public void DestroyBottomLines() {
            ShootLasers(laserLine1.transform.GetComponent<LineRenderer>(), RowToDestroyPosition);
            ShootLasers(laserLine2.transform.GetComponent<LineRenderer>(), RowToDestroyPosition + Constants.BlockSize);
            ShootLasers(laserLine3.transform.GetComponent<LineRenderer>(), RowToDestroyPosition + 2 * Constants.BlockSize);
            foreach (Block b in GridController.blocksSpawned) {
                if (!b.destroyed && (b.row == RowToDestroyIndex || b.row == RowToDestroyIndex+1)) {
                    b._behaviour.OnDestroy();
                }
            }
            GameUIController.Instance.UpdateScore(GameController.levelScore);
        }

    private void ShootLasers(LineRenderer laserLine, float y) {

        Color c = laserLine.material.color;
        c.a = 1f;
        laserLine.material.color = c;

        laserLine.positionCount = 2;
        laserLine.SetPosition(0, new Vector2(3.15f, y));
        laserLine.SetPosition(1, new Vector2(-3.15f, y));

        StartCoroutine(BombLaserFade(laserLine));
    }

    private IEnumerator BombLaserFade(LineRenderer laserLine) {
        for (int i = 0; i < 50; i++) {
            Color c = laserLine.material.color;
            c.a = c.a - 0.02f;
            laserLine.material.color = c;
            yield return null;
        }
        GameController.isGameOver = false;
    }

    public void DisableButton() {
        button.transform.GetComponent<Button>().interactable = false;
    }

    public void EnableButton() {
        if (available) {
            button.transform.GetComponent<Button>().interactable = false;
        } else {
            button.transform.GetComponent<Button>().interactable = true;
        }
    }
}
