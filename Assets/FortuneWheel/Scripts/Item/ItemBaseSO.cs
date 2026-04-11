using FortuneWheel.Scripts.Item.Enums;
using FortuneWheel.Scripts.Utils;
using UnityEngine;

namespace FortuneWheel.Scripts.Item
{
    public abstract class ItemBaseSO : ScriptableObject, IItem
    {
        [field: SerializeField, ReadOnly] public string ItemID { get; private set; }
        [field: SerializeField] public string ItemName { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public ItemRarity Rarity { get; private set; }

        public abstract ItemType Type { get; }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(ItemID))
            {
                ItemID = System.Guid.NewGuid().ToString();

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }
    }
}