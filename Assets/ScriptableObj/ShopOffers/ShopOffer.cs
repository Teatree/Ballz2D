using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Scriptables", menuName = "ShopOffer", order = 3)]
public class ShopOffer : ScriptableObject {
    public string name;
    public string id;

    public bool enabled;

    [Header("All items that will be received upon purchase")]
    public ItemObject[] items;

    public void GivePlayerStuff() {
        foreach (ItemObject i in items) {
            ItemsController.getItem(i, true);
        }
    }
}