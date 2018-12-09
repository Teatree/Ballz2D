using UnityEngine;

public abstract class IBehaviour {

    protected Block block;

    public abstract void setBlock(Block b);

    public abstract void OnDestroy();

    public abstract void OnCollide();

    public abstract void OnCollisionExit();

    public abstract void GetOneLife();
}
