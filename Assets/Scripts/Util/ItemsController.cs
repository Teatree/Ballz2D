using UnityEngine;

public class ItemsController : MonoBehaviour {

    public static bool getItem(ItemObject iobj, bool free) {
        if (free || PlayerController.player.gems >= iobj.costGems) {
            PlayerController.player.gems = !free ? PlayerController.player.gems - iobj.costGems : PlayerController.player.gems;

            if (PlayerController.player.items.FindIndex(x => x.name.Equals(iobj.name)) < 0) {
                PlayerController.player.items.Add(new ItemData(iobj));
            }
            else {
                int i = PlayerController.player.items.FindIndex(x => x.name.Equals(iobj.name));
                PlayerController.player.items[i].amount += iobj.amount;
            }

            if (iobj.itemType == ItemObject.ItemType.Currency) {
                PlayerController.player.gems += iobj.amount;
            }

            Debug.Log("THere were some items claimed");
            return true;
        }
        return false;
    }

    public static void EquipSpecialBall(ItemObject iobj) {
        LevelController.SpecialBall = iobj.assetImage;
        PlayerController.player.specialBallImageName = iobj.assetImage;
        PlayerController.player.specialBallName = iobj.name;
        iobj.enabled = true;
        PlayerController.player.items.Find(x => x.name.Equals(iobj.name)).enabled = true;
    }
}
