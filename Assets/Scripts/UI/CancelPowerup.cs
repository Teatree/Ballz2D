using UnityEngine;

public class CancelPowerup : SceneSingleton<CancelPowerup> {
    public GameObject Button;
    public GameObject HC_cost;

    public int CostGems = 200;

    private void CancelLastMove() {
        GridController.doNotMoveRowDown = true;
        BallLauncher.Instance.SummonAllBalls();

        foreach (BlockClone bc in GridController.Instance.blocksSpawnedSaved) {
            bool stillOnGrid = false;
            foreach (Block b in GridController.blocksSpawned) {
                if (bc.col == b.col && bc.row == b.row && !b.destroyed) {
                    b.wasHit = false;
                    b.hitsRemaining = bc.hitsRemaining;
                    b.UpdateVisualState();

                    stillOnGrid = true;
                    break;
                }
            }

            if (!stillOnGrid) {
                BringBackDestroyedBlock(bc);
            }
        }
    }

    private static void BringBackDestroyedBlock(BlockClone bc) {
        Block resurrected = GridController.Instance.GetTheBlock(bc.typeCode, bc.row, bc.col);
        resurrected._type = bc._type;
        resurrected._behaviour = bc._behaviour;
        resurrected.hitsRemaining = bc.hitsRemaining;
        resurrected.row = bc.row;
        resurrected.col = bc.col;
        resurrected.gridRow = bc.gridRow;
        resurrected.UpdateVisualState();
        resurrected.Setup(bc.typeCode, bc.hitsRemaining, bc.gridRow, bc.col);
        resurrected.transform.position = GridController.Instance.GetPosition(bc.gridRow, bc.col);
        GridController.blocksSpawned.Add(resurrected);
    }

    public void DisableButton() {
        Button.SetActive(false);
    }

    public void EnableButton() {
        Button.SetActive(true);
    }

    public void OnClick_CancelMove() {
        if (PlayerController.player.gems >= CostGems) {
            PlayerController.player.gems -= CostGems;

            AnalyticsController.Instance.LogSpendCreditsEvent("Undo", "Undo", CostGems);

            CancelLastMove();
        } else {
            GameUIController.Instance.ShowRedirectPoor();
        }
    }
}
