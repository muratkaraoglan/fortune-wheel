using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FortuneWheel.Scripts.Wheel
{
    public class SliceSelector
    {
        private readonly List<WheelSliceItemData> _sliceItems;
 
        public SliceSelector(List<WheelSliceItemData> sliceItems)
        {
            _sliceItems = sliceItems;
        }
 
        public int SelectWinningIndex()
        {
            var totalChance = _sliceItems.Sum(i => i.DropChance);
            var randomValue = Random.Range(0f, totalChance);
            var cumulative = 0f;
 
            for (var i = 0; i < _sliceItems.Count; i++)
            {
                cumulative += _sliceItems[i].DropChance;
                if (randomValue <= cumulative)
                    return i;
            }
 
            Debug.LogWarning("[SliceSelector] Fallback triggered. Check DropChance values.");
            return _sliceItems.Count - 1;
        }
    }
}