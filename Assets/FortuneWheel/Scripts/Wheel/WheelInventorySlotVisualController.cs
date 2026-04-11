using FortuneWheel.Scripts.Inventory;
using FortuneWheel.Scripts.Item;
using FortuneWheel.Scripts.UI.Visual;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FortuneWheel.Scripts.Wheel
{
    public sealed class WheelInventorySlotVisualData
    {
        public InventorySlot<ItemBaseSO> slot;
    }

    public class WheelInventorySlotVisualController : BaseVisualController<WheelInventorySlotVisualData>
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI quantityText;

        public AnimatedCounter Counter { get; private set; }
        public PunchFeedback Feedback { get; private set; }

        private void Awake()
        {
            Counter = GetComponentInChildren<AnimatedCounter>();
            Feedback = GetComponentInChildren<PunchFeedback>();
        }

        protected override void OnInitialize(WheelInventorySlotVisualData data)
        {
            iconImage.enabled = true;
            iconImage.sprite = data.slot.Item.Icon;
            quantityText.SetText(data.slot.Quantity.ToString());
        }

        protected override void OnClear()
        {
            iconImage.enabled = false;
            quantityText.SetText(string.Empty);
        }
        
        public RectTransform IconImageRect => iconImage.rectTransform;
    }
}