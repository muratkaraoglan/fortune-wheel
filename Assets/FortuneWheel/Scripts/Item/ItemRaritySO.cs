using FortuneWheel.Scripts.Item.Enums;
using UnityEngine;

namespace FortuneWheel.Scripts.Item
{
    [CreateAssetMenu(menuName = "Fortune Wheel/Item/New Rarity")]
    public class ItemRaritySO : ScriptableObject
    {
        [field: SerializeField] public ItemRarity Type { get; private set; }
        [field: SerializeField] public Color RarityColor { get; private set; }
    }
}