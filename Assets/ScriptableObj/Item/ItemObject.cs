using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Scriptables", menuName = "Item", order = 2)]
public class ItemObject : ScriptableObject {
    public string name;
    public ItemType itemType;
    
    public int costGems;
    public int amount;
    public Sprite shopImage;
    public string assetImage;



    public enum ItemType { Ball, Booster, Currency };
}