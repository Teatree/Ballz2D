using UnityEngine;

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
        foreach (Block b in BlockSpawner.blocksSpawned) {
            ShootLasers();
            if (!b.destroyed && (b.row == block.row || b.col == block.col)) {
                b.Hit();
            }
        }
        block.wasHit = true;
    }

    public override void OnCollisionExit() {
        laserLine.positionCount = 0;
    }

    // shoot them pretty lasers
    public void ShootLasers() {
        laserLine.positionCount = 5;
        laserLine.SetPosition(0, new Vector2(3.15f, block.transform.position.y));
        laserLine.SetPosition(1, new Vector2(-3.15f, block.transform.position.y));
        laserLine.SetPosition(2, new Vector2(block.transform.position.x, block.transform.position.y));

        laserLine.SetPosition(3, new Vector2(block.transform.position.x, 0));
        laserLine.SetPosition(4, new Vector2(block.transform.position.x, 8.5f));
    }

    public override void GetOneLife() {
        //Do something 
    }
}
