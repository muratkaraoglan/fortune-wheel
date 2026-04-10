using FortuneWheel.Scripts.Item.Enums;
using UnityEngine;

namespace FortuneWheel.Scripts.Item
{
    [CreateAssetMenu(menuName = "Fortune Wheel/Item/New Grenade", fileName = "Grenade SO ")]
    public class GrenadeSO : EquippableItemBaseSO
    {
        public override ItemType Type => ItemType.Consumable;
        
        //Grenade stats
    }
}