using UnityEngine;

public class FountainBehaviour : IBehaviour {

    public override void setBlock(Block b) {
        this.block = b;
    }

    public override void OnDestroy() {
        block.DestroySelf();
    }

    public override void OnCollide() {
        UpdateSavedBlocks();
        //ball.AddForceBall(new Vector3(Random.Range(-0.75f, 0.75f), Mathf.Abs(ball.dir.y)));

        block.wasHit = true;
    }

    public override void LooseOneLife() {
        //Do something 
    }

    public override void Update() { }
}
