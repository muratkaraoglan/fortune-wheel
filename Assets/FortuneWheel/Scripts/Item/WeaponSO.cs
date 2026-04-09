using FortuneWheel.Scripts.Item.Enums;
using UnityEngine;

namespace FortuneWheel.Scripts.Item
{
    [CreateAssetMenu(menuName = "Fortune Wheel/Item/New Weapon", fileName = "Weapon SO ")]
    public class WeaponSO : ItemBaseSO
    {
        public override ItemType Type => ItemType.Weapon;

        [field: SerializeField] public CategoryType Category { get; private set; }
        [field: SerializeField] public GearSlotType GearSlotType { get; private set; }
        // weapon stats
    }
}