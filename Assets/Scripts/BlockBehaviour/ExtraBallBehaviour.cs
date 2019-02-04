using UnityEngine;
using System.Collections;

public class ExtraBallBehaviour : IBehaviour {

    public float moveSpeed = 12;

    public override void setBlock(Block b) {
        this.block = b;
    }

    public override void OnCollide(Ball b) {
        UpdateSavedBlocks();
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
                GetExtraBall();
                block.StartCoroutine(DestroySelfLater());
            }
        }
    }

    public void GetExtraBall() {
        moveSpeed = 0;
        BallLauncher.ExtraBalls++;
    }

    public override void LooseOneLife() {
    }

    private IEnumerator DestroySelfLater() {
        yield return new WaitForSeconds(5f);
        block.DestroySelf();
    }
}