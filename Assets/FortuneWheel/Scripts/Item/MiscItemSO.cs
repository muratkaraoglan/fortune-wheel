using FortuneWheel.Scripts.Item.Enums;
using UnityEngine;

namespace FortuneWheel.Scripts.Item
{
    [CreateAssetMenu(menuName = "Fortune Wheel/Item/New Misc Item", fileName = "Misc Item SO ")]
    public class MiscItemSO : ItemBaseSO
    {
        public override ItemType Type => ItemType.Misc;
        [field: SerializeField] public bool IsPenalty { get; private set; }
    }
}