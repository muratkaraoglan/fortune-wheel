using System;
using System.Collections.Generic;
using System.Linq;
using FortuneWheel.Scripts.Item;
using UnityEngine;

namespace FortuneWheel.Scripts.Inventory
{
    public abstract class Inventory<T> : MonoBehaviour, IInventory<T> where T : IItem
    {
        [SerializeField] private int capacity = 30;

        private readonly List<InventorySlot<T>> _slots = new();
        public event Action<InventorySlot<T>> OnItemAdded;
        public event Action<InventorySlot<T>> OnItemRemoved;
        public event Action<InventorySlot<T>> OnSlotCreated;
        public event Action OnChanged;

        public int SlotCount => _slots.Count;
        public bool IsFull => _slots.Count >= capacity;

        public bool TryAdd(T item, int quantity = 1)
        {
            var existing = _slots.FirstOrDefault(s => !s.IsEmpty && s.Item.ItemID == item.ItemID);
            if (existing != null)
            {
                existing.Add(quantity);
                OnItemAdded?.Invoke(existing);
                OnChanged?.Invoke();
                return true;
            }

            if (IsFull) return false;

            var newSlot = new InventorySlot<T>();
            newSlot.Set(item, quantity);
            _slots.Add(newSlot);

            OnSlotCreated?.Invoke(newSlot);
            OnItemAdded?.Invoke(newSlot);
            OnChanged?.Invoke();
            return true;
        }

        public bool TryRemove(string itemId, int quantity = 1)
        {
            var slot = _slots.FirstOrDefault(s => !s.IsEmpty && s.Item.ItemID == itemId);
            if (slot == null || slot.Quantity < quantity) return false;

            slot.Remove(quantity);
            OnItemRemoved?.Invoke(slot);
            OnChanged?.Invoke();
            return true;
        }

        public bool Has(string itemId, int quantity = 1)
            => _slots.Any(s => !s.IsEmpty && s.Item.ItemID == itemId && s.Quantity >= quantity);

        public int GetQuantity(string itemId)
            => _slots.Where(s => !s.IsEmpty && s.Item.ItemID == itemId).Sum(s => s.Quantity);

        public IReadOnlyList<InventorySlot<T>> GetAllSlots() => _slots;

        public int IndexOf(string itemId)
            => _slots.FindIndex(s => !s.IsEmpty && s.Item.ItemID == itemId);
    }
}