using UnityEngine;
using System.Collections;

public class LaserCrossBehaviour : IBehaviour {

    private LineRenderer laserLine;

    public override void setBlock(Block b) {

        this.block = b;
        laserLine = block.gameObject.GetComponent<LineRenderer>();
    }

    public override void OnDestroy() {
        block.DestroySelf();
    }

    public override void OnCollide() {
        if (!activated) {
            ShootLasers();
            foreach (Block b in BlockSpawner.blocksSpawned) {
                if (!b.destroyed && (b.row == block.row || b.col == block.col)) {
                    b.Hit();
                }
            }
            block.wasHit = true;
        }
    }

    // shoot them pretty lasers
    public void ShootLasers() {
        Color c = laserLine.material.color;
        c.a = 1f;
        laserLine.material.color = c;

        laserLine.positionCount = 5;
        laserLine.SetPosition(0, new Vector2(3.15f, block.transform.position.y));
        laserLine.SetPosition(1, new Vector2(-3.15f, block.transform.position.y));
        laserLine.SetPosition(2, new Vector3(block.transform.position.x, block.transform.position.y, -100));

        laserLine.SetPosition(3, new Vector2(block.transform.position.x, 0));
        laserLine.SetPosition(4, new Vector2(block.transform.position.x, 8.5f));

        block.StartCoroutine(LaserFade(laserLine));
    }

    private IEnumerator LaserHide(LineRenderer laserLine) {
        return LaserFade(laserLine);
    }

    public override void LooseOneLife() {
        //Do something 
    }

    public override void Update() { }
}
