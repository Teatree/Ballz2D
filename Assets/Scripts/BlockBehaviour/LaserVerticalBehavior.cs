using UnityEngine;
using System.Collections;

public class LaserVerticalBehaviour : IBehaviour {

    private LineRenderer laserLine;

    public override void setBlock(Block b) {

        this.block = b;
        laserLine = block.gameObject.GetComponent<LineRenderer>();
    }

    public override void OnDestroy() {
        block.DestroySelf();
    }

    public override void OnCollide(Ball ball) {
        if (!activated) {
            ShootLasers();
            foreach (Block b in GridController.blocksSpawned) {
                if (!b.destroyed && b.col == block.col) {
                    b.Hit();
                }
            }
            block.wasHit = true;
            activated = true;
        }
    }

    // shoot them pretty lasers
    public void ShootLasers() {
        Color c = laserLine.material.color;
        c.a = 1f;
        laserLine.material.color = c;

        RaycastHit2D hitUp = Physics2D.Raycast(block.transform.position, Vector2.up, 50f, block.WallsMask);
        RaycastHit2D hitDown = Physics2D.Raycast(block.transform.position, Vector2.down, 50f, block.WallsMask);

        laserLine.positionCount = 2;
        laserLine.SetPosition(0, new Vector2(block.transform.position.x, hitDown.point.y));
        laserLine.SetPosition(1, new Vector2(block.transform.position.x, hitUp.point.y));

        block.StartCoroutine(LaserFade(laserLine));
    }

    public override void LooseOneLife() {
        //Do something 
    }

    public override void Update() { }
}
