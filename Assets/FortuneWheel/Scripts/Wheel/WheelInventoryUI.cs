using System.Collections.Generic;
using DG.Tweening;
using FortuneWheel.Scripts.Inventory;
using FortuneWheel.Scripts.Item;
using UnityEngine;

namespace FortuneWheel.Scripts.Wheel
{
    public class WheelInventoryUI : MonoBehaviour
    {
        [SerializeField] private WheelInventorySlotVisualController slotPrefab;
        [SerializeField] private Transform slotContainer;

        [SerializeField] private float spawnScaleDuration = 0.25f;

        private Inventory<ItemBaseSO> _inventory;
        private readonly List<WheelInventorySlotVisualController> _views = new();

        public bool SuppressRefresh { get; set; }

        public void Bind(Inventory<ItemBaseSO> inventory)
        {
            Unbind();
            _inventory = inventory;
            _inventory.OnSlotCreated += HandleSlotCreated;
            _inventory.OnChanged += Refresh;
        }

        private void Unbind()
        {
            if (_inventory == null) return;
            _inventory.OnSlotCreated -= HandleSlotCreated;
            _inventory.OnChanged -= Refresh;
        }

        private void HandleSlotCreated(InventorySlot<ItemBaseSO> slot)
        {
            var view = Instantiate(slotPrefab, slotContainer);
            _views.Add(view);

            view.transform.localScale = Vector3.zero;
            view.transform
                .DOScale(Vector3.one, spawnScaleDuration)
                .SetEase(Ease.OutBack);
            view.Initialize(new WheelInventorySlotVisualData()
            {
                slot = slot
            });
        }

        private void Refresh()
        {
            if (SuppressRefresh) return;

            var slots = _inventory.GetAllSlots();
            for (int i = 0; i < _views.Count; i++)
            {
                _views[i].Initialize(new WheelInventorySlotVisualData()
                {
                    slot = slots[i]
                });
            }
        }

        public WheelInventorySlotVisualController GetExistingViewForItem(string itemId)
        {
            int index = _inventory.IndexOf(itemId);
            if (index < 0 || index >= _views.Count) return null;
            return _views[index];
        }

        public WheelInventorySlotVisualController GetLastView() => _views.Count > 0 ? _views[^1] : null;

        private void OnDestroy() => Unbind();
    }
}