using System;
using FortuneWheel.Scripts.Item;
using UnityEngine;

namespace FortuneWheel.Scripts.Wheel
{
    [Serializable]
    public class WheelSliceItemData 
    {
        [field: SerializeField] public ItemBaseSO DropItem { get; private set; }
        [field: SerializeField, Range(0, 100)] public float DropChance { get; private set; }
        [field: SerializeField] public int DropCount { get; private set; }
    }
}