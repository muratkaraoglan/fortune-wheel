using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using FortuneWheel.Scripts.Wheel;
using UnityEngine;
using UnityEngine.UI;

namespace FortuneWheel.Scripts.Managers
{
    public class FortuneWheelManager : MonoBehaviour
    {
        [SerializeField] private Transform wheelTransform;
        [SerializeField] private WheelSettingsConfigSO wheelSettingsConfig;
        [SerializeField] private WheelRewardDispatcher rewardDispatcher;
        [SerializeField] private List<WheelSliceVisualController> sliceVisualControllers;


        [Header("Visual")] [SerializeField] private Image wheelImage;
        [SerializeField] private Image wheelPointerImage;

        private readonly List<WheelSliceItemData> _cachedSliceItems = new(8);
        private WheelZoneConfigSO _currentZone;

        private const int SliceItemCount = 8;
        private const float AnglePerSlice = 45;
        private const float FullRotationDegrees = 360f;

        private int _currentSpinCount = 1;
        private int selectedSliceIndex;
        public float targetRotation;

        private void Start()
        {
            ResetWheel();
            SetWheelSlices();
        }

        [ContextMenu("Spin")]
        public void Spin()
        {
            _currentSpinCount++;

            selectedSliceIndex = SelectWinningSliceIndex();

            print(_cachedSliceItems[selectedSliceIndex].DropItem.ItemName);

            var rotations = Random.Range(wheelSettingsConfig.MinRotation, wheelSettingsConfig.MaxRotation);
            targetRotation = selectedSliceIndex * AnglePerSlice;


            if (wheelSettingsConfig.ClockwiseRotation)
            {
                targetRotation = -(FullRotationDegrees * rotations + (FullRotationDegrees - targetRotation));
            }
            else
            {
                targetRotation = FullRotationDegrees * rotations + targetRotation;
            }

            wheelTransform.DORotate(new Vector3(0, 0, targetRotation),
                    wheelSettingsConfig.SpinDuration, RotateMode.FastBeyond360)
                .SetEase(wheelSettingsConfig.SpinCurve)
                .OnComplete(OnSpinComplete);
        }

        private void ResetWheel()
        {
            _currentSpinCount = 1;
            wheelTransform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        private void SetWheelSlices()
        {
            wheelTransform.localRotation = Quaternion.Euler(0, 0, 0);
            _currentZone = wheelSettingsConfig.GetCurrentZone(_currentSpinCount);
            wheelImage.sprite = _currentZone.WheelSprite;
            wheelPointerImage.sprite = _currentZone.WheelPointerSprite;

            _currentZone.PopulateSliceItems(_cachedSliceItems, SliceItemCount);

            for (int i = 0; i < SliceItemCount; i++)
            {
                sliceVisualControllers[i].Initialize(_cachedSliceItems[i]);
            }
        }

        private int SelectWinningSliceIndex()
        {
            var totalChance = _cachedSliceItems.Sum(i => i.DropChance);
            var randomValue = Random.Range(0f, totalChance);

            var cumulative = 0f;

            for (int i = 0; i < SliceItemCount; i++)
            {
                cumulative += _cachedSliceItems[i].DropChance;
                if (randomValue <= cumulative)
                {
                    return i;
                }
            }

            return 0;
        }

        private void OnSpinComplete()
        {
            var selectedItem = _cachedSliceItems[selectedSliceIndex];
            rewardDispatcher.Dispatch(selectedItem.DropItem, selectedItem.DropCount);
            SetWheelSlices();
        }
    }
}