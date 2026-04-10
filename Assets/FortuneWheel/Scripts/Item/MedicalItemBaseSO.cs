using System.Collections.Generic;
using FortuneWheel.Scripts.Item.Enums;
using FortuneWheel.Scripts.Stat;
using UnityEngine;

namespace FortuneWheel.Scripts.Item
{
    [CreateAssetMenu(menuName = "Fortune Wheel/Medical/New Medical Item", fileName = "Medical SO ")]
    public class MedicalItemBaseSO : ItemBaseSO
    {
        public override ItemType Type => ItemType.Consumable;
        
        [field: SerializeField] public CategoryType Category { get; private set; }
        [field: SerializeField] public GearSlotType GearSlotType { get; private set; }
        [field:Header("")]
        [field: SerializeField] public float Duration { get; private set; }
        [field: SerializeField] public float Cooldown { get; private set; }

    
        [SerializeReference] public List<StatEffect> effects;
        
        public void Use(GameObject user)
        {
           
        }
    }
    
}