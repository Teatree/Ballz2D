using UnityEngine;

public class BombVerticalBehaviour : IBehaviour {

    private LineRenderer laserLine;

    public override void setBlock(Block b) {
        this.block = b;
        laserLine = block.gameObject.GetComponent<LineRenderer>();
    }

    public override void OnDestroy() {
        if (!block.destroyed) {
            GameController.IncreseScore();
            ShootLasers();
            foreach (Block b in BlockSpawner.blocksSpawned) {
                if (!b.destroyed && b.col == block.col && b != this.block) {
                    b.DestroySelf();
                }
            }
            block.destroyed = true;
        } 
    }

    public override void OnCollide() {
        block.Hit();
        block.UpdateVisualState();
    }

    public override void GetOneLife() {
        block.hitsRemaining--;
        if (block.hitsRemaining > 0) {

            block.UpdateVisualState();
        }
        else {
            OnDestroy();
        }
    }

    public override void OnCollisionExit() { }

    // shoot them pretty lasers
    public void ShootLasers() {
        Color c = laserLine.material.color;
        c.a = 1f;
        laserLine.material.color = c;

        laserLine.positionCount = 2;
        laserLine.SetPosition(0, new Vector2(block.transform.position.x, 0));
        laserLine.SetPosition(1, new Vector2(block.transform.position.x, 8.5f));

        block.StartCoroutine(BombLaserFade(laserLine));
    }
}

