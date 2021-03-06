﻿public class BombBehaviour : IBehaviour {

    public override void setBlock(Block b) {
        this.block = b;
    }

    public override void OnDestroy() {
        LevelController.IncreseScore();
        GameUIController.Instance.UpdateScore(LevelController.levelScore);

        this.block.destroyed = true;
        foreach (Block b in GridController.blocksSpawned) {
            if (!b.destroyed && !b.Equals(this.block) &&
                    (b.row >= block.row - 1 && b.row <= block.row + 1) &&
                    (b.col >= block.col - 1 && b.col <= block.col + 1) && b._type.isCollidable) {
                b._behaviour.OnDestroy();
            }
        }
        this.block.destroyed = false;
        block.DestroySelf();
    }

    public override void OnCollide(Ball b) {
        UpdateSavedBlocks();
        block.Hit();
        block.UpdateVisualState();
    }

    public override void LooseOneLife() {
        block.hitsRemaining--;
        if (block.hitsRemaining > 0) {

            block.UpdateVisualState();
        }
        else {
            OnDestroy();
        }
    }

    public override void Update() { }
}
