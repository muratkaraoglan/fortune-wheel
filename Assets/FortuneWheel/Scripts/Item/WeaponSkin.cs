using FortuneWheel.Scripts.Item.Enums;
using UnityEngine;

namespace FortuneWheel.Scripts.Item
{
    [CreateAssetMenu(menuName = "Fortune Wheel/Skin/New Weapon Skin", fileName = "Weapon Skin SO ")]
    public class WeaponSkin : SkinBaseSO
    {
        [SerializeField] private WeaponSO targetItem;
        
        public override ItemType Type => ItemType.Skin;
        public override ItemBaseSO TargetItem => targetItem;
        
        //weapon skin stats
    }
}