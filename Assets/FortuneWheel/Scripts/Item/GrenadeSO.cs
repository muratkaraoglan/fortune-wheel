using FortuneWheel.Scripts.Item.Enums;
using UnityEngine;

namespace FortuneWheel.Scripts.Item
{
    [CreateAssetMenu(menuName = "Fortune Wheel/Item/New Grenade", fileName = "Grenade SO ")]
    public class GrenadeSO : ItemBaseSO
    {
        public override ItemType Type => ItemType.Consumable;

        [field: SerializeField] public CategoryType Category { get; private set; }
        [field: SerializeField] public GearSlotType GearSlotType { get; private set; }
        
        //Grenade stats
    }
}