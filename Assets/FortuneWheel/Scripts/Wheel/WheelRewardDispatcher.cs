using System;
using DG.Tweening;
using FortuneWheel.Scripts.Item;
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

        public void Dispatch(ItemBaseSO item, int quantity)
        {
            bool isNew = !inventory.Has(item.ItemID);

            // Her iki durumda da önce inventory güncelle
            inventory.TryAdd(item, quantity);

            // View'u al — yeni ise OnSlotCreated ile az önce doğdu
            WheelInventorySlotVisualController targetView = isNew
                ? inventoryUI.GetLastView()
                : inventoryUI.GetExistingViewForItem(item.ItemID);

            if (targetView == null) return;

            // Yeni item: counter 0'dan başlar, flyingler asıl değere taşır
            // Var olan item: counter mevcut değerdeydi, flyingler yeni değere taşır
            var displayStart = isNew
                ? 0
                : inventory.GetQuantity(item.ItemID) - quantity;

            targetView.Counter?.SetImmediate(displayStart);

            inventoryUI.SuppressRefresh = true;

            LaunchFlying(item, quantity, targetView, inventory.GetQuantity(item.ItemID));
        }

        private void LaunchFlying(ItemBaseSO item, int quantity, WheelInventorySlotVisualController targetView,
            int finalQuantity)
        {
            int effectiveCount = Mathf.Clamp(spawnCount, 1, quantity);
            int perIcon = quantity / effectiveCount;
            int remainder = quantity % effectiveCount;
            int arrived = 0;

            for (int i = 0; i < effectiveCount; i++)
            {
                int capturedIndex = i;

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

                        int bump = perIcon + (arrived == effectiveCount ? remainder : 0);
                        //targetView.IconImageRect.DOPunchScale(Vector3.one * .7f, .1f, 5, 0.5f);
                        if (bump > 0)
                        {
                            targetView.Counter?.BumpBy(bump);
                            targetView.Feedback?.Punch();
                        }

                        if (arrived == effectiveCount)
                            inventoryUI.SuppressRefresh = false;
                    }
                });
            }
        }
    }
}