using UnityEngine;
using System.Collections;

public class ExtraBallBehaviour : IBehaviour {

    public float moveSpeed = 12;

    public override void setBlock(Block b) {
        this.block = b;
    }

    public override void OnCollide(Ball ball) {
        if (!block.wasHit) {
            block.wasHit = true;
        }
    }

    public override void OnDestroy() {
    }

    public override void Update() {

        if (block.wasHit && moveSpeed > 0) {
            if (block.transform.position.y >= 0.07) {
                block.transform.position = new Vector2(block.transform.position.x, block.transform.position.y - moveSpeed * Time.deltaTime);
            } else {
                moveSpeed = 0;
                block.StartCoroutine(addExtraBall());
            }
        }
    }

    public override void LooseOneLife() {
    }

    private IEnumerator addExtraBall() {
        yield return new WaitForSeconds(5f);
        BallLauncher.ExtraBalls++;
        block.DestroySelf();
    }
}