using FortuneWheel.Scripts.Item.Enums;
using UnityEngine;

namespace FortuneWheel.Scripts.Item
{
    [CreateAssetMenu(menuName = "Fortune Wheel/Item/New Helmet", fileName = "Helmet SO ")]
    public class HelmetSO : ItemBaseSO
    {
        public override ItemType Type => ItemType.Helmet;
        [field: SerializeField] public CategoryType Category { get; private set; }
        [field: SerializeField] public GearSlotType GearSlotType { get; private set; }
        
        //Helmet stats
    }
}