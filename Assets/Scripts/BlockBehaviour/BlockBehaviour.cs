public class BlockBehaviour : IBehaviour {

    public override void setBlock(Block b) {
        this.block = b;
    }

    public override void OnDestroy() {
        GameController.IncreseScore();
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
}
