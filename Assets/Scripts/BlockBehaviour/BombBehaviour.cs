public class BombBehaviour : IBehaviour {

    public override void setBlock(Block b) {
        this.block = b;
    }

    public override void OnDestroy() {
        PlayAni();
        GameController.IncreseScore();
        foreach (Block b in BlockSpawner.blocksSpawned) {
            if (!b.destroyed &&
                    (b.row >= block.row - 1 && b.row <= block.row + 1) &&
                    (b.col >= block.col - 1 && b.col <= block.col + 1)) {
                b.DestroySelf();
            }
        }
        block.DestroySelf();
    }

    public override void OnCollide() {
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

    public void PlayAni() {
    }

    public override void Update() { }
}
