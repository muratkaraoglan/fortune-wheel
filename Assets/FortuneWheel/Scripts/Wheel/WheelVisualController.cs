using System.Collections.Generic;
using FortuneWheel.Scripts.UI.Visual;
using UnityEngine;
using UnityEngine.UI;

namespace FortuneWheel.Scripts.Wheel
{
    public class WheelVisualController
    {
        private static readonly Color SafeZoneColor = Color.green;
        private static readonly Color NormalZoneColor = new Color(.6f, .6f, .6f);
 
        private readonly Image _wheelImage;
        private readonly Image _wheelPointerImage;
        private readonly TextScroller _spinCountTextScroller;
        private readonly Button _exitButton;
        private readonly List<WheelSliceVisualController> _sliceVisualControllers;
 
        public WheelVisualController(
            Image wheelImage,
            Image wheelPointerImage,
            TextScroller spinCountTextScroller,
            Button exitButton,
            List<WheelSliceVisualController> sliceVisualControllers)
        {
            _wheelImage = wheelImage;
            _wheelPointerImage = wheelPointerImage;
            _spinCountTextScroller = spinCountTextScroller;
            _exitButton = exitButton;
            _sliceVisualControllers = sliceVisualControllers;
        }
 
        public void ApplyZoneVisuals(WheelZoneConfigSO zone)
        {
            _wheelImage.sprite = zone.WheelSprite;
            _wheelPointerImage.sprite = zone.WheelPointerSprite;
            _exitButton.interactable = zone.IsSafeZone;
        }
 
        public void UpdateSliceVisuals(List<WheelSliceItemData> sliceItems)
        {
            for (var i = 0; i < sliceItems.Count; i++)
                _sliceVisualControllers[i].Initialize(sliceItems[i]);
        }
 
        public void UpdateSpinCountDisplay(int spinCount, bool isSafeZone)
        {
            var color = isSafeZone ? SafeZoneColor : NormalZoneColor;
            _spinCountTextScroller.ScrollToValue(spinCount.ToString(), color);
        }
    }
}