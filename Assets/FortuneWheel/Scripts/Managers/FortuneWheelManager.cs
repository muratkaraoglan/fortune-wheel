using System.Collections.Generic;
using FortuneWheel.Scripts.Item;
using FortuneWheel.Scripts.UI.Visual;
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

        private WheelSpinController _spinController;
        private SliceSelector _sliceSelector;
        private WheelVisualController _visualController;
        private FailFlowHandler _failFlowHandler;

        private readonly List<WheelSliceItemData> _cachedSliceItems = new(8);
        private WheelZoneConfigSO _currentZone;

        private const int SliceItemCount = 8;
        private float _targetRotation;
        private int _currentSpinCount = 1;
        private int _selectedSliceIndex;
        private bool _canSpin;
        
        private void Awake()
        {
            InitializeSystems();
        }

        private void Start()
        {
            ResetWheel();
            spinButton.onClick.AddListener(Spin);
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnDestroy()
        {
            spinButton.onClick.RemoveListener(Spin);
            exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            FindButton("SpinButton", ref spinButton);
            FindButton("ExitButton", ref exitButton);

            failPanelController = transform.GetComponentInChildren<FailPanelController>();
            if (failPanelController == null)
                Debug.LogError("FailPanelController not found");
#endif
        }

        private void InitializeSystems()
        {
            _spinController = new WheelSpinController(wheelTransform, wheelSettingsConfig);
            _sliceSelector = new SliceSelector(_cachedSliceItems);
            _visualController = new WheelVisualController(
                wheelImage,
                wheelPointerImage,
                spinCountTextScroller,
                exitButton,
                sliceVisualControllers);
            _failFlowHandler = new FailFlowHandler(
                failPanelController,
                inventoryScrollViewTransform,
                inventoryDefaultHolder);
        }
        
#if UNITY_EDITOR
        private void FindButton(string buttonName, ref Button assignButton)
        {
            var btn = transform.Find(buttonName);
            if (btn != null) assignButton = btn.GetComponent<Button>();
            else Debug.LogError($"{buttonName} button not found");
        }
#endif
        
        // ──────────────────────────────────────────────
        // Spin Flow
        // ──────────────────────────────────────────────

        private void Spin()
        {
            if (!_canSpin) return;

            exitButton.interactable = false;
            SetSpinButtonState(false);
            _currentSpinCount++;

            _selectedSliceIndex = _sliceSelector.SelectWinningIndex();

#if UNITY_EDITOR
            Debug.Log($"[Wheel] Selected: {_cachedSliceItems[_selectedSliceIndex].DropItem.ItemName}");
#endif

            _spinController.Spin(_selectedSliceIndex, OnSpinComplete);
        }

        private void OnSpinComplete()
        {
            var selectedItem = _cachedSliceItems[_selectedSliceIndex];

            if (_currentZone.IsPenaltyItem(selectedItem))
                HandlePenalty(selectedItem);
            else
                HandleReward(selectedItem);
        }
        
        private void HandleReward(WheelSliceItemData item)
        {
            rewardDispatcher.Dispatch(item.DropItem, item.DropCount, () =>
            {
                SetWheelSlices();
                SetSpinButtonState(true);
            });
        }

        private void HandlePenalty(WheelSliceItemData item)
        {
            _failFlowHandler.OpenFailPanel(item.DropItem.Icon, OnFailPanelResult);
        }

        // ──────────────────────────────────────────────
        // Fail Flow
        // ──────────────────────────────────────────────

        private void OnFailPanelResult(FailPanelResult result)
        {
            if (result == FailPanelResult.GiveUp)
            {
                rewardDispatcher.RemoveAll();
                ResetWheel();
            }
            else
            {
                SetWheelSlices();
                SetSpinButtonState(true);
            }
        }

        // ──────────────────────────────────────────────
        // Wheel Setup
        // ──────────────────────────────────────────────

        private void ResetWheel()
        {
            _currentSpinCount = 1;
            SetWheelSlices();
            SetSpinButtonState(true);
        }

        private void SetWheelSlices()
        {
            _spinController.ResetRotation();
            _currentZone = wheelSettingsConfig.GetCurrentZone(_currentSpinCount);

            _currentZone.PopulateSliceItems(_cachedSliceItems, SliceItemCount, _currentSpinCount);

            _visualController.ApplyZoneVisuals(_currentZone);
            _visualController.UpdateSliceVisuals(_cachedSliceItems);
            _visualController.UpdateSpinCountDisplay(_currentSpinCount, _currentZone.IsSafeZone);
        }

        // ──────────────────────────────────────────────
        // Helpers
        // ──────────────────────────────────────────────

        private void SetSpinButtonState(bool canSpin)
        {
            _canSpin = canSpin;
            spinButton.interactable = canSpin;
        }

        private void OnExitButtonClicked()
        {
            rewardDispatcher.ClaimRewards();
            ResetWheel();
        }
    }
}