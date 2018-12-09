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

    public override void OnCollide() {
        foreach (Block b in BlockSpawner.blocksSpawned) {
            ShootLasers();
            if (!b.destroyed && b.row == block.row) {
                b.Hit();
            }
        }
        block.wasHit = true;
        OnCollisionExit();
    }

    public override void OnCollisionExit() {
        laserLine.positionCount = 0;
    }

    // shoot them pretty lasers
    public void ShootLasers() {
        laserLine.positionCount = 2;
        laserLine.SetPosition(0, new Vector2(3.15f, block.transform.position.y));
        laserLine.SetPosition(1, new Vector2(-3.15f, block.transform.position.y));
    }

    public override void GetOneLife() {
        //Do something 
    }
}
