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

        [SerializeField, Min(0)] private int baseDropCount;
        [SerializeField, Range(0, 3)] private float exponent;

        public int DropCount { get; private set; }

        public void CalculateDropCount(int spinCount)
        {
            DropCount = baseDropCount * Mathf.RoundToInt(Mathf.Pow(spinCount, exponent));
        }
    }
}