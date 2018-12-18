using UnityEngine;

public class BombCrossBehaviour : IBehaviour {

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
                if (!b.Equals(this.block) && !b.destroyed && (b.col == block.col || b.row == block.row))  {
                    b._behaviour.OnDestroy();
                }
            }
            GameUIController.Instance.UpdateScore(GameController.levelScore);
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

        laserLine.positionCount = 5;
        laserLine.SetPosition(0, new Vector2(3.15f, block.transform.position.y));
        laserLine.SetPosition(1, new Vector2(-3.15f, block.transform.position.y));
        laserLine.SetPosition(2, new Vector3(block.transform.position.x, block.transform.position.y, -100));

        laserLine.SetPosition(3, new Vector2(block.transform.position.x, 0.40f));
        laserLine.SetPosition(4, new Vector2(block.transform.position.x, 8.5f));

        block.StartCoroutine(BombLaserFade(laserLine));
    }
    public override void Update() { }

}