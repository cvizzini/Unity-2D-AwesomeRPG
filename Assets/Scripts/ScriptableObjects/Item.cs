using UnityEngine;
// 1
[CreateAssetMenu(menuName = "Item")]
// 2
public class Item : ScriptableObject
{
    // 3
    public string objectName;
    // 4
    public Sprite sprite;
    // 5
    public int quantity;
    // 6
    public bool stackable;
    // 7
    public enum ItemType
    {
        COIN,
        HEALTH
    }
    public ItemType itemType;
}
