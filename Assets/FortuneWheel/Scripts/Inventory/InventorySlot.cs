using FortuneWheel.Scripts.Item;
using UnityEngine;

namespace FortuneWheel.Scripts.Inventory
{
    [System.Serializable]
    public sealed class InventorySlot<T> where T : IItem
    {
        public T Item { get; private set; }
        public int Quantity { get; private set; }
        
        public bool IsEmpty => Item == null || Quantity <= 0;

        public InventorySlot()
        {
        }

        public InventorySlot(T item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }

        public void Add(int amount)
        {
            Quantity += amount;
        }

        public void Remove(int amount)
        {
            Quantity = Mathf.Max(0, Quantity - amount);
            if (Quantity == 0) Item = default;
        }

        public void Set(T item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }

        public void Clear()
        {
            Item = default;
            Quantity = 0;
        }
    }
}