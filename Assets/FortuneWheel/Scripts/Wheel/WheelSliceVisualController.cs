using FortuneWheel.Scripts.UI.Visual;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FortuneWheel.Scripts.Utils;

namespace FortuneWheel.Scripts.Wheel
{
    public class WheelSliceVisualController : BaseVisualController<WheelItemData>
    {
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI itemAmountText;
        
        protected override void OnInitialize(WheelItemData data)
        {
            itemIcon.sprite = data.ItemSprite;

            if (data.DropCount > 0)
            {
                itemAmountText.SetFormattedNumber(data.DropCount);
            }
            else itemAmountText.SetText(string.Empty);
        }
        
        protected override void OnClear()
        {
           itemIcon.sprite = null;
           itemAmountText.SetText(string.Empty);
        }
    }
}