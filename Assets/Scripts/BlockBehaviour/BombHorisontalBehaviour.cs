using System.Collections;
using UnityEngine;

public class BombHorisontalBehaviour : IBehaviour {

    private LineRenderer laserLine;

    public override void setBlock(Block b) {
        this.block = b;
        laserLine = block.gameObject.GetComponent<LineRenderer>();
    }

    public override void OnDestroy() {
        if (!block.destroyed) {
            this.block.destroyed = true;
            GameController.IncreseScore();
            ShootLasers();
            foreach (Block b in BlockSpawner.blocksSpawned) {
                if (!b.Equals(this.block) && !b.destroyed && b.row == block.row && b != this.block) {
                    b._behaviour.OnDestroy();
                }
            }
            GameUIController.Instance.UpdateScore(GameController.Score);
            block.destroyed = true;
        }
    }

    public override void OnCollide(Ball ball) {
        block.Hit();
        block.UpdateVisualState();
    }

    public override void LooseOneLife() {
        block.hitsRemaining--;
        if (block.hitsRemaining > 0) {
            block.UpdateVisualState();
        }
        else {
            OnDestroy();
        }
    }

    // shoot them pretty lasers
    public void ShootLasers() {

        Color c = laserLine.material.color;
        c.a = 1f;
        laserLine.material.color = c;

        laserLine.positionCount = 2;
        laserLine.SetPosition(0, new Vector2(3.15f, block.transform.position.y));
        laserLine.SetPosition(1, new Vector2(-3.15f, block.transform.position.y));

        block.StartCoroutine(BombLaserFade(laserLine));
    }

    public override void Update() { }
}

