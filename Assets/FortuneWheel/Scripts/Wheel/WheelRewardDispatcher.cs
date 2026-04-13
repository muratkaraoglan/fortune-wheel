using System;
using System.Collections;
using System.Collections.Generic;
using FortuneWheel.Scripts.Item;
using FortuneWheel.Scripts.Managers;
using FortuneWheel.Scripts.UI.Visual;
using UnityEngine;

namespace FortuneWheel.Scripts.Wheel
{
    public sealed class WheelRewardDispatcher : MonoBehaviour
    {
        [SerializeField] private WheelInventory inventory;
        [SerializeField] private WheelInventoryUI inventoryUI;
        [SerializeField] private FlyingItemVisual flyingItemPrefab;

        [SerializeField] private RectTransform flyingItemContainer;
        [SerializeField] private RectTransform wheelCenter;

        [Header("Fly settings")] [SerializeField, Range(1, 10)]
        private int spawnCount = 5;

        [SerializeField] private float flyDuration = 0.55f;
        [SerializeField] private float delayBetween = 0.07f;

        private void Awake()
        {
            inventoryUI.Bind(inventory);
        }

        public void Dispatch(ItemBaseSO item, int quantity, Action onDispatchComplete)
        {
            bool isNew = !inventory.Has(item.ItemID);

            inventory.TryAdd(item, quantity);

            WheelInventorySlotVisualController targetView = isNew
                ? inventoryUI.GetLastView()
                : inventoryUI.GetExistingViewForItem(item.ItemID);

            if (targetView == null) return;

            var displayStart = isNew
                ? 0
                : inventory.GetQuantity(item.ItemID) - quantity;

            targetView.Counter?.SetImmediate(displayStart);

            inventoryUI.SuppressRefresh = true;

            StartCoroutine(DelayedLaunch(item, quantity, targetView, onDispatchComplete));
            //LaunchFlying(item, quantity, targetView, onDispatchComplete);
        }

        IEnumerator DelayedLaunch(ItemBaseSO item, int quantity, WheelInventorySlotVisualController targetView,
            Action onComplete)
        {
            yield return null;
            LaunchFlying(item, quantity, targetView, onComplete);
        }

        private void LaunchFlying(ItemBaseSO item, int quantity, WheelInventorySlotVisualController targetView,
            Action onComplete)
        {
            var effectiveCount = Mathf.Clamp(spawnCount, 1, quantity);
            var perIcon = quantity / effectiveCount;
            var remainder = quantity % effectiveCount;
            var arrived = 0;

            for (var i = 0; i < effectiveCount; i++)
            {
                var capturedIndex = i;

                var flying = Instantiate(flyingItemPrefab, flyingItemContainer);
                flying.Initialize(new FlyingItemVisualData
                {
                    icon = item.Icon,
                    spawnPoint = wheelCenter,
                    targetPoint = targetView.IconImageRect,
                    duration = flyDuration,
                    delay = capturedIndex * delayBetween,
                    onArrived = () =>
                    {
                        arrived++;

                        var bump = perIcon + (arrived == effectiveCount ? remainder : 0);
                        //targetView.IconImageRect.DOPunchScale(Vector3.one * .7f, .1f, 5, 0.5f);
                        if (bump > 0)
                        {
                            targetView.Counter?.BumpBy(bump);
                            targetView.Feedback?.Punch();
                        }

                        if (arrived == effectiveCount)
                        {
                            inventoryUI.SuppressRefresh = false;
                            onComplete?.Invoke();
                        }
                    }
                });
            }
        }

        public void ClaimRewards()
        {
            if (inventory.SlotCount == 0) return;

            List<RewardVisualData> rewards = new(inventory.SlotCount);

            var inventorySlots = inventory.GetAllSlots();
            for (var i = 0; i < inventory.SlotCount; i++)
            {
                var slotData = inventorySlots[i];

                rewards.Add(new RewardVisualData()
                {
                    itemId = slotData.Item.ItemID,
                    itemName = slotData.Item.ItemName,
                    icon = slotData.Item.Icon,
                    quantity = slotData.Quantity,
                    rarity = slotData.Item.Rarity,
                });
            }

            RewardPanelManager.Instance.ShowRewardPanel(rewards);
            RemoveAll();
        }

        public void RemoveAll()
        {
            inventory.RemoveAll();
            inventoryUI.RemoveAll();
        }
    }
}