using UnityEngine;

public abstract class IBehaviour {

    protected Block block;

    public void setBlock(Block b) {
        this.block = b;
    }

    public abstract void OnDestroy();

    public abstract void OnCollide();

    public abstract void Update();

    //public abstract void OnDestroy(Block b);

    //public abstract void OnCollide(Block b);
}
