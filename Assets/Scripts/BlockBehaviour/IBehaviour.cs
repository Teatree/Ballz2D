using UnityEngine;
using System.Collections;

public abstract class IBehaviour {

    protected Block block;
    protected bool activated;
    protected Coroutine fadeRoutine;

    public abstract void setBlock(Block b);

    public abstract void OnDestroy();

    public abstract void OnCollide();
    public abstract void LooseOneLife();

    public abstract void Update();


    protected IEnumerator LaserFade(LineRenderer laserLine) {
        activated = false;
        for (int i = 0; i < 50; i++) {
            Color c = laserLine.material.color;
            c.a = c.a - 0.02f;
            laserLine.material.color = c;
            yield return null;
        }
    }

    protected IEnumerator BombLaserFade(LineRenderer laserLine) {

        block.transform.position = new Vector2(-100, -100);
        for (int i = 0; i < 50; i++) {
            activated = false;
            Color c = laserLine.material.color;
            c.a = c.a - 0.02f;
            laserLine.material.color = c;
            yield return null;
        }
        activated = false;
        block.destroyed = false; //to call destroy self method
        block.DestroySelf();

    }
    protected void StopFadeRoutine() {
        block.StopCoroutine(fadeRoutine);
    }

    protected void UpdateBlocksInfo() {
        if (GridController.BlocksAmount > 0 && block._type.isCollidable) {
            GridController.BlocksAmount--;
            GameUIController.Instance.UpdateBlocksAmount();
        }
    }

    public void UpdateSavedBlocks() {
        if (GridController.Instance.blocksSpawnedSaved != null && GridController.Instance.blocksSpawnedSaved.Count > 0) {
            foreach (BlockClone b in GridController.Instance.blocksSpawnedSaved) {
                if (b.row == block.row && b.col == block.col) {
                    //Debug.Log(">>>> UpdateSavedBlocks > " + block.col + " / " + block.row);
                    return;
                }
            }
        }
        //Debug.Log(">>>> UpdateSavedBlocks > " + block.col + " / " + block.row);
        GridController.Instance.blocksSpawnedSaved.Add(new BlockClone(block));
    }
}
