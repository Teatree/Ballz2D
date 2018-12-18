using UnityEngine;

public class LightningPowerup : SceneSingleton<LightningPowerup> {

    private int costGems;
    GameObject lightning;

    public void ShootLightning() {
        if (GameController.Gems >= costGems) {
            PlayAni();
            foreach (Block b in BlockSpawner.blocksSpawned) {
                if (b._type.isCollidable) {
                    b.GetLightningDamage();
                }
            }
            GameController.Gems -= costGems;
           // costGems += 100;
        }
    }

    private void PlayAni() {

    }
}
