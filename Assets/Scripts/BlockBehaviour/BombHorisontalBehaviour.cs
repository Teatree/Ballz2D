public class BombHorisontalBehaviour : IBehaviour {

    public override void setBlock(Block b) {
        this.block = b;
    }

    public override void OnDestroy() {
        GameController.IncreseScore();
        PlayAni();
        foreach (Block b in BlockSpawner.blocksSpawned) {
            if (!b.destroyed && b.row == block.row) {
                b.DestroySelf();
            }
        }
        block.DestroySelf();
    }

    public override void OnCollide() {
        block.Hit();
        block.UpdateVisualState();
    }

    public override void GetOneLife() {
        block.hitsRemaining--;
        if (block.hitsRemaining > 0) {

            block.UpdateVisualState();
        }
        else {
            OnDestroy();
        }
    }

    public override void OnCollisionExit() { }

    public void PlayAni() {
    }
}

