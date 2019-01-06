using System.Collections;
using UnityEngine;

public class LaserHorisontalBehaviour : IBehaviour {

    private LineRenderer laserLine;

    public override void setBlock(Block b) {

        this.block = b;
        laserLine = block.gameObject.GetComponent<LineRenderer>();
    }

    public override void OnDestroy() {
        block.DestroySelf();
    }

    public override void OnCollide(Ball ball) {
        UpdateSavedBlocks();
        if (!activated) {
            ShootLasers();
            foreach (Block b in GridController.blocksSpawned) {
                if (!b.destroyed && b.row == block.row) {
                    b.Hit();
                }
            }
            block.wasHit = true;
            activated = true;
        }
    }

    // shoot them pretty lasers
    public void ShootLasers() {
        if (fadeRoutine != null) {
            StopFadeRoutine();
        }
        Color c = laserLine.material.color;
        c.a = 1f;
        laserLine.material.color = c;

        RaycastHit2D hitLeft = Physics2D.Raycast(block.transform.position, Vector2.left, 50f, block.WallsMask);
        RaycastHit2D hitRight = Physics2D.Raycast(block.transform.position, Vector2.right, 50f, block.WallsMask);

        laserLine.positionCount = 2;
        laserLine.SetPosition(0, new Vector2(hitRight.point.x, block.transform.position.y));
        laserLine.SetPosition(1, new Vector2(hitLeft.point.x, block.transform.position.y));

        fadeRoutine = block.StartCoroutine(LaserFade(laserLine));
    }

    public override void LooseOneLife() {
        //Do something 
    }

    public override void Update() { }
}
