using FortuneWheel.Scripts.Item.Enums;
using UnityEngine;

namespace FortuneWheel.Scripts.Item
{
    [CreateAssetMenu(menuName = "Fortune Wheel/Item/New Weapon", fileName = "Weapon SO ")]
    public class WeaponSO : EquippableItemBaseSO
    {
        public override ItemType Type => ItemType.Weapon;
        
        // weapon stats
    }
}