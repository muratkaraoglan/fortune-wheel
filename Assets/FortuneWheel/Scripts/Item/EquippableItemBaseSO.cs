using FortuneWheel.Scripts.Item.Enums;
using UnityEngine;

namespace FortuneWheel.Scripts.Item
{
     
    public abstract class EquippableItemBaseSO : ItemBaseSO
    {
        [field: SerializeField] public CategoryType Category { get; private set; }
        [field: SerializeField] public GearSlotType GearSlotType { get; private set; }
    }
}