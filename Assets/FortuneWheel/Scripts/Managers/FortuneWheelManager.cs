using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using FortuneWheel.Scripts.Item;
using FortuneWheel.Scripts.UI.Visual;
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
        [SerializeField] private FailPanelController failPanelController;

        [Header("Inventory")]
        [SerializeField] private Transform inventoryDefaultHolder;
        [SerializeField] private Transform inventoryScrollViewTransform;
        
        [Header("Slices")] 
        [SerializeField] private List<WheelSliceVisualController> sliceVisualControllers;

        [Header("Visual")]
        [SerializeField] private Image wheelImage;
        [SerializeField] private Image wheelPointerImage;
        [SerializeField] private TextScroller spinCountTextScroller;
        [SerializeField] private Button spinButton;
        [SerializeField] private Button exitButton;

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
            spinButton.onClick.AddListener(Spin);
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnValidate()
        {
            FindButton("SpinButton",ref spinButton);
            FindButton("ExitButton", ref exitButton);

            failPanelController = transform.GetComponentInChildren<FailPanelController>();
            if (failPanelController == null) Debug.LogError("FailPanelController not found");
        }

        private void FindButton(string buttonName,ref Button assignButton)
        {
            var btn = transform.Find(buttonName);
            if (btn != null) assignButton = btn.GetComponent<Button>();
            else Debug.LogError($"{buttonName} button not found");
        }

        private void OnDestroy()
        {
            spinButton.onClick.RemoveListener(Spin);
            exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }

        private void Spin()
        {
            if (!_canSpin) return;
            exitButton.interactable = false;
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

        private void OnExitButtonClicked()
        {
            rewardDispatcher.ClaimRewards();
            ResetWheel();
        }

        private void ResetWheel()
        {
            _currentSpinCount = 1;
            SetWheelSlices();
            SetSpinButtonState(true);
        }

        private void SetWheelSlices()
        {
            wheelTransform.localRotation = Quaternion.Euler(0, 0, 0);
            
            _currentZone = wheelSettingsConfig.GetCurrentZone(_currentSpinCount);
            
            wheelImage.sprite = _currentZone.WheelSprite;
            wheelPointerImage.sprite = _currentZone.WheelPointerSprite;
            
            exitButton.interactable = _currentZone.IsSafeZone;
            
            _currentZone.PopulateSliceItems(_cachedSliceItems, SliceItemCount);

            for (var i = 0; i < SliceItemCount; i++)
            {
                sliceVisualControllers[i].Initialize(_cachedSliceItems[i]);
            }

            spinCountTextScroller.ScrollToValue(_currentSpinCount.ToString(),
                _currentZone.IsSafeZone ? Color.green : new Color(.6f, .6f, .6f));
        }

        private int SelectWinningSliceIndex()
        {
            var totalChance = _cachedSliceItems.Sum(i => i.DropChance);
            var randomValue = Random.Range(0f, totalChance);

            var cumulative = 0f;

            for (var i = 0; i < SliceItemCount; i++)
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

            if (selectedItem.DropItem is MiscItemSO { IsPenalty: true })
            {
                OnFail(selectedItem);
            }
            else
            {
                rewardDispatcher.Dispatch(selectedItem.DropItem, selectedItem.DropCount,
                    () =>
                    {
                        SetWheelSlices();
                        SetSpinButtonState(true);
                    });
            }
        }

        private void SetSpinButtonState(bool state)
        {
            _canSpin = state;
            spinButton.interactable = _canSpin;
        }

        private void OnFail(WheelSliceItemData selectedItem)
        {
            inventoryScrollViewTransform.SetParent(failPanelController.transform);
            failPanelController.OpenFailPanel(selectedItem.DropItem.Icon, OnFailPanelChoice);
        }

        private void OnFailPanelChoice(bool isGiveUp)
        {
            if (isGiveUp)
            {
                rewardDispatcher.RemoveAll();
                ResetWheel();
            }
            else
            {
                SetWheelSlices();
                SetSpinButtonState(true);
            }
            inventoryScrollViewTransform.SetParent(inventoryDefaultHolder);
        }
    }
}