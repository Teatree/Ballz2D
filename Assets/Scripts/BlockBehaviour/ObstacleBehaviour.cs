using UnityEngine;

public class ObstacleBehaviour : IBehaviour {


    public override void setBlock(Block b) {
        this.block = b;
    }

    public override void OnDestroy() {
    }

    public override void OnCollide(Ball ball) {
    }

    public override void LooseOneLife() {
    }

    public override void Update() {

    }
}
