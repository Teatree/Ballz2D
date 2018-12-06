using UnityEngine;

public class LaserBehaviour : IBehaviour { 

    public override void OnDestroy() {
        block.DestroySelf();
        Debug.Log("Laser Destroy");;
    }

    public override void OnCollide() {
        foreach (Block b in BlockSpawner.blocksSpawned) {
            if (!b.destroyed && b.row == block.row ) {
                b.HitOnce();
                b.UpdateVisualState();
                ShootLasers();
            }
        }
        Debug.Log("Laser Collide");
    }

    // dies after end of turn

    // shoot them pretty lasers
    public void ShootLasers() {
        var laserLine = block.gameObject.GetComponent<LineRenderer>();

        laserLine.positionCount = 2;
        laserLine.SetPosition(0, new Vector2(3.15f, block.transform.position.y));
        laserLine.SetPosition(1, new Vector2(-3.15f, block.transform.position.y));
    }

    public override void Update() {
        OnCollide();
    }

        //public override void OnDestroy(Block b) {
        //    b.DestroySelf();
        //    Debug.Log("Laser Destroy");
        //}

        //public override void OnCollide(Block b) {
        //    Debug.Log("Laser Collide");
        //}
    }
