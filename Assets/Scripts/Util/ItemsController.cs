using UnityEngine;
using System.Collections.Generic;

public class ItemsController : MonoBehaviour {
    //public static List<ItemData> allItems = new List<ItemData>();

    public static void SetupPowerups() {
        if (PlayerController.player.items != null && PlayerController.player.items.Count > 0) {

        }
    }

    public static void getItem(ItemObject iobj, bool free) {
        if (PlayerController.player.items.Find(x => x.name.Equals(iobj.assetImage)) != null) {
            //PlayerController.player.items.Add(iobj);
            // Creating JSON
        } else {
            //PlayerController.player.items.Find(x => x.name.Equals(iobj.name)).amount ++;
        }

        if (!free) {
            PlayerController.player.gems -= iobj.costGems;
        }
    }

    public static void EquipSpecialBall (ItemObject iobj) {
        LevelController.SpecialBall = iobj.assetImage;
    }
}
