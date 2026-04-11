using FortuneWheel.Scripts.UI.Visual;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FortuneWheel.Scripts.Utils;

namespace FortuneWheel.Scripts.Wheel
{
    public class WheelSliceVisualController : BaseVisualController<WheelSliceItemData>
    {
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI itemAmountText;
        
        protected override void OnInitialize(WheelSliceItemData data)
        {
            itemIcon.sprite = data.DropItem.Icon;

            if (data.DropCount > 0)
            {
                itemAmountText.SetFormattedNumber(data.DropCount);
            }
            else itemAmountText.SetText(string.Empty);
        }
        
        protected override void OnClear()
        {
           itemIcon.enabled = false;
           itemAmountText.SetText(string.Empty);
        }
    }
}