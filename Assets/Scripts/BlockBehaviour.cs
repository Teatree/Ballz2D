using UnityEngine;

public class BlockBehaviour : IBehaviour {

    //public override void OnDestroy(Block b) {
    //    GameController.IncreseScore();
    //    b.DestroySelf();
    //}

    //public override void OnCollide(Block b) {
    //    b.hitsRemaining--;
    //    Debug.Log("Block Collide");
    //}

    public override void OnDestroy() {
        GameController.IncreseScore();
        block.DestroySelf();
    }

    public override void OnCollide() {
            block.HitOnce();
            block.UpdateVisualState();
    }

    public override void Update() {
        if (block.hitsRemaining > 0) {
            OnCollide();
        }
        else {
            OnDestroy();
        }
    }
}
