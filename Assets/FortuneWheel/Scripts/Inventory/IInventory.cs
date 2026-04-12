using System.Collections.Generic;
using FortuneWheel.Scripts.Item;

namespace FortuneWheel.Scripts.Inventory
{
    public interface IInventory<T> where T : IItem
    {
        bool TryAdd(T item, int quantity = 1);
        bool TryRemove(string itemId, int quantity = 1);
        bool Has(string itemId, int quantity = 1);
        int GetQuantity(string itemId);
        IReadOnlyList<InventorySlot<T>> GetAllSlots();
        void RemoveAll();
    }
}