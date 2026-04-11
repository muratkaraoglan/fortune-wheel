using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using FortuneWheel.Scripts.Wheel;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

        private Button _spinButton;

        private readonly List<WheelSliceItemData> _cachedSliceItems = new(8);
        private WheelZoneConfigSO _currentZone;

        private const int SliceItemCount = 8;
        private const float AnglePerSlice = 45;
        private const float FullRotationDegrees = 360f;

        private float _targetRotation;
        private int _currentSpinCount = 1;
        private int _selectedSliceIndex;
        private bool _canSpin;

        private void Start()
        {
            ResetWheel();
            SetWheelSlices();
            SetSpinButtonState(true);
        }

        private void OnValidate()
        {
            var spinBtn = transform.Find("SpinButton").GetComponent<Button>();
            if (spinBtn != null)
            {
                _spinButton = spinBtn;
                _spinButton.onClick.AddListener(Spin);
            }
            else
            {
                Debug.LogError("Spin button not found");
            }
        }

        private void OnDestroy()
        {
            if (_spinButton != null)
            {
                _spinButton.onClick.RemoveListener(Spin);
            }
        }

        private void Spin()
        {
            if (!_canSpin) return;
            SetSpinButtonState(false);
            _currentSpinCount++;

            _selectedSliceIndex = SelectWinningSliceIndex();

            print(_cachedSliceItems[_selectedSliceIndex].DropItem.ItemName);

            var rotations = Random.Range(wheelSettingsConfig.MinRotation, wheelSettingsConfig.MaxRotation);
            _targetRotation = _selectedSliceIndex * AnglePerSlice;


            if (wheelSettingsConfig.ClockwiseRotation)
            {
                _targetRotation = -(FullRotationDegrees * rotations + (FullRotationDegrees - _targetRotation));
            }
            else
            {
                _targetRotation = FullRotationDegrees * rotations + _targetRotation;
            }

            wheelTransform.DORotate(new Vector3(0, 0, _targetRotation),
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

            for (var i = 0; i < SliceItemCount; i++)
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
            var selectedItem = _cachedSliceItems[_selectedSliceIndex];
            rewardDispatcher.Dispatch(selectedItem.DropItem, selectedItem.DropCount,
                () =>
                {
                    SetWheelSlices();
                    SetSpinButtonState(true);
                });
        }

        private void SetSpinButtonState(bool state)
        {
            _canSpin = state;
            _spinButton.interactable = _canSpin;
        }
    }
}