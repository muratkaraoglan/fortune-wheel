using FortuneWheel.Scripts.Item.Enums;
using UnityEngine;

namespace FortuneWheel.Scripts.Item
{
    public interface IItem
    {
        string ItemID { get; }
        string ItemName { get; }
        Sprite Icon { get; }
        ItemRarity Rarity { get; }
    }
}