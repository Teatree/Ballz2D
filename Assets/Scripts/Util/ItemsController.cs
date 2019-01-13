using UnityEngine;

public class ItemsController : MonoBehaviour {

    public static void getItem(ItemObject iobj, bool free) {
        if (PlayerController.player.items.FindIndex(x => x.name.Equals(iobj.name)) < 0) {
            PlayerController.player.items.Add(new ItemData(iobj));
        }
        else {
            int i = PlayerController.player.items.FindIndex(x => x.name.Equals(iobj.name));
            PlayerController.player.items[i].amount += iobj.amount;
        }

        if (!free) {
            PlayerController.player.gems -= iobj.costGems;
        }
    }

    public static void EquipSpecialBall(ItemObject iobj) {
        LevelController.SpecialBall = iobj.assetImage;
        iobj.enabled = true;
        PlayerController.player.items.Find(x => x.name.Equals(iobj.name)).enabled = true;
    }
}
