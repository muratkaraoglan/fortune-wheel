using System.Collections.Generic;
using FortuneWheel.Scripts.Utils;
using UnityEngine;

namespace FortuneWheel.Scripts.Wheel
{
    [CreateAssetMenu(menuName = "Fortune Wheel/Wheel Item Config")]
    public class WheelItemConfigSO : ScriptableObject
    {
        [field: SerializeField] public bool IsSafeZone { get; private set; }
        [field: SerializeField] public Sprite WheelSprite { get; private set; }
        [field: SerializeField] public Sprite WheelPointerSprite { get; private set; }

        [Header("Guaranteed Items")]
        [SerializeField] private List<WheelSliceItemData> guaranteedItems;

        [Header("Random Pool")]
        [SerializeField] private List<WheelSliceItemData> randomItemPool;
        
        public void PopulateSliceItems( List<WheelSliceItemData> cachedListItem,int sliceCount)
        {
            cachedListItem.Clear();
            
            var guaranteedCount = Mathf.Min(guaranteedItems.Count, sliceCount);
            for (var i = 0; i < guaranteedCount; i++)
            {
                cachedListItem.Add(guaranteedItems[i]);
            }
            
            var remaining = sliceCount - cachedListItem.Count;
            if (remaining > 0 && randomItemPool.Count > 0)
            {
                randomItemPool.Shuffle();
                var randomCount = Mathf.Min(remaining, randomItemPool.Count);
                for (var i = 0; i < randomCount; i++)
                {
                    cachedListItem.Add(randomItemPool[i]);
                }
            }
            
            cachedListItem.Shuffle();
        }
    }
}