using UnityEngine;
using System.Collections;

public abstract class IBehaviour {

    protected Block block;
    protected bool activated;

    public abstract void setBlock(Block b);

    public abstract void OnDestroy();

    public abstract void OnCollide();

    public abstract void OnCollisionExit();

    public abstract void GetOneLife();

    protected IEnumerator LaserFade(LineRenderer laserLine) {
        for (int i = 0; i < 50; i++) {
            Color c = laserLine.material.color;
            c.a = c.a - 0.02f;
            laserLine.material.color = c;
            yield return null;
        }
        activated = false;
    }

    protected IEnumerator BombLaserFade(LineRenderer laserLine) {

        block.transform.position = new Vector2(-100, -100);
        for (int i = 0; i < 50; i++) {
            activated = false;
            Color c = laserLine.material.color;
            c.a = c.a - 0.02f;
            laserLine.material.color = c;
            yield return null;
        }
        activated = false;
        block.DestroySelf();
    }
}
