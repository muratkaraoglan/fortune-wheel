using System.Collections.Generic;
using FortuneWheel.Scripts.Item;
using FortuneWheel.Scripts.Utils;
using UnityEngine;

namespace FortuneWheel.Scripts.Wheel
{
    [CreateAssetMenu(menuName = "Fortune Wheel/Wheel/Wheel Zone Config")]
    public class WheelZoneConfigSO : ScriptableObject
    {
        [field: SerializeField] public bool IsSafeZone { get; private set; }
        [field: SerializeField] public Sprite WheelSprite { get; private set; }
        [field: SerializeField] public Sprite WheelPointerSprite { get; private set; }

        [Header("Guaranteed Items")] [SerializeField]
        private List<WheelSliceItemData> guaranteedItems;

        [Header("Random Pool")] [SerializeField]
        private List<WheelSliceItemData> randomItemPool;

        public void PopulateSliceItems(List<WheelSliceItemData> cachedListItem, int sliceCount)
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

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (!IsSafeZone) return;
 
            foreach (var wheelSliceItemData in guaranteedItems)
            {
                if (IsPenaltyItem(wheelSliceItemData))
                {
                    Debug.LogError("You can not add skull item the guaranteed list while zone is safe. Item deleted.");
                    guaranteedItems.Remove(wheelSliceItemData);
                    break;
                }
            }

            foreach (var wheelSliceItemData in randomItemPool)
            {
                if (IsPenaltyItem(wheelSliceItemData))
                {
                    Debug.LogError("You can not add skull item the random pool list while zone is safe. Item deleted.");
                    randomItemPool.Remove(wheelSliceItemData);
                    break;
                }
            }
#endif
        }

        public bool IsPenaltyItem(WheelSliceItemData wheelSliceItemData) =>
            wheelSliceItemData.DropItem is MiscItemSO { IsPenalty: true };
    }
}