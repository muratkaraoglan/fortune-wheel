using System;
using UnityEngine;

namespace FortuneWheel.Scripts.Managers
{
    public class FailFlowHandler
    {
        private readonly FailPanelController _failPanelController;
        private readonly Transform _inventoryScrollViewTransform;
        private readonly Transform _inventoryDefaultHolder;
 
        public FailFlowHandler(
            FailPanelController failPanelController,
            Transform inventoryScrollViewTransform,
            Transform inventoryDefaultHolder)
        {
            _failPanelController = failPanelController;
            _inventoryScrollViewTransform = inventoryScrollViewTransform;
            _inventoryDefaultHolder = inventoryDefaultHolder;
        }
 

        public void OpenFailPanel(Sprite icon, Action<FailPanelResult> onResult)
        {
            _inventoryScrollViewTransform.SetParent(_failPanelController.transform);
 
            _failPanelController.OpenFailPanel(icon, result =>
            {
                RestoreInventoryParent();
                onResult?.Invoke(result);
            });
        }
 
        private void RestoreInventoryParent()
        {
            _inventoryScrollViewTransform.SetParent(_inventoryDefaultHolder);
        }
    }
 
    public enum FailPanelResult
    {
        GiveUp,
        Continue
    }
}