using FortuneWheel.Scripts.Item.Enums;
using UnityEngine;

namespace FortuneWheel.Scripts.Item
{
    [CreateAssetMenu(menuName = "Fortune Wheel/Item/New Weapon Point",fileName = "Weapon Point SO ")]
    public class WeaponPointSO : ItemBaseSO
    {
        public override ItemType Type => ItemType.WeaponPoint;
        [field: SerializeField] public CategoryType TargetCategory { get; private set; }
    }
}